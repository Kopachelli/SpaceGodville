using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public class LootItem
    {
        public string name;
        public string abilityId;
        public int shopValue;

        public static LootItem Generate(LootItem prototype)
        {
            LootItem output = new LootItem();
            output.name = prototype.name;
            output.abilityId = prototype.abilityId;
            output.shopValue = Random.Get(85, 185);
            if (Random.Percentage() <= 0.15f) { output.shopValue += Random.Get(25, 350); }
            if (Random.Percentage() <= 0.15f) { output.shopValue = (int)(output.shopValue * (Random.Get(15, 50) / 100.00f)); }
            return output;
        }
    }
}
