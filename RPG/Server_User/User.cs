using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace RPG
{
    public class User
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private Display_info display;
        private Controller controller;
        private int playerID;
        public User(string ip, int port)
        {
            display = Display_info.GetInstance((43, 2));
            this.client = new TcpClient();
            client.Connect(ip, port);
            var stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            controller = new Controller(null);
        }
        public void Start()
        {
            string InitState = reader.ReadLine();
            HandleFirstJson(InitState);
            new Thread(Receive).Start();
            Input();
        }
        private void Receive()
        {
            while (true)
            {
                try
                {
                    string json = reader.ReadLine();
                    if (json == null) break;
                    HandleJson(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Receive error: {ex.Message}");
                    break;
                }
            }
        }
        private void Input()
        {
            while (true)
            {
                try
                {
                    ActionDTO input = controller.CreateAction();
                    string json = JsonSerializer.Serialize(input);
                    writer.WriteLine(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private void HandleFirstJson(string InitState)
        {
            FirstMessage message = JsonSerializer.Deserialize<FirstMessage>(InitState);
            playerID = message.playerID;
            display.DisplayMazeDTO(message.world);
        }
        private void HandleJson(string InitState)
        {
            WorldDTo world = JsonSerializer.Deserialize<WorldDTo>(InitState);
            PlayerDTO mainPlayer = null;
            foreach (PlayerDTO p in world.Players)
            {
                if (p.model == (char)('0' + playerID))
                {
                mainPlayer = p;
                    
                }
            }
            display.DisplayMazeDTO(world);
            display.DisplayDTO(world, mainPlayer);
        }


    }
}
