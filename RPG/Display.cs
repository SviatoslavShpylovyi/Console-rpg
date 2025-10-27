using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class Display_info
    {
        private (int x, int y) position;
        private static Display_info _instance;
        private Display_info((int x, int y) position)
        {
            this.position = position;
        }
        public static Display_info GetInstance((int x, int y) position)
        {
            if (_instance == null)
            {
                _instance = new Display_info(position);
            }

            return _instance;
        }

        public void DISPlay(Dungeon dung, Player player)
        {
            for (int i = 0; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(position.x, 1 + i);
                Console.Write(new string(' ', Console.WindowWidth - position.x));


            }
            Console.SetCursorPosition(position.x, position.y - 1);
            Console.WriteLine($"Coin: {player.Coin}      Gold: {player.Gold}");
            Console.SetCursorPosition(position.x, position.y);
            Console.WriteLine($"Strength: {player.Strength}");
            Console.SetCursorPosition(position.x, position.y + 1);
            Console.WriteLine($"Dexterity: {player.Dexterity}, Wisdom : {player.Wisdom}");
            Console.SetCursorPosition(position.x, position.y + 2);
            Console.WriteLine($"Health: {player.Health}");
            Console.SetCursorPosition(position.x, position.y + 3);
            Console.WriteLine($"Luck: {player.Luck}");
            Console.SetCursorPosition(position.x, position.y + 4);
            Console.WriteLine($"Aggresion: {player.Aggression}");
            Console.SetCursorPosition(position.x, position.y + 5);
            Console.WriteLine($"Right hand: {(player.hands[0] != null ? player.hands[0].Name : "None")}");
            Console.SetCursorPosition(position.x, position.y + 6);
            Console.WriteLine($"Left hand: {(player.hands[1] != null ? player.hands[1].Name : "None")}");
            int max_path = 0;
            foreach (var s in dung.Symbols.Keys)
            {
                if (dung.Symbols[s] == 'E')
                {
                    max_path = Math.Max(max_path, player.Path(s));
                }

            }
            Console.SetCursorPosition(position.x, position.y + 7);
            Console.WriteLine($"Closest Opponent {max_path}");
            Console.SetCursorPosition(60, 2);
            DisplayInventory(player);
            DisplayPotions(player);
        }
        public void DisplayPotions(Player player)
        {
            //Console.SetCursorPosition(position.x + 20, position.y + 8);
            //for (int i = 0; i <= player.potion_bag.Count(); i++)
            //{
            //    Console.SetCursorPosition(position.x, position.y + 8 + i);
            //    Console.Write(new string(' ', Console.WindowWidth - position.x));
            //}
            Console.SetCursorPosition(position.x + 20, position.y + 8);
            Console.WriteLine("Potion bag:");
            for (int i = 0; i < player.potion_bag.Count(); i++)
            {
                Console.SetCursorPosition(position.x + 20, position.y + 9 + i);
                Console.WriteLine(player.potion_bag[i].Name);
            }
            Console.SetCursorPosition(position.x + 40, position.y + 8);
            Console.WriteLine("Current Effects:");
            int counter = 0;
            foreach (var pot in player.active_potions.Values)
            {
                Console.SetCursorPosition(position.x + 40, position.y + 9 + counter);
                Console.WriteLine(pot.Name);
                counter++;
            }
        }
        //public void ClearRender()
        //{
        //    Console.SetCursorPosition(0, 21);
        //    Console.Write(new string(' ', Console.WindowWidth));
        //    for (int i = 0; i <= Console.; i++)
        //    {
        //        Console.WriteLine(new string(' ', Console.WindowWidth));
        //    };
        //}
        public void RenderPotions(Player player, int selectedIndex)
        {
            Console.SetCursorPosition(0, 21);
            Console.Write(new string(' ', Console.WindowWidth));
            for (int i = 0; i < player.potion_bag.Count + 1; i++)
            {
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }
            ;
            if (player.potion_bag.Count == 0)
            {
                return;
            }

            Console.SetCursorPosition(0, 22);
            Console.WriteLine("Potion Bag:");
            for (int i = 0; i < player.potion_bag.Count; i++)
            {
                if (i == selectedIndex)
                    Console.Write(" > ");
                else
                    Console.Write("   ");

                Console.WriteLine(player.potion_bag[i].Name);
            }
            Console.SetCursorPosition(0, 21);
            Console.WriteLine("Up/Down arrows - navigate. Enter to drink.Q -drop item");
        }
        private void DisplayInventory(Player player)
        {
            Console.SetCursorPosition(position.x, position.y + 8);
            for (int i = 0; i <= player.GetList().Count; i++)
            {
                Console.SetCursorPosition(position.x, position.y + 8 + i);
                Console.Write(new string(' ', Console.WindowWidth - position.x));
            }
            Console.SetCursorPosition(position.x, position.y + 8);
            Console.WriteLine("Inventory:");
            for (int i = 0; i < player.GetList().Count; i++)
            {
                Console.SetCursorPosition(position.x, position.y + 9 + i);
                Console.WriteLine(player.GetList()[i].Name);
            }
        }


        public void DisplayDescription(Iitem item)
        {
            Console.SetCursorPosition(0, 25);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 25);
            /*
             
            {
                if (item is IWeapon weapon)
                    Console.Write(weapon.Damage);
            } 
             
             */




            Console.WriteLine($"{item.Name}:  {item.Description} ");
        }
        public void OnPlayerMove(int oldX, int oldY, Dictionary<(int, int), char> Symbols, Dungeon dung, Player player)
        {
            Console.SetCursorPosition(0, 25);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(oldX, oldY);
            if (Symbols.ContainsKey((oldX, oldY)))
                Console.Write(Symbols[(oldX, oldY)]);
            else
                Console.Write(' ');
            Console.SetCursorPosition(player.GetX(), player.GetY());
            Console.Write(player.GetModel());
            //Console.SetCursorPosition(32, 3);
            //Console.Write($"{oldX},{oldY}");
            DISPlay(dung, player);
        }
        // public void RenderInventory(Player int selectedIndex)
        // {
        //     Console.SetCursorPosition(0, 21);
        //     Console.Write(new string(' ', Console.WindowWidth));
        //     for (int i = 0; i < player.inventory.Count + 1; i++)
        //     {
        //         Console.WriteLine(new string(' ', Console.WindowWidth));
        //     };
        //     Console.SetCursorPosition(0, 22);
        //     Console.WriteLine("Inventory:");
        //     for (int i = 0; i < player.inventory.Count; i++)
        //     {
        //         if (i == selectedIndex)
        //             Console.Write(" > ");
        //         else
        //             Console.Write("   ");

        //         Console.WriteLine(player.inventory[i].Name);
        //     }
        //     Console.SetCursorPosition(0, 21);
        //     Console.WriteLine("Up/Down arrows - navigate. Enter - equip. R - right hand/ any other key left hand. Q -drop item");
        // }
        // public void DisplayMovement(string str) {
        //     Console.SetCursorPosition(position.x, position.y - 2);
        //     Console.Write(new string(' ', Console.WindowWidth - position.x)); // clear the line
        //     Console.SetCursorPosition(position.x, position.y - 2);
        //     Console.Write(str);

        // }
        public void EndGame()
        {
            Console.Clear();
            Console.SetCursorPosition(10, 10);
            Console.WriteLine("Oh no you lost");
        }

        public void Display_world(Dungeon dung)
        {
            //Console.ForegroundColor = ConsoleColor.Green; // ok so todays plan is to refactor Iitem in order to get read of is operators 
            //Console.BackgroundColor = ConsoleColor.Green;
            for (int i = 0; i < dung.height; i++)
            {
                for (int j = 0; j < dung.width; j++)
                {
                    if (dung.Symbols.ContainsKey((j, i)))
                        Console.Write(dung.Symbols[(j, i)]);
                    else
                        Console.Write(dung.dungeon[i, j]);
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }
        public void DisplayInstruction(Dungeon dung)
        {
            InstructionMaker ins = new InstructionMaker();
            string instruction = ins.GetInstruction(dung);
            ClearInstruction();
            string[] lines = instruction.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
            int y = 21;
            foreach (var line in lines)
            {
                Console.SetCursorPosition(0, y);
                Console.Write(line);
                y++;
            }
        }
        public void ClearInstruction()
        {
            for (int i = 0; i < Console.WindowHeight - 21; i++)
            {
                Console.SetCursorPosition(0, 21 + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }
        public void EnterBattle(Enemy e, Player player)
        {
            ClearInstruction();
            Console.SetCursorPosition(0, 22);
            Console.WriteLine("======================================BatlePhase==============");
            Console.SetCursorPosition(0, 23);
            Console.WriteLine("Chose the attack");
            Console.SetCursorPosition(0, 24);
            Console.WriteLine("1. Normal Attack ");
            Console.SetCursorPosition(0, 25);
            Console.WriteLine("2. Stealth Attack");
            Console.SetCursorPosition(0, 26);
            Console.WriteLine("3. Magic Attack");
            Console.SetCursorPosition(20, 23);
            Console.WriteLine($"Player health is {player.Health}");
            Console.SetCursorPosition(20, 24);
            Console.WriteLine($"Enemy health is {e.Health}");
            Console.SetCursorPosition(30, 25);
            Console.WriteLine($"Enemy : {e.Name} {e.Armour}");


            //Console.WriteLine($"The damage is {(player.hands[0]!= null? ((Weapon)(player.hands[1])).Damage : 0) + (player.hands[1] != null ? ((Weapon)(player.hands[1])).Damage : 0)}");

        }
        public void displayNums(int damage, int defence)
        {
            Console.SetCursorPosition(30, 27);
            Console.WriteLine($"{damage},  {defence}");
        }
        public void StartGame()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            string[] mini = new string[]
       {
            "##     ## #### ##    ## #### ",
            "###   ###  ##  ###   ##  ##  ",
            "#### ####  ##  ####  ##  ##  ",
            "## ### ##  ##  ## ## ##  ##  ",
            "##     ##  ##  ##  ####  ##  ",
            "##     ##  ##  ##   ###  ##  ",
            "##     ## #### ##    ## #### ",
       };

            string[] rpg = new string[]
            {
            "######  ######   #####   ",
            "#   ## ##   ## ##   ##  ",
            "#   ## ##   ## ##       ",
            "#####  ######  ##  ###  ",
            "#   ## ##      ##   ##  ",
            "#   ## ##      ##   ##  ",
            "#   ## ##       #####   "
            };

            for (int i = 0; i < mini.Length; i++)
            {
                Console.WriteLine(mini[i] + "    " + rpg[i]);
            }
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Start as (S)erver or (C)lient?");
        }

        public void DisplayMazeDTO(WorldDTo world)
        {
             Console.Clear();
            for (int y = 0; y < world.DungeonGrid.Count; y++)
            {
                string row = world.DungeonGrid[y];
                for (int x = 0; x < row.Length; x++)
                {
                    bool playerPrinted = false;
                    foreach (var play in world.Players)
                    {
                        if (play.X == x && play.Y == y)
                        {
                            Console.Write(play.model);
                            playerPrinted = true;
                            break;
                        }
                    }

                    if (!playerPrinted)
                    {
                        string key = $"({x},{y})";
                        if (world.Symbols != null && world.Symbols.ContainsKey(key))
                            Console.Write(world.Symbols[key]);
                        else
                            Console.Write(row[x]);
                    }
                }
                Console.WriteLine();
            }





        }

        public void DisplayDTO(WorldDTo world, PlayerDTO player)
        {
            if (player.HAS_LEFT == 't')
            {
                Console.Clear();
                Console.WriteLine("GAME OVER");
            }




            for (int i = 0; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(position.x, 1 + i);
                Console.Write(new string(' ', Console.WindowWidth - position.x));


            }
            Console.SetCursorPosition(position.x, position.y - 1);
            Console.WriteLine($"Coin: {player.Coin}      Gold: {player.Gold}");
            Console.SetCursorPosition(position.x, position.y);
            Console.WriteLine($"Strength: {player.Strength}");
            Console.SetCursorPosition(position.x, position.y + 1);
            Console.WriteLine($"Dexterity: {player.Dexterity}, Wisdom : {player.Wisdom}");
            Console.SetCursorPosition(position.x, position.y + 2);
            Console.WriteLine($"Health: {player.Health}");
            Console.SetCursorPosition(position.x, position.y + 3);
            Console.WriteLine($"Luck: {player.Luck}");
            Console.SetCursorPosition(position.x, position.y + 4);
            Console.WriteLine($"Aggresion: {player.Aggression}");
            Console.SetCursorPosition(position.x, position.y + 5);
            Console.WriteLine($"Right hand: {player.Hands[0]}");
            Console.SetCursorPosition(position.x, position.y + 6);
            Console.WriteLine($"Left hand: {player.Hands[1]}");
            int max_path = 0;
            foreach (var s in world.Symbols.Keys)
            {
                if (world.Symbols[s] == 'E')
                {
                    var trimmed = s.Trim('(', ')').Split(',');
                    int x = int.Parse(trimmed[0]);
                    int y = int.Parse(trimmed[1]);
                    int dist = Math.Abs(player.X - x) + Math.Abs(player.Y - y);
                    max_path = Math.Max(max_path, dist);
                }

            }
            Console.SetCursorPosition(position.x, position.y + 7);
            Console.WriteLine($"Closest Opponent {max_path}");
            Console.SetCursorPosition(60, 2);
            DisplayInventoryDTO(player);
            DisplayPotionsDTO(player);
            if (player.IsAtFight == 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(0, 22);
                Console.WriteLine("============== BATTLE PHASE ==============");
                Console.ResetColor();
                Console.SetCursorPosition(0, 23);
                Console.WriteLine("Choose your attack:");
                Console.WriteLine("1. Normal Attack   2. Stealth Attack   3. Magic Attack");
                Console.SetCursorPosition(0, 26);
                Console.WriteLine("(Press the corresponding number key)");
            }
            else
            {
                for (int i = 20; i < Console.WindowHeight - 1; i++)
                {
                    Console.SetCursorPosition(position.x, 1 + i);
                    Console.Write(new string(' ', Console.WindowWidth - position.x));


                }
            }
        }
        public void DisplayInventoryDTO(PlayerDTO player)
        {
            Console.SetCursorPosition(position.x + 2, position.y + 8);
            Console.WriteLine("Inventory:");
            for (int i = 0; i < player.Inventory.Count; i++)
            {
                Console.SetCursorPosition(position.x + 2, position.y + 9 + i);
                Console.WriteLine(player.Inventory[i]);
            }
        }
        public void DisplayPotionsDTO(PlayerDTO player)
        {
            Console.SetCursorPosition(position.x + 20, position.y + 8);
            Console.WriteLine("Potion bag:");
            for (int i = 0; i < player.Potions.Count(); i++)
            {
                Console.SetCursorPosition(position.x + 20, position.y + 9 + i);
                Console.WriteLine(player.Potions[i]);
            }

            Console.SetCursorPosition(position.x + 40, position.y + 8);
            Console.WriteLine("Current Effects:");
            for (int i = 0; i < player.ActivePotions.Count; i++)
            {
                Console.SetCursorPosition(position.x + 40, position.y + 9 + i);
                Console.WriteLine(player.ActivePotions[i]);
            }
        }



        public void StartingRender(WorldDTo gameState)
        {
            for (int y = 0; y < gameState.DungeonGrid.Count; y++)
             {
                 string row = gameState.DungeonGrid[y];
                 for (int x = 0; x < row.Length; x++)
                 {
                     bool playerPrinted = false;
                     foreach (var player in gameState.Players)
                     {
                         if (player.X == x && player.Y == y)
                         {
                             Console.Write(player.model);
                             playerPrinted = true;
                             break;
                         }
                     }

                     if (!playerPrinted)
                     {
                         string key = $"({x},{y})";
                         if (gameState.Symbols != null && gameState.Symbols.ContainsKey(key))
                             Console.Write(gameState.Symbols[key]);
                         else
                             Console.Write(row[x]);
                     }
                 }
                 Console.WriteLine();
             }
        }



    }

















}
//(player.hands[0] != null ? player.hands[0].Name + player.hands[0].GetType() : "None")
