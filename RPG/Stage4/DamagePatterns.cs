using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class NormalAttack : IDamageVisitor, IDefenceVisitor , IAttackVisible
    {
        public NormalAttack(Player p)
        {
            this.p = p;

        }
        public Player p { get; set; }
        public int damage;
        public int defence;

        public (int,int )AcceptAttack(IAttackVisitor visitor)
        {
             return visitor.Attack(this);
        }

        public int ApplyDefence(Iitem item)
        {
            defence = p.Dexterity;
            return p.Dexterity;
        }

        public int ApplyDefence(LightWeapon weapon)
        {

            defence = p.Dexterity + p.Luck;
            return p.Dexterity + p.Luck;
        }

        public int ApplyDefence(HeavyWeapon weapon)
        {
            defence  = p.Strength + p.Luck;
            return p.Strength + p.Luck;
        }

        public int ApplyDefence(MagicWeapon weapon)
        {
            defence = p.Dexterity + p.Luck;
            return p.Dexterity + p.Luck;
        }

        public int Visit(LightWeapon weapon)
        {
            damage = weapon.Damage;
            return weapon.Damage;
        }

        public int Visit(HeavyWeapon weapon)
        {

            damage = weapon.Damage;
            return weapon.Damage;
        }

        public int Visit(MagicWeapon weapon)
        {
            damage = 1;
            return 1;
        }

        public int Visit(Iitem item)
        {
            damage = 0;
            return 0;
        }
    }
    public class StealthAttac : IDamageVisitor, IDefenceVisitor, IAttackVisible
    {
        public int damage;
        public int defence;
        public Player p { get; set; }

        public StealthAttac(Player p)
        {
            this.p = p;

        }
        public int ApplyDefence(Iitem item)
        {
            defence = 0;
            return 0;
        }

        public int ApplyDefence(LightWeapon weapon)
        {
            defence = p.Dexterity;
            return p.Dexterity;
        }

        public int ApplyDefence(HeavyWeapon weapon)
        {
            defence = p.Strength;
            return p.Strength;
        }

        public int ApplyDefence(MagicWeapon weapon)
        {

            defence = 0;
            return 0;
        }

        public int Visit(LightWeapon weapon)
        {
            damage = weapon.Damage * 2;
            return weapon.Damage * 2;
        }

        public int Visit(HeavyWeapon weapon)
        {
            damage = weapon.Damage / 2;
            return weapon.Damage / 2;
        }

        public int Visit(MagicWeapon weapon)
        {
            damage = 1;
            return 1;
        }

        public int Visit(Iitem item)
        {
            damage = 0;
            return 0;
        }

        public (int, int) AcceptAttack(IAttackVisitor visitor)
        {
            return visitor.Attack(this);
        }
    }
    public class MagicAttack : IDamageVisitor, IDefenceVisitor, IAttackVisible
    {
        public int damage;
        public int defence;
        public (int, int) AcceptAttack(IAttackVisitor visitor)
        {
            return visitor.Attack(this);
        }
        public Player p { get; set; }
        public MagicAttack(Player p)
        {
            this.p = p;

        }
        public int ApplyDefence(Iitem item)
        {
            defence = p.Luck;
            return p.Luck;
        }

        public int ApplyDefence(LightWeapon weapon)
        {
            defence = p.Luck;
            return p.Luck;
        }

        public int ApplyDefence(HeavyWeapon weapon)
        {
            defence = p.Luck;
            return p.Luck;
        }

        public int ApplyDefence(MagicWeapon weapon)
        {
            defence = p.Wisdom * 2;
            return p.Wisdom * 2;
        }

        public int Visit(LightWeapon weapon)
        {

            damage = 1;
            return 1;
        }

        public int Visit(HeavyWeapon weapon)
        {
            damage = 1;
            return 1;
        }

        public int Visit(MagicWeapon weapon)
        {
            damage = weapon.Damage;
            return weapon.Damage;
        }

        public int Visit(Iitem item)
        {
            damage = 0;
            return 0;
        }
    }

}
