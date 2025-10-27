// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Channels;
// using System.Threading.Tasks;

// namespace RPG
// {
//     public class Game_Starter
//     {
//         public int PlayerNum = 0; // max9 
//         public void Start_Game()
//         {
//             Console.Clear();
//             Console.WriteLine("Welcome to the game");
//             Console.WriteLine("Please choose do you want to  \n A (H)ost or (J)Join");
//             bool _Host = false;
//             while(true){var key  = Console.ReadKey(false).Key;
//             if(key == ConsoleKey.H){
//                 _Host = true;
//                 Console.Clear();
//                 Console.WriteLine("Starting Game as the Host");
//                 break;
//             }
//             else if(key == ConsoleKey.J){
//                 Console.Clear();
//                 Console.WriteLine("Joining Game");
//                 break;
//             }
//             else {
//                 Console.WriteLine("INcorect input");
//                 Console.Clear();
//                 Console.WriteLine("Please choose do you want to  \n A (H)ost or (J)Join");
//                 continue;
//             }
//             }
//             Controller game = new Controller(_Host, null);
//             game.Game_Loop();

//         }
//     }
// }
