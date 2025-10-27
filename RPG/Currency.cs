using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public interface ICurrency : Iitem
    {
        public int Value { get; }
    }
    public abstract class Currency : ICurrency
    {
        public int Value { get; }

        public bool IsUsable => false;

        public string Name { get; }

        public string Description {  get; }

        public abstract void _Pickup(Player p);

        public void Equip(int hand, Player p) { return; }

        public void Unequip(int hand, Player p)
        {
            return;
        }

        public Currency(int value, string name, string description)
        {
            Value = value;
            Name = name;
            Description = description;
        }
    }
    public class Gold : Currency
    {
        public Gold(int value) : base(value, "Gold", $" A valuable resource for shops{value}") { }
        public override void _Pickup(Player p)
        {
            p.Add_Gold(this);
        }
    }
    public class Coin : Currency
    {
        public Coin(int value) : base (value, "Coin", $" A nice currency for buying elixirs and items {value}") { }
        public override void _Pickup(Player p)
        {
            p.Add_Coin(this);
        }
    }
}
