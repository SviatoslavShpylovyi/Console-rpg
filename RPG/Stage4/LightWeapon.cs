using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class LightWeapon :Weapon, IDamageVisitable , IDefenceVisitable 
    {
        public LightWeapon(string name, string description, int dmg, bool one_hand) : base(name, description, dmg, one_hand) { }
        public override int Accept(IDamageVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override int Defence(IDefenceVisitor visitor)
        {
            return visitor.ApplyDefence(this);
        }
    }
}
