using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace zpgServer
{
    public static class Localization
    {
        static int currentLanguage = ru;

        const int en = 0;
        const int ru = 1;

        const int languageCount = 2;

        static int messageLinksGenerated = 0;
        static List<Dictionary<string, List<string>>> _library = new List<Dictionary<string, List<string>>>();
        public static void Load()
        {
            for (int i = 0; i < languageCount; i++)
            {
                _library.Add(new Dictionary<string, List<string>>());
            }

            //=================================================================================================================
            // Russian library
            //=================================================================================================================
            Add("missingno", "No localization");
            Add("moneySingular", "кредит");
            Add("moneyPlural", "кредиты");
            Add("moneyPluralA", "кредиты");
            Add("moneyPluralB", "кредита");
            Add("moneyPluralC", "кредитов");
            Add("newPilot", "Наткну{g:-лся|-лась} на странный заброшенный корабль в космосе. Судя по наружной обшивке, называется он \"{shipName}\". Что же, мне сейчас подойдет любой корабль, даже столь побитый.");
        }

        public static string Add(string text)
        {
            string index = messageLinksGenerated++.ToString();
            while (index.Length < 4) { index = "0" + index; }
            string link = "autogen_" + index;

            Add(link, text);
            return link;
        }
        public static void Add(string link, string text)
        {
            List<String> dataList;

            if (_library[currentLanguage].TryGetValue(link, out dataList))
            {
                dataList.Add(text);
            }
            else
            {
                dataList = new List<String>();
                dataList.Add(text);
                _library[currentLanguage].Add(link, dataList);
            }
        }

        public static List<string> Get(string link)
        {
            try
            {
                return _library[currentLanguage][link];
            }
            catch (KeyNotFoundException ex)
            {
                // If failed - fall back to default language
                if (currentLanguage != en)
                {
                    try { return _library[ru][link]; }
                    catch (KeyNotFoundException exc) { }
                }
                // Not found - return empty list
                return new List<String>();
            }
        }
        public static string GetRandom(string link)
        {
            List<string> data = Get(link);
            if (data.Count == 0)
                return "";
            else
                return data[Random.Get(0, data.Count)];
        }
        /* Список переменных:
         * {pilotName} - имя пилота
         * {shipName} - имя корабля
         * {planetName} - название активной планеты
         * {planetNameLong} - название планеты, включающее её тип (планета, станция, сектор)
         * {pilotMoney} - деньги пилота, название валюты включено
         * {lootMoney} - последняя полученная сумма денег
         * {lootItemA} - последний полученный предмет (первый)
         * {lootItemB} - последний полученный предмет (второй)
         * {lootItemC} - последний полученный предмет (третий)
         * {lootItemD} - последний полученный предмет (четвертый)
         *
         * Список условных выражений:
         * {g: male|female} - выбор по полу
         */
        public static String ParseForShip(Ship target, string text)
        {
            string output = text;
            string parsed = null;
            // Simple variables
            output = output.Replace("{pilotName}", target.pilot.name);
            output = output.Replace("{shipName}", target.name);
            output = output.Replace("{planetName}", target.planet.name);
            // Complex variables
                // Planet name long
            if (output.Contains("{planetNameLong}"))
            {
                string planetName = target.planet.name;
                string planetTitle = null;
                // Is station
                if ((target.planet.eventGroups & EventGroup.Station) > 0)
                    planetTitle = "станция";
                // Is planet
                if ((target.planet.eventGroups & EventGroup.Planet) > 0)
                    planetTitle = "планета";
                // Is space sector
                if ((target.planet.eventGroups & EventGroup.SpaceSector) > 0)
                    planetTitle = "сектор";

                // Add a whitespace
                if (planetTitle != null)
                    planetTitle = planetTitle + " ";

                output = output.Replace("{planetNameLong}", planetTitle + planetName);
            }
                // Loot money
            if (output.Contains("{lootMoney}"))
            {
                string lootMoney = target.pilot.lastReceivedMoney.ToString();
                string lootMoneyName = GetRandom("moneyPluralC");
                string lastChar = lootMoney.Substring(lootMoney.Length - 1);
                string secondToLastChar = "";
                if (lootMoney.Length >= 2) { secondToLastChar = lootMoney.Substring(lootMoney.Length - 2, 1); }
                if (lastChar == "1" && secondToLastChar != "1") { lootMoneyName = GetRandom("moneySingular"); }
                else if ((lastChar == "2" || lastChar == "3" || lastChar == "4") && secondToLastChar != "1")
                    { lootMoneyName = GetRandom("moneyPluralB"); }

                lootMoney += " " + lootMoneyName;
                output = output.Replace("{lootMoney}", lootMoney);
            }
            
            // Total money
            if (output.Contains("{pilotMoney}"))
            {
                string totalMoney = target.pilot.money.ToString();
                string totalMoneyName = GetRandom("moneyPluralC");
                string lastChar = totalMoney.Substring(totalMoney.Length - 1);
                string secondToLastChar = "";
                if (totalMoney.Length >= 2) { secondToLastChar = totalMoney.Substring(totalMoney.Length - 2, 1); }
                if (lastChar == "1" && secondToLastChar != "1") { totalMoneyName = GetRandom("moneySingular"); }
                else if ((lastChar == "2" || lastChar == "3" || lastChar == "4") && secondToLastChar != "1")
                    { totalMoneyName = GetRandom("moneyPluralB"); }

                totalMoney += " " + totalMoneyName;
                output = output.Replace("{pilotMoney}", totalMoney);
            }
            // Loot items
            if (output.Contains("{lootItemA}") && target.lastReceivedCargo.Count > 0)
                output = output.Replace("{lootItemA}", target.lastReceivedCargo[0].name);
            if (output.Contains("{lootItemB}") && target.lastReceivedCargo.Count > 1)
                output = output.Replace("{lootItemB}", target.lastReceivedCargo[1].name);
            if (output.Contains("{lootItemC}") && target.lastReceivedCargo.Count > 2)
                output = output.Replace("{lootItemC}", target.lastReceivedCargo[2].name);
            if (output.Contains("{lootItemD}") && target.lastReceivedCargo.Count > 3)
                output = output.Replace("{lootItemD}", target.lastReceivedCargo[3].name);

            // Conditionals
            // Gender
            while (output.IndexOf("{g:") != -1)
            {
                // Find information
                int pos = output.IndexOf("{g:");
                int len = output.IndexOf("}", pos) - pos;
                // Parse
                parsed = output.Substring(pos, len);
                if (target.pilot.gender == Gender.Male) { parsed = parsed.Substring(3, parsed.IndexOf("|") - 3); }
                else if (target.pilot.gender == Gender.Female) { parsed = parsed.Substring(parsed.IndexOf("|") + 1); }
                parsed = parsed.Replace("-", "");
                // Output
                output = output.Remove(pos, len + 1);
                output = output.Insert(pos, parsed);
            }

            return output;
        }
    }
}
