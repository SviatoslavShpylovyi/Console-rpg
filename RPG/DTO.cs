using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace RPG
{
    public class PlayerDTO
    {
        public char model { get; set; }
        public char HAS_LEFT{ get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Luck { get; set; }
        public int Wisdom { get; set; }
        public int Aggression { get; set; }
        public int Gold { get; set; }
        public int Coin { get; set; }
        public List<string> Hands { get; set; }
        public List<string> Inventory { get; set; }
        public List<string> Potions { get; set; }
        public int IsAtFight { get; set; }
        public List<string> ActivePotions { get; set; }
        public PlayerDTO() { }
        public PlayerDTO(Player p)
        {
            model = p.GetModel();
            X = p.GetX();
            Y = p.GetY();
            Health = p.Health;
            Strength = p.Strength;
            Dexterity = p.Dexterity;
            Luck = p.Luck;
            Wisdom = p.Wisdom;
            Aggression = p.Aggression;
            Coin = p.Coin;
            Gold = p.Gold;
            Hands = new List<string>
            {
                p.hands[0]?.Name ?? "None",
                p.hands[1]?.Name ?? "None"
            };
            Inventory = new List<string>();
            foreach (var item in p.inventory)
            {
                Inventory.Add(item.Name);
            }
            Potions = new List<string>();
            foreach (var item in p.potion_bag)
            {
                Potions.Add(item.Name);
            }
            ActivePotions = new List<string>();
            foreach (var item in p.active_potions.Values)
            {
                ActivePotions.Add(item.Name);
            }
            if (ActivePotions.Count == 0) { ActivePotions.Add("None"); }
            IsAtFight = p.IsAtFight;
            HAS_LEFT = p.HAS_LEFT == true ? 't' : 'f';
        }
    }
    public class WorldDTo
    {
        public List<PlayerDTO> Players { get; set; }
        public Dictionary<string, char> Symbols { get; set; }
        public List<string> DungeonGrid { get; set; } = new();
        public char IsAtFight = 'f';
        // "(x,y)": symbol
        public WorldDTo() { }
        public WorldDTo(World w)
        {
            Players = new List<PlayerDTO>();
            foreach (var p in w.Players.Values)
                Players.Add(new PlayerDTO(p));

            Symbols = new Dictionary<string, char>();
            foreach (var kvp in w.dung.Symbols)
            {
                string key = $"({kvp.Key.Item1},{kvp.Key.Item2})";
                Symbols[key] = kvp.Value;
            }
            char[,] grid = w.dung.dungeon;
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                char[] row = new char[cols];
                for (int j = 0; j < cols; j++)
                {
                    row[j] = grid[i, j];
                }
                DungeonGrid.Add(new string(row));
            }
        }

    }
    public class ActionDTO
    {
        public string Action { get; set; }
        public ActionDTO(string action)
        {
            Action = action;
        }
    }
    public class FirstMessage
    {
        public WorldDTo world { get; set; }
        public int playerID { get; set; }
        public FirstMessage(WorldDTo world, int playerID)
        {
            this.world = world;
            this.playerID = playerID;
        }
    }



}