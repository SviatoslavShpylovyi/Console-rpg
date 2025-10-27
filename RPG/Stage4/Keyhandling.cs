using RPG;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public abstract class KEY_Handling
    {
        // Handler Handler = new Handler
        protected KEY_Handling _handler;
        // Display_info display ;
        protected Player player;
        public void NextKey(KEY_Handling handler, Player p)
        {
            _handler = handler;
            player = p;
        }
        public abstract (int, int) KeyPressed(string Action, Player p);
    }
    public class MovementKeys : KEY_Handling
    {
        public override (int, int) KeyPressed(string Action, Player p)
        {
            int retx = p.GetX();
            int rety = p.GetY();
            if (Action == "Up")
            {
                rety = Math.Max(1, p.GetY() - 1);
            }
            else if (Action == "Down")
            {
                rety = Math.Min(18, p.GetY() + 1);
            }
            else if (Action == "Left")
            {
                retx = Math.Max(1, p.GetX() - 1);
            }
            else if (Action == "Right")
            {
                retx = Math.Min(38, p.GetX() + 1);
            }
            else
            {
                return _handler.KeyPressed(Action, p);
            }
            return (retx, rety);
            //case ConsoleKey.I:
            //    info.ShowInventory();
            //    break;
            //case ConsoleKey.E:
            //    Pickup(ItemsLocation, Symbols);
            //    brea
        }


    }
    public class PickUpKey : KEY_Handling
    {
        public override (int, int) KeyPressed(string Action, Player p)
        {

            if (Action == "Pickup") {
                return (p.GetX(), -p.GetY()); // the minus coordinate will be like a flag for the Player to understand              
            }
            else
            {

                return _handler.KeyPressed(Action, p);
            }
        }
    }
    // public class ClearInstructionKey : KEY_Handling
    // {
    //     public override (int, int) KeyPressed(ConsoleKey k, Player p)
    //     {
    //         if (k == KeyBinds.GetKey("Clear"))
    //         {
    //             display.DisplayMovement("Clear");
    //             display.ClearInstruction();
    //             return (p.GetX(), p.GetY());

    //         }
    //         else
    //         {
    //             return _handler.KeyPressed(k, p);
    //         }
    //     }
    // }
    // public class InstructionKey : KEY_Handling
    // {
    //     public override (int, int) KeyPressed(ConsoleKey k, Player p)
    //     {
    //         if(k == KeyBinds.GetKey("Help")) {
    //             display.DisplayMovement("Help");
    //         return(-p.GetX(),p.GetY());

    //         }
    //         else
    //         {
    //             return _handler.KeyPressed(k, p);
    //         }
    //     }
    // }
    public class UnknowKey : KEY_Handling
    {
        public override (int, int) KeyPressed(string Action, Player p)
        {
            return (p.GetX(), p.GetY());
        }
    }
    public class UnknowAttack : KEY_Handling
    {
        public override (int, int) KeyPressed(string Action, Player p)
        {
            return (0, 0);
        }
    }
        public class EndGame : KEY_Handling
    {
        public override (int, int) KeyPressed(string Action, Player p)
        {
            if (Action == "Exit")
            {
                p.HAS_LEFT = true;
                return (p.GetX(), p.GetY());
            }
            else
            {
                return _handler.KeyPressed(Action, p);
            }
        }
    }
    public class AttackN : KEY_Handling
    {
        public override (int, int) KeyPressed(string Action, Player p)
        {
            if (Action == "NormalAttack")
            {
                int dmg = 0;
                int dfs = 0;
                NormalAttack att = new NormalAttack(p);
                dmg = (p.hands[0] != null ? ((Weapon)p.hands[0]).Accept(att) : 0) + (p.hands[1] != null ? ((Weapon)p.hands[1]).Accept(att) : 0);
                dfs = (p.hands[0] != null ? ((Weapon)p.hands[0]).Defence(att) : 0) + (p.hands[1] != null ? ((Weapon)p.hands[1]).Defence(att) : 0);
                // dfs += ((Weapon)p.hands[1]).Defence(att);
                //dmg += ((Weapon)p.hands[1]).Accept(att);
                //p.Step();
                return (dmg, dfs);

            }
            else return _handler.KeyPressed(Action, p);

        }
    }
    public class AttackS : KEY_Handling
    {
        public override (int, int) KeyPressed(string Action, Player p)
        {
            if (Action == "StealthAttack")
            {
                int dmg = 0;
                int dfs = 0;
                // new Battle(p);
                StealthAttac att = new StealthAttac(p);
                dmg = (p.hands[0] != null ? ((Weapon)p.hands[0]).Accept(att) : 0) + (p.hands[1] != null ? ((Weapon)p.hands[1]).Accept(att) : 0);
                dfs = (p.hands[0] != null ? ((Weapon)p.hands[0]).Defence(att) : 0) + (p.hands[1] != null ? ((Weapon)p.hands[1]).Defence(att) : 0);
                // dfs += ((Weapon)p.hands[1]).Defence(att);
                //  dmg += ((Weapon)p.hands[1]).Accept(att);
                return (dmg, dfs);

            }
            else return _handler.KeyPressed(Action, p);

        }
    }
    public class AttackM : KEY_Handling
    {
        public override (int, int) KeyPressed(string Action, Player p)
        {
            if (Action == "Magicttack")
            {
                int dmg = 0;
                int dfs = 0;
                MagicAttack att = new MagicAttack(p);
                dmg = (p.hands[0] != null ? ((Weapon)p.hands[0]).Accept(att) : 0) + (p.hands[1] != null ? ((Weapon)p.hands[1]).Accept(att) : 0);
                dfs = (p.hands[0] != null ? ((Weapon)p.hands[0]).Defence(att) : 0) + (p.hands[1] != null ? ((Weapon)p.hands[1]).Defence(att) : 0);
                // dfs += ((Weapon)p.hands[1]).Defence(att);
                // dmg += ((Weapon)p.hands[1]).Accept(att);
                return (dmg, dfs);

            }
            else return _handler.KeyPressed(Action, p);

        }
    }







}
public class InvetoryEquip : KEY_Handling {
    public override (int, int) KeyPressed(string Action, Player p)
    {
        if (Action.Length == 3 && Action[0] == 'I')
        {
            int index = Action[1] - '0';
            char act = Action[2];
            switch (act)
            {
                case 'L':
                    p.Equip_to_hands(0, index);
                    break;
                case 'R':
                    p.Equip_to_hands(1, index);
                    break;
            }
            return (p.GetX(), p.GetY());
        }
        else
        {
               return _handler.KeyPressed(Action, p);
        }
    }
}
    public class InventoryDropItem : KEY_Handling
    {
    public override (int, int) KeyPressed(string Action, Player p)
    {
            if (Action.Length == 3 && Action[0] == 'I')
        {
            int index = Action[1] - '0';
            char act = Action[2];
            if (act == 'D')
            {
                p.DropItem(index);
            }
            return (p.GetX(), p.GetY());
        }
        else
        {
               return _handler.KeyPressed(Action, p);
        }
            
        }
    }
    public class DropHandItemKey : KEY_Handling
    {
    public override (int, int) KeyPressed(string Action, Player p)
    {
        if (Action.Length == 3 && Action[0] == 'H' && (Action[1]-'0')<3)
        {
            int index = Action[1] - '0';
            char act = Action[2];
            if (act == 'D')
            {
                var item = p.hands[index];
                if (item != null)
                {
                    var pos = (p.GetX(), p.GetY());
                    if (p.OnDropItem.Invoke(pos, item))
                    {
                        p.hands[index] = null;
                    }
                }
            }
            return (p.GetX(), p.GetY());
        }
        else
        {
               return _handler.KeyPressed(Action, p);
        }
            
            
    }
    }
    public class DropAllItems : KEY_Handling
    {
        public override (int, int) KeyPressed(string Action, Player p)
        {
            if (Action == "DropAll")
            {
               p.DropAll();
                return (p.GetX(), p.GetY());
            }

            return _handler.KeyPressed(Action, p);
        }
    }
