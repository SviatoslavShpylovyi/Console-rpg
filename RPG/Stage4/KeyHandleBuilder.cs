using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class KeyBilder
    {
         public MovementKeys BuildKeyHandle( Dungeon dung, Player p )
        {
            if (dung.Items_in_world.Count() > 0)
            {
                var k = new MovementKeys();
                var k1 = new PickUpKey();
                var k2 = new InvetoryEquip();
                var k3 = new InventoryDropItem();
                var k4 = new PotionDrink();
                var k5 = new PotionDrop();
                var k6 = new DropHandItemKey();
                var k7 = new DropAllItems();
                var k8 = new EndGame();
                var k9 = new UnknowKey();

                k.NextKey(k1, p);
                k1.NextKey(k2, p);
                k2.NextKey(k3, p);
                k3.NextKey(k4, p);
                k4.NextKey(k5, p);
                k5.NextKey(k6, p);
                k6.NextKey(k7, p);
                k7.NextKey(k8, p);
                k8.NextKey(k9, p);
                k9.NextKey(null, p);

                return k;
            }
            else
            {
                var k = new MovementKeys();
                var k7 = new EndGame();
                var k10 = new UnknowKey();
                k.NextKey(k7, p);
                k7.NextKey(k10, p);
                k10.NextKey(null, p);
                return (k);
            }
            
        }
        public AttackN BuildAttack(Player p)
        {
            var k = new AttackN();
            var k1 = new AttackS();
            var k2= new AttackM();
            var k3 = new UnknowAttack();
            k.NextKey(k1, p);
            k1.NextKey(k2, p);
            k2.NextKey(k3, p);
            k3.NextKey(null, p);
            return (k);

        }
    }
}
