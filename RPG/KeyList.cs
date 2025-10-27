using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public  static class KeyBinds
    {
        private static Dictionary<string, ConsoleKey> bindings = new()
        {
            ["Up"] = ConsoleKey.W,
            ["Down"] = ConsoleKey.S,
            ["Left"] = ConsoleKey.A,
            ["Right"] = ConsoleKey.D,
            ["Inventory"] = ConsoleKey.I,
            ["Potions"] = ConsoleKey.P,
            ["EquipRight"] = ConsoleKey.Oem5,
            ["EquipLeft/Drink"] = ConsoleKey.Enter,
            ["DropSelected"] = ConsoleKey.Q,
            ["DropLeftHand"] = ConsoleKey.Z,
            ["DropRightHand"] = ConsoleKey.X,
            ["DropAll"] = ConsoleKey.P,
            ["Exit"] = ConsoleKey.Tab,
            ["LeaveInventory"] = ConsoleKey.Escape,
            ["Help"] = ConsoleKey.K,//
            ["Clear"] = ConsoleKey.L,
            ["Pickup"] = ConsoleKey.E,
            ["NormalAttack"] = ConsoleKey.D1,
            ["StealthAttack"] = ConsoleKey.D2,
            ["Magicttack"] = ConsoleKey.D3,
            ["Hand"] = ConsoleKey.H,
        };
        public static ConsoleKey GetKey(string action)
        {
            return bindings.ContainsKey(action) ? bindings[action] : ConsoleKey.NoName;
        }

        public static void SetKey(string action, ConsoleKey newKey)
        {
            bindings[action] = newKey;
        }
        public static string GetValue(ConsoleKey k){
            if(bindings.ContainsValue(k)){
                foreach(var key in bindings.Keys){
                   if (bindings[key] == k){
                    return key;
                    
                   }
                }
            }
            return "Unknown";
        }
    }
}
