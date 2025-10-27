using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
namespace RPG
{
    public class Player : IObserverPotion
    {
        public  bool HAS_LEFT = false;
        public  char player_model = '¶';
        private int posX = 0;
        private int posY = 0;
        public int Strength = 5;
        public int Dexterity = 5;
        public int Health = 100;
        public int Luck = 5;
        public int Aggression = 5;
        public int Wisdom = 5;
        public int Gold = 0;
        public int Coin = 0;
        private int Steps = 0;
        public int Defence = 0;
        public int IsAtFight = 0;
        public List<Potion> potion_bag;
        public readonly Iitem[] hands = new Iitem[2];
        public Dictionary< int , Potion> active_potions;
        public List<Iitem> inventory;
        public readonly string Name;
        public Enemy? Current_Enemy;
        public bool isPlaced = false;
        public Func<(int,int), Iitem, bool> OnDropItem;
        public List<Iitem> GetList() { return inventory; }
        public Player(char model) { this.player_model = model; this.inventory = new List<Iitem>();  this.potion_bag = new List<Potion>() ;  this.active_potions = new Dictionary<int, Potion>(); }
        public void DrinkPotion(int index) {
            potion_bag[index].Start(this);
            active_potions.Add(potion_bag[index].Pot_ID, potion_bag[index]);
            potion_bag.RemoveAt(index);
        }
        public void Step()
        {
            Steps++;
            foreach(var active in active_potions.Values)
            {
                active.OnStep(this);
            }
        }
        // public int Move(Dungeon dung )
        // {
        //     KeyBilder ks = new KeyBilder();
        //     MovementKeys k = ks.BuildKeyHandle(dung, this);
        //     Display_info display = Display_info.GetInstance(this, (50, 2));
        //     ConsoleKey my_string = Console.ReadKey(true).Key;
        //     (int, int) pos = k.KeyPressed(my_string, this);
        //     Console.SetCursorPosition(32, 3);
        //    // Console.Write($"{pos.Item1},{pos.Item2}");
        //     if (pos.Item1 < 0)
        //     {
        //         display.DisplayInstruction(dung);
        //         return 1;
        //     }
        //     if (pos.Item2 < 0)
        //     {
        //         // Console.WriteLine("Picking UP");
        //         Pickup(dung.Items_in_world, dung.Symbols);

        //         return 1;
        //     }
        //     if (dung.dungeon[pos.Item2,pos.Item1] == '█')
        //     {
        //         return 0;
        //     }

        //     posX = pos.Item1;
        //     posY = pos.Item2;
        //     Step();
        //     return 1;

        // }
        public int Move_MVC(Dungeon dung, string Action)
        {
            KeyBilder ks = new KeyBilder();
            MovementKeys k =   ks.BuildKeyHandle(dung, this);
             (int, int) pos = k.KeyPressed(Action, this);
             Console.SetCursorPosition(32, 3);
            // Console.Write($"{pos.Item1},{pos.Item2}");
             if (pos.Item1 < 0)
             {
                 return 1;
             }
             if (pos.Item2 < 0)
             {
                // Console.WriteLine("Picking UP");
                Pickup(dung.Items_in_world, dung.Symbols);
                 return 1;
             }
            if (dung.dungeon[pos.Item2, pos.Item1] == '█')
            {
                return 0;
            }
             posX = pos.Item1;
             posY = pos.Item2;
             Step();
             return 1;

        }
        public void Add_item_to_PotionBag(Potion pot)
        {
            potion_bag.Add(pot);
        }
        
        public void Add_item_to_inventory(Iitem item)
        {
            inventory.Add(item);
        }
        public void Pickup(Dictionary<(int, int), Iitem> ItemsLocation, Dictionary<(int, int), char> Sybols)
        {

            var coordinates = (posX, posY);
            if (ItemsLocation.TryGetValue(coordinates, out Iitem item))
            {
                item._Pickup(this);
                ItemsLocation.Remove(coordinates);
                Sybols.Remove(coordinates);
            }
            
        }
        public void Add_Gold(Gold currency)
        {
            Gold += currency.Value;
        }
        public void Add_Coin(Coin currency)
        {
            Coin += currency.Value;
        }
       // public void DrinkPotion() { } // implement
        public bool Equip_to_hands(int hand, int index)
        {
            if (hands[hand]!= null)
            {
                hands[hand].Unequip(hand, this);
            }
           if(inventory.Count ==0)
                return false;

            inventory[index].Equip(hand, this);
            inventory.RemoveAt(index);
            return true;

        }

        public char GetModel()
        {
            return player_model;
        }
        public List<Iitem> GET_list()
        {
            return inventory;
        }
        public void DropItem(int index)
        {
            inventory.RemoveAt(index);
        }
        public void DropPotion(int index)
        {
            potion_bag.RemoveAt(index);
        }
        public void DropAll()
        {
            var position = (posX, posY);
            var dropPositions = GetTiles(position);

            int dropIndex = 0;
            for (int i = 0; i < 2; i++)
            {
                if (hands[i] != null && dropIndex < dropPositions.Count)
                {
                    var pos = dropPositions[dropIndex++];
                    if (OnDropItem.Invoke(pos, hands[i]))
                    {
                        hands[i].Unequip(i, this);
                        hands[i] = null;
                    }
                }
            }

            for (int i = inventory.Count - 1; i >= 0 && dropIndex < dropPositions.Count; i--)
            {
                var pos = dropPositions[dropIndex++];
                if (OnDropItem.Invoke(pos, inventory[i]))
                    inventory.RemoveAt(i);
            }

            for (int i = potion_bag.Count - 1; i >= 0 && dropIndex < dropPositions.Count; i--)
            {
                var pos = dropPositions[dropIndex++];
                if (OnDropItem.Invoke(pos, potion_bag[i]))
                    potion_bag.RemoveAt(i);
            }
        }
        public List<(int, int)> GetTiles((int x, int y) center)
        {
            List<(int, int)> tiles = new();
            for (int i = -1; i <= 2; i++)
            {
                for (int j = -1; j <= 2; j++)
                {
                    var pos = (center.x + i, center.y + j);
                        tiles.Add(pos);
                }
            }
            return tiles;
        }
        public int Path((int,int) e )
        {
           return Math.Abs(posX - e.Item1) + Math.Abs(posY - e.Item2);
        }
        public int GetX() { return  posX; }
        public int GetY() { return posY;}
        public int GetSteps() { return Steps; }

        public void OnEndEffect( int Id)
        {
            active_potions.Remove(Id);
        }
    }
}
 