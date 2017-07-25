using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public enum Quality
    {
        None = -1,
        Common = 0,
        Rare = 1,
        Epic = 2,
        Legendary = 3,
    }
    public static class Looter
    {
        static List<List<LootItem>> _lootLibrary = new List<List<LootItem>>();

        public static void LoadItems()
        {
            foreach (Quality q in Enum.GetValues(typeof(Quality))) { _lootLibrary.Add(new List<LootItem>()); }

            // Parse the loot
            foreach (string path in Settings.itemsXmlPaths)
            {
                List<ItemBuilder> parsedItems = XmlManager.ParseItems(path);
                foreach (ItemBuilder parsedItem in parsedItems)
                {
                    //_lootLibrary.Add(parsedItem);
                    if (parsedItem.itemClass == ItemClass.Loot)
                        Add(parsedItem.rarity, parsedItem.name, parsedItem.abilityId);
                }
                if (parsedItems.Count > 0)
                    ConsoleEx.Log("Successfully parsed " + parsedItems.Count + " item(s) in " + path.Substring(path.LastIndexOf("/") + 1));
                else
                    ConsoleEx.Log("No valid items found in " + path.Substring(path.LastIndexOf("/") + 1));
            }
        }
        public static void Add(Quality quality, string name, string abilityId = null)
        {
            LootItem item = new LootItem();
            item.name = name;
            item.abilityId = abilityId;
            _lootLibrary[(int)quality].Add(item);
        }
        public static LootItem GetRandom(Quality quality)
        {
            LootItem item = _lootLibrary[(int)quality][Random.Get(0, _lootLibrary[(int)quality].Count)];
            LootItem newItem = LootItem.Generate(item);
            return newItem;
        }
    }
}
