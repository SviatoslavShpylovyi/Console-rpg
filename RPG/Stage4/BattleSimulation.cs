using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class BattleSimulation
    {
        Player p;
         public Enemy enemy;
        public BattleSimulation(Player p, Enemy enemy)
        {
            this.p = p;
            this.enemy = enemy;
        }
        public void RunSimulation()
        {
            Random  rand = new Random();
            int damage = 0;
            int defence = 0;
            while (p.Health >=0 && enemy.Health>= 0)
            {
               int attack =  rand.Next(1,3);
                switch (attack)
                {
                    case 1:
                         NormalAttack attack1= new NormalAttack(p);
                        damage= ((Weapon)p.hands[0]).Accept(attack1); 
                        defence= ((Weapon)p.hands[0]).Defence(attack1); 
                        Console.WriteLine("Performing Normal attack");
                        break;
                    case 2:
                        StealthAttac attack2 = new StealthAttac(p);
                        damage = ((Weapon)p.hands[0]).Accept(attack2); 
                        defence = ((Weapon)p.hands[0]).Defence(attack2);

                        Console.WriteLine("Performing Stealth attack");
                        break;
                    case 3:
                        MagicAttack attack3 = new MagicAttack(p);
                        damage = ((Weapon)p.hands[0]).Accept(attack3); 
                        defence = ((Weapon)p.hands[0]).Defence(attack3); // test method for text analyze of the pattern 
                        Console.WriteLine("Performing Magic attack");
                        break;
                }
                Console.WriteLine($"Dealing damage to the enemy {damage} , and having {defence}");
                enemy.Health -= damage;
                Thread.Sleep(1000);
                Console.WriteLine($"Enemy {enemy.Name}, {enemy.Health}  performce a hit with the {enemy.Damage}");
                Thread.Sleep(1000);
                p.Health -= (defence - enemy.Damage);
                Console.WriteLine($"Player , {p.Health}");

            }
        }
        public int RealFight(string command)
        {
            // while(p.Health >= 0 && enemy.Health >= 0)
            //{
            KeyBilder ks = new KeyBilder();
            AttackN k  =  ks.BuildAttack(p);
            (int, int) pos = k.KeyPressed(command, p);
                int damage = pos.Item1;
                int defence= pos.Item2;
                p.Defence = defence;
                enemy.Health -= Math.Max(0, damage - enemy.Armour);
                p.Health -= Math.Max(0, enemy.Damage - defence);
                p.Step();
            //}
            if (p.Health <= 0)
            {
                p.HAS_LEFT = true;
                return -1;
            }
            if (enemy.Health <= 0) {
            return 1;
            
            }
            return 0;
        }
    }
}
