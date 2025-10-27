using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public interface IDamageVisitor
    {
        Player p { get; set; }
        public int Visit(Iitem item);
        public int Visit(LightWeapon weapon);
        public int Visit(HeavyWeapon weapon);
        public int Visit(MagicWeapon weapon);
    }
    public interface IDamageVisitable
    {
        public abstract int Accept(IDamageVisitor visitor);
    }
    public interface IDefenceVisitor {

        public int ApplyDefence(Iitem item);
        public int ApplyDefence(LightWeapon weapon);
        public int ApplyDefence(HeavyWeapon weapon);
        public int ApplyDefence(MagicWeapon weapon);
    }
    public interface IDefenceVisitable
    {
        public abstract int Defence(IDefenceVisitor visitor);
    }
    public interface IAttackVisitor
    { 
        public (int, int) Attack(NormalAttack weapon);
        public (int, int) Attack(StealthAttac weapon);
        public (int, int) Attack(MagicAttack weapon);
    }
    public interface IAttackVisible
    {
        public (int,int )AcceptAttack(IAttackVisitor visitor);
    }

}
