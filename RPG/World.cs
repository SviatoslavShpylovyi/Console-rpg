using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace RPG;

public class World  // can be a potential controller and some form of the viewer can just be an some form of the add player to the world as it would triger the 
                    //event in the Player to move and that is it 
                    // but how do I handle the concurrent inputs ? 
{
    public Dungeon dung;
    private int height, width;
    public static World? Instance;
    public Dictionary<char, Player> Players;
    private Dictionary< Player,BattleSimulation> Battles = new Dictionary<Player, BattleSimulation>();
    private readonly object Lock = new object();
    private World(int width, int height, bool flag) // sends every display to the player
                                                    // player can be a host and just a blank server 

    {
        dung = new DungeonBuilder(width, height).NormalGeneration();
        Players = new Dictionary<char, Player>();
    }
    public Dictionary<(int, int), char  >GetSymbols()
    {
        return dung.Symbols;   
    }
    public static World Get_Instance(int width, int height)
    {
        if (Instance == null)
        {
            Instance = new World(width, height, true);
        }
        return Instance;
    }
    public bool AddPlayer(char name)
    {
        lock (Lock)
        {
            if (Players.Count >= 9)
            {
                Console.WriteLine("Too many players on server");
                return false;
            }
            else if (Players.ContainsKey(name))
            {
                return false;
            }
            else
            {

                Player p = new Player(name);
                BindPlayer(p);
                Players.Add(name, p);
                return true;
            }
        }
    }
    public bool Drop((int, int) position, Iitem item)
    {
        if (dung.Items_in_world.ContainsKey(position))
        {
            return false;
        }
        dung.Items_in_world[position] = item;
        dung.Symbols[position] = 'd';
        Console.SetCursorPosition(position.Item1, position.Item2);
        Console.Write(dung.Symbols[position]);
        return true;
    }

    public void BindPlayer(Player p)
    {
        p.OnDropItem = Drop;
        dung.dungeon[p.GetY(), p.GetX()] = p.GetModel();

        return;
    }
    public int Start_Game(Player p, string Action)
    {
        lock (Lock)
        {
            foreach (Enemy enemy in dung.Enemies_in_world.Values.ToList())
            {
                if (!enemy.AtBatlle)
                {
                    enemy.DoSomething(this);
                }
            }
            if (p.HAS_LEFT)
            {
                return -1;
            }
            if (Battles.ContainsKey(p))
            {
                int result = Battles[p].RealFight(Action);
                if (result == -1)
                {
                    return -1; // Player died
                }
                else if (result == 1)
                {
                    Battles.Remove(p);
                    dung.Enemies_in_world.Remove((p.GetX(), p.GetY()));
                    dung.Symbols.Remove((p.GetX(), p.GetY()));
                    p.IsAtFight = 0; // important to reset!
                }
                return 1;
            }

            if (p.Move_MVC(dung, Action) == 1)
            {
                foreach (var c in dung.Symbols.Keys)
                {
                    if (dung.Symbols[c] == 'E')
                    {
                        if (p.Path(c) == 0)
                        {
                            dung.Enemies_in_world.TryGetValue(c, out IEnemy en);
                            Battles.Add(p, new BattleSimulation(p, (Enemy)en));
                            p.IsAtFight = 1;
                            ((Enemy)en).AtBatlle = true;
                        }
                    }
                }
            }
        }
        return 1;
    }
    public bool CheckCords(int x, int y)
    {
        bool ret_flag = false;
        if (x >= 0 && x < dung.width && y >= 0 && y < dung.height)
        {
            if (!dung.Enemies_in_world.ContainsKey((x, y)))
            {
                if (!dung.Items_in_world.ContainsKey((x, y)))
                {

                    ret_flag = true;
                }
            }
        }
        return ret_flag;
    }
    public int GetPlayers()
    {
        return Players.Count;
    }
}