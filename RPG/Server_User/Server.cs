using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RPG
{
    public class Server
    {
        private TcpListener tcpListener;
        private List<ClientHandler> clients = new List<ClientHandler>();
        private Controller controller;
        private World world;
        private const int MAX_players = 9;
        private int next_player = 1;
        public Server(int port)
        {
            world = World.Get_Instance(40,20);
            controller = new Controller(world); // will be created always with the view 
            tcpListener = new TcpListener(IPAddress.Any, port);
        }
        public void Start()
        {
            tcpListener.Start();
            while(clients.Count < MAX_players) 
            {
            TcpClient client = tcpListener.AcceptTcpClient();
            int playerID = next_player ++;
            ClientHandler handler = new ClientHandler(client, playerID, this);
                world.AddPlayer((char)('0' + playerID));
            clients.Add(handler);
            string to_send = Create_First_State(playerID);
            handler.Send(to_send);
            new Thread(handler.Start).Start(); // handling the data from the user 
            }

        }
        public bool HandleAction(string action, int playerID)
        {
            try
            {
                bool ret_flag = true;
                ActionDTO act = JsonSerializer.Deserialize<ActionDTO>(action);
                if ((controller.Handle(act, playerID)) == false)
                {
                    ret_flag = false;
                }

                string state = Create_State();
                SendAll(state);
                return ret_flag;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return true;
            }
        }
        private string  Create_State()
        {
            WorldDTo dt = new WorldDTo(world);
            return JsonSerializer.Serialize(dt);
        }
        private string Create_First_State(int playerID)
        {
            WorldDTo dt = new WorldDTo(world);
            FirstMessage m = new FirstMessage(dt, playerID);
            return JsonSerializer.Serialize(m);
            
        }
        private void SendAll(string state)
        {
            foreach (var client in clients)
            {
                client.Send(state);
            }
        }

    }
    public class ClientHandler
    {
        private TcpClient tcpClient;
        private StreamReader reader;
        private StreamWriter writer;
        private int playerID;
        private Server server;
        public ClientHandler(TcpClient tcpClient, int playerID, Server serv)
        {
            this.tcpClient = tcpClient;
            this.playerID = playerID;
            this.server = serv;
            var stream = tcpClient.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };
        }
        public void Start()
        {
            try
            {
                while (true)
                {
                    string json = reader.ReadLine();
                        if(json != null)
                    {
                        Console.WriteLine("handling");
                        if (!server.HandleAction(json, playerID))
                        {
                            tcpClient.Close();
                            break;
                        }
                    }
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                tcpClient.Close();
            }
        }
        public void Send(string json)
        {
            try
            {
                writer.WriteLine(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
