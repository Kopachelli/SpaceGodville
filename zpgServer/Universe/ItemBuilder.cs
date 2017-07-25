using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public enum ItemClass
    {
        Loot,
        Equipment,
    }
    public class ItemBuilder : LootItem
    {
        public Quality rarity;
        public ItemClass itemClass;
        public string rawEquipSlot;
        public string rawLevel;
    }
}
