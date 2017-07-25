using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace zpgServer
{
    public static class XmlManager
    {
        //===============================================================================
        // Generic
        //===============================================================================
        public static void Initialize()
        {

        }
        //===============================================================================
        // Account management
        //===============================================================================
        public static void SaveAccounts()
        {
            //StringBuilder output = new StringBuilder();
            MemoryStream output = new MemoryStream();

            bool savedToTempFile = false;
            StreamWriter file;
            if (File.Exists(Settings.playersXmlPath))
            {
                savedToTempFile = true;
                file = new StreamWriter(Settings.playersTempXmlPath);
            }
            else
            {
                file = new StreamWriter(Settings.playersXmlPath);
            }
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = true;
            ws.IndentChars = "\t";
            ws.Encoding = Encoding.UTF8;
            XmlWriter writer = XmlWriter.Create(output, ws);
            // Generate an XML
            writer.WriteStartDocument();
            writer.WriteStartElement("root");
            foreach (Player p in Authorization.playerList)
            {
                writer.WriteStartElement("player");

                writer.WriteStartElement("username");
                writer.WriteValue(p.username);
                writer.WriteEndElement();

                writer.WriteStartElement("password");
                writer.WriteValue(p.password);
                writer.WriteEndElement();

                writer.WriteStartElement("passwordSalt");
                writer.WriteValue(p.passwordSalt);
                writer.WriteEndElement();

                writer.WriteStartElement("loginCounter");
                writer.WriteValue(p.loginCounter);
                writer.WriteEndElement();

                writer.WriteStartElement("startingShipName");
                writer.WriteValue(p.startingShipName);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            output.Position = 0;
            //file.Write(output.Read);
            output.WriteTo(file.BaseStream);
            output.Close();
            output.Dispose();
            file.Close();
            if (savedToTempFile)
            {
                try
                {
                    File.Move(Settings.playersXmlPath, Settings.playersBackupXmlPath);
                    File.Move(Settings.playersTempXmlPath, Settings.playersXmlPath);
                    File.Delete(Settings.playersBackupXmlPath);
                }
                catch (Exception)
                {
                    ConsoleEx.Error("Account database update failed.");
                    return;
                }
            }
            ConsoleEx.Log("Account database updated successfully.");
            //ConsoleEx.Debug(output.ToString());
        }
        public static List<Player> ParseAccounts()
        {
            List<Player> outputList = new List<Player>();
            if (!File.Exists(Settings.playersXmlPath))
                return outputList;

            FileStream file = File.Open(Settings.playersXmlPath, FileMode.Open);
            XmlReader reader = XmlReader.Create(file);

            string lastElement = null;
            string username = null, password = null, passwordSalt = null, loginCounter = null, startingShipName = null;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        lastElement = reader.Name;
                        break;
                    case XmlNodeType.Text:
                        switch (lastElement)
                        {
                            case "username":
                                username = reader.Value;
                                break;
                            case "password":
                                password = reader.Value;
                                break;
                            case "passwordSalt":
                                passwordSalt = reader.Value;
                                break;
                            case "loginCounter":
                                loginCounter = reader.Value;
                                break;
                            case "startingShipName":
                                startingShipName = reader.Value;
                                break;
                        }
                        break;
                    case XmlNodeType.XmlDeclaration:
                    case XmlNodeType.ProcessingInstruction:
                        break;
                    case XmlNodeType.Comment:
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Name == "player" && username != null && password != null && passwordSalt != null)
                        {
                            Player player = new Player(username, password, passwordSalt, true);
                            if (loginCounter != null) { player.loginCounter = Int32.Parse(loginCounter); }
                            if (startingShipName != null) { player.startingShipName = startingShipName; }
                            outputList.Add(player);

                            username = null; password = null; passwordSalt = null; loginCounter = null; startingShipName = null;
                        }
                        break;
                }
            }
            file.Close();
            return outputList;
        }
        //===============================================================================
        // Event management
        //===============================================================================
        public static List<EventBuilder> ParseEvents(string filename)
        {
            List<EventBuilder> outputList = new List<EventBuilder>();

            if (!File.Exists(filename))
                return outputList;

            FileStream file = File.Open(filename, FileMode.Open);
            XmlReader reader = XmlReader.Create(file);

            string lastElement = null;
            EventBuilder eventBuilder = new EventBuilder(filename);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        lastElement = reader.Name;
                        break;
                    case XmlNodeType.Text:
                        switch (lastElement)
                        {
                            case "text":
                                eventBuilder.text = reader.Value;
                                break;
                            case "eventGroup":
                                eventBuilder.rawEventGroup = reader.Value;
                                break;
                            case "eventSpecial":
                                eventBuilder.rawEventSpecial = reader.Value;
                                break;
                            case "chance":
                                Int32.TryParse(reader.Value, out eventBuilder.chance);
                                break;
                            case "exp":
                                Int32.TryParse(reader.Value, out eventBuilder.reward.exp);
                                break;
                            case "stamina":
                                Int32.TryParse(reader.Value, out eventBuilder.reward.stamina);
                                break;
                            case "curiosity":
                                Int32.TryParse(reader.Value, out eventBuilder.reward.curiosity);
                                break;
                            case "money":
                                eventBuilder.rawRewardMoney = reader.Value;
                                break;
                            case "storyAdvance":
                                eventBuilder.rawStoryAdvance = reader.Value;
                                break;
                            case "loot":
                                eventBuilder.rawRewardLoot = reader.Value;
                                break;
                            case "pilotGender":
                                eventBuilder.rawFilterPilotGender = reader.Value;
                                break;
                            case "pilotMoney":
                                eventBuilder.rawFilterPilotMoney = reader.Value;
                                break;
                            case "storyStage":
                                eventBuilder.rawFilterStoryStage = reader.Value;
                                break;
                        }
                        break;
                    case XmlNodeType.XmlDeclaration:
                    case XmlNodeType.ProcessingInstruction:
                        break;
                    case XmlNodeType.Comment:
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Name == "event")
                        {
                            eventBuilder.ParseRawData();
                            outputList.Add(eventBuilder);

                            eventBuilder = new EventBuilder(filename);
                        }
                        break;
                }
            }
            file.Close();

            return outputList;
        }
        //===============================================================================
        // Item management
        //===============================================================================
        public static List<ItemBuilder> ParseItems(string filename)
        {
            List<ItemBuilder> outputList = new List<ItemBuilder>();

            if (!File.Exists(filename))
                return outputList;

            FileStream file = File.Open(filename, FileMode.Open);
            XmlReader reader = XmlReader.Create(file);

            string lastElement = null;
            Quality itemQuality = Quality.None;
            ItemBuilder itemBuilder = new ItemBuilder();
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case "common":
                                itemQuality = Quality.Common;
                                break;
                            case "rare":
                                itemQuality = Quality.Rare;
                                break;
                            case "epic":
                                itemQuality = Quality.Epic;
                                break;
                            case "legendary":
                                itemQuality = Quality.Legendary;
                                break;
                        }
                        lastElement = reader.Name;
                        break;
                    case XmlNodeType.Text:
                        switch (lastElement)
                        {
                            case "item":
                            case "name":
                                itemBuilder.name = reader.Value;
                                break;
                            case "abilityId":
                                itemBuilder.abilityId = reader.Value;
                                break;
                        }
                        break;
                    case XmlNodeType.XmlDeclaration:
                    case XmlNodeType.ProcessingInstruction:
                        break;
                    case XmlNodeType.Comment:
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Name == "item")
                        {
                            itemBuilder.rarity = itemQuality;
                            itemBuilder.itemClass = ItemClass.Loot;

                            outputList.Add(itemBuilder);
                            itemBuilder = new ItemBuilder();
                        }
                        break;
                }
            }
            file.Close();

            return outputList;
        }
    }
}