public class PotionDrop : KEY_Handling
{
    public override (int, int) KeyPressed(string Action, Player p)
    {

        if (Action.Length == 3 && Action[0] == 'P')
        {
            int index = Action[1] - '0';
            char act = Action[2];
            if (act == 'D')
            {
                p.DropPotion(index);
            }
            return (p.GetX(), p.GetY());
        }
        else
        {
            return _handler.KeyPressed(Action, p);
        }

    }

}
public class PotionDrink : KEY_Handling
{
    public override (int, int) KeyPressed(string Action, Player p)
    {
         if (Action.Length == 3 && Action[0] == 'P')
        {
            int index = Action[1] - '0';
            char act = Action[2];
            if (act == 'U')
            {
                p.DrinkPotion(index);
            }
            return (p.GetX(), p.GetY());
        }
        else
        {
            return _handler.KeyPressed(Action, p);
        }
    }
}   

//Jason for the name and command 
// is_HAndable true/false  Done
// Proper Invetory handling Done
// Eternal effect Done
//moving in four directions (keys: `W`, `S`, `A`, `D`), Done
//- picking up an item (`E`), Done
//- dropping a selected item from the inventory or a hand, Done
//- dropping all items, ? -- розкидувати по світу 
//- drinking a selected potion from the inventory,  Done 
//- placing an item in a hand or hiding it in the inventory, Weird
//- exiting the game. Done


// so plan for todaye make a nice start of the game 
// Create another class for key handling or just make another one key handle
// start two thereaded start at becoming a server 
// refactor key logic of the player 