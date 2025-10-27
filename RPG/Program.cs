﻿using System.Net.Security;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using RPG;
class Program
{
    static void Main(string[] args)
    {
        int mode = -1;
        string ip = "127.0.0.1";
        int port = 5555;
        if (args.Length == 2)
        {
            switch (args[0])
            {
                case "--server":
                    if (args.Length > 1)
                    {
                        port = int.Parse(args[1]);
                        mode = 0;
                    }

                    break;
                case "--client":
                    mode = 1;
                    if (args.Length > 1)
                    {
                        string[] str = args[1].Split(':');
                        if (str.Length != 2)
                        {
                            mode = -1;
                            break;
                        }
                        ip = str[0];
                        port = int.Parse(str[1]);
                        mode = 1;
                    }

                    break;

            }
        }
        else
        {
            Display_info display = Display_info.GetInstance((43, 2));
            display.StartGame();
                ConsoleKey k = Console.ReadKey(true).Key;
                switch (k)
                {
                    case ConsoleKey.S:
                        mode = 0;
                        break;
                    case ConsoleKey.C:
                        mode = 1;
                        break;

                }

            switch (mode)
            {
                case 0:
                    Server serv = new Server(port);
                    serv.Start();
                    break;
                case 1:
                    User us = new User(ip, port);
                    us.Start();
                    break;
            }
        }
    }
}