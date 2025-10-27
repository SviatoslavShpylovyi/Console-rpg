using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public interface Iitem
    {
        public bool IsUsable { get; }
        public void  Equip(int hand, Player p );
        //the base interface and then build from it 
        public void Unequip(int hand, Player p );
        public string Name { get; }
        public string Description { get; }
        public void _Pickup(Player p);
    }
    /// /////////////////////////////////////////////////////////
    public interface IWeapon : Iitem
    {
        public bool Is_Onehanded { get; }
        public int Damage { get; }
    }
    /////////////////////////////////////////////////////////
    public abstract class Weapon : IWeapon ,  IDamageVisitable, IDefenceVisitable
    {
        public bool Is_Onehanded { get; protected set; }

        public int Damage { get; protected set; }
        public string Description { get; protected set; }
        public string Name { get; set; }


        public bool IsUsable => true;
        public void Equip(int hand, Player p)
        {
            if (Is_Onehanded)
            {
                //p.Add_item_to_inventory(p.hands[hand]);
                p.hands[hand] = this;
            }
            else
            {
                //p.Add_item_to_inventory(p.hands[0]);
               // p.Add_item_to_inventory(p.hands[1]);
                p.hands[0] = this;
                p.hands[1] = this;
            }
        }

        public void _Pickup(Player p)
        {
            p.Add_item_to_inventory(this);
        }

        public void Unequip(int hand, Player p)
        {
            if (p.hands[hand] == null)
            {
                return;
            }
            if (Is_Onehanded)
            {
                p.Add_item_to_inventory(p.hands[hand]);
               // p.hands[hand] = this;
            }
            else
            {
                p.Add_item_to_inventory(p.hands[0]);
                p.hands[1] = null;
               // p.Add_item_to_inventory(p.hands[1]);
            }
        }

        public abstract int Accept(IDamageVisitor visitor);


        public abstract int Defence(IDefenceVisitor visitor);

        public Weapon(string name, string description, int dmg, bool one_hand)
        {
            Name = name;
            Description = description;
            Damage = dmg;
            Is_Onehanded = one_hand;
        }
    }
    /////////////////////////////////////////////////////////
    public class Sword : LightWeapon
    {
        public Sword() : base("Sword", "A sharp blade", 10, true) { }
    }

    public class Axe : MagicWeapon // for testing
    {
        public Axe() : base("Axe", "A battle axe", 11, true) { }
    }
    public class GreateSword : HeavyWeapon
    {
        public GreateSword() : base("Great Sword", "A two-handed weapon with big damage", 23, false) { }
    }
    /////////////////////////////////////////////////////////
    public class Unusable : Iitem
    {
        public bool IsUsable => false;
        public string Name { get; set; }
        public string Description { get; set; }
        public Unusable(string name, string descriprion = "no Description")
        {
            Name = name;
            Description = descriprion;
        }
        public void Equip(int hand, Player p) { p.Add_item_to_inventory(this); }

        public void _Pickup(Player p)
        {
            p.Add_item_to_inventory(this);
        }

        public void Unequip(int hand, Player p)
        {
            return;
        }
    }
    public class Vase : Unusable
    {
        public Vase() : base("Ancient Vase", "An ancient vase with strange ornament") { }
    }
    public class RustyCoin : Unusable
    {
        public RustyCoin() : base("Rusty Coin", "An old rusty coin which is no longer in use") { }
    }
    public class Bone : Unusable
    {
        public Bone() : base("Human bone", "A piece of the previous adventurer") { }
    }
    /////////////////////////////////////////////////////////
    public abstract class ItemEffect : Iitem
    {
        protected Iitem item;
        public virtual string Name => item.Name;
        public virtual string Description => item.Description;
        public virtual bool IsUsable => item.IsUsable;
        public ItemEffect(Iitem item , int modifier_index = 0 )
        {
            this.item = item;
        }
        public virtual void Equip(int hand, Player p)
        {
           item.Equip(hand, p);
            ApplyEffect(p);
        }
        public virtual void Unequip(int hand, Player p)
        {
            item.Unequip(hand, p);
            RemoveEffect(p);
        }
        public virtual void ApplyEffect(Player player) {
        }
        public virtual void RemoveEffect(Player player) {}

        public void _Pickup(Player p)
        {
            p.Add_item_to_inventory(this);
        }
    }
    /////////////////////////////////////////////////////////
    public abstract class WeaponEffect : ItemEffect, IWeapon
    {
        protected IWeapon weapon;
        public WeaponEffect(IWeapon weapon ) : base(weapon)
        {
            this.weapon = weapon;
        }
        public virtual int Damage =>weapon.Damage;
        public  override void  Equip(int hand, Player p )
        {
            if (Is_Onehanded)
            {
                //p.Add_item_to_inventory(p.hands[hand]);
                p.hands[hand] = this;
            }
            else
            {
                //p.Add_item_to_inventory(p.hands[0]);
                // p.Add_item_to_inventory(p.hands[1]);
                p.hands[0] = this;
                p.hands[1] = this;
            }
            ApplyEffect(p);
        }
        public override void Unequip(int hand, Player p)
        {
            if (p.hands[hand] == null)
            {
                return;
            }
            if (Is_Onehanded)
            {
                p.Add_item_to_inventory(p.hands[hand]);
                // p.hands[hand] = this;
            }
            else
            {
                p.Add_item_to_inventory(p.hands[0]);
                p.Add_item_to_inventory(p.hands[1]);
            }
            RemoveEffect(p);
        }
        public virtual bool Is_Onehanded => weapon.Is_Onehanded;
    }
    /// ///////////////////////////////////////////////////////
    public class Lucky : WeaponEffect , IWeapon
    {
        public override string Name => item.Name + " (Lucky)";
        public Lucky(Weapon item) : base(item) { }// is this 


        public override int Damage => weapon.Damage + 5;
        public override void ApplyEffect(Player player)
        {
            base.ApplyEffect(player);
            player.Luck += 5;
        }
        public override void RemoveEffect(Player player)
        {
            player.Luck -= 5;
            base.RemoveEffect(player);
        }

    }
    public class PowerfulEffect : WeaponEffect
    {
        public override string Name => weapon.Name + "(Powerful)";

        public PowerfulEffect(IWeapon weapon) : base(weapon) { }

        public override int Damage => weapon.Damage + 5;
    }
    

}


