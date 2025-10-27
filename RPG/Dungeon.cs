using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static RPG.StrenghtPotion;

namespace RPG
{
    public class Cords
    {
        public int x { get; set; }
        public int y { get; set; }
        internal Cords(int x, int y)
        {
            this.x = x; this.y = y;
        }
    };
    public interface IDungeon
    {
        void EmptyDungeon();
        void FilledDungeon();
        void AddPath();
        void AddChamber(int n = 4, int width = 4, int height = 4);
        void CentralRoom(int new_width, int new_height);
        void AddItem();
        void AddWeapons();
        void AddModifiedWeapons();
        void AddPotions();
        void AddEnemies();
    }
    public class Dungeon : IDungeon
    {
        public Dictionary<(int, int), Iitem> Items_in_world { get; }
        public Dictionary<(int, int), char> Symbols;
        public Dictionary<(int, int), IEnemy> Enemies_in_world;
        public char[,] dungeon;
        public readonly int height, width;
        private Random rand;
        public Dungeon(int width, int height)
        {
            Enemies_in_world = new Dictionary<(int, int), IEnemy>();
            Items_in_world = new Dictionary<(int, int), Iitem>();
            Symbols = new Dictionary<(int, int), char>();
            this.width = width;
            this.height = height;
            dungeon = new char[height, width];
            rand = new Random();
        }
        public char[,] GetResult() => dungeon;

        public void EmptyDungeon()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                    dungeon[i, j] = ' ';
            }
        }

        public void FilledDungeon()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                    dungeon[i, j] = '█';
            }
        }

        public void AddChamber(int n = 4, int width = 4, int height = 4)
        {
            int prevx = 0;
            int prevy = 0;
            for (int i = 0; i < n; i++)
            {
                int x, y;
                x = rand.Next(1, this.width - width);
                y = rand.Next(1, this.height - height);
                //if (IsChamberOverlap(x, y, width, height))
                //{
                // i--;
                // continue;
                //}
                //if(x < prevx)
                for (int j = y; j < y + height; j++)
                {
                    for (int k = x; k < x + width; k++)
                    {
                        dungeon[j, k] = ' ';
                    }

                }
                prevx = x;
                prevy = y;

            }
        }
        //private bool IsChamberOverlap(int x, int y, int width, int height)
        //{
        //    for (int j = y; j < y + height; j++)
        //    {
        //        for (int k = x; k < x + width; k++)
        //        {
        //            if (dungeon[j, k] == ' ')  
        //            {
        //                return true;  
        //            }
        //        }
        //    }
        //    return false; 
        //}
        public void CentralRoom(int room_width, int room_height)
        {
            int startx = (width / 2) - (room_width / 2);
            int starty = (height / 2) - (room_height / 2);
            for (int i = starty; i < starty + room_height; i++)
            {
                for (int j = startx; j < startx + room_width; j++)
                {
                    dungeon[i, j] = ' ';
                }

            }

        }
        private (int, int) RandomizePos()
        {
            int limit_of_iterations = 500;
            int x, y;
            (int, int) pos = (-1, -1);
            for (int i = 0; i < limit_of_iterations; i++)
            {
                x = rand.Next(1, width - 2);
                y = rand.Next(1, height - 2);
                pos = (x, y);
                if (dungeon[y, x] == ' ' && !Symbols.ContainsKey(pos) && (CheckApproximate(x, y)) < 3)
                {
                    //Console.WriteLine($"place for item is found{x},{y}");
                    return pos;
                }
            }
            (int, int) ret = (-1, -1);
            return ret;

        }

        public void AddItem()
        {
            (int, int) pos;
            if ((pos = RandomizePos()) == (-1, -1))
                return;
            switch (rand.Next(3))
            {
                case 0: Items_in_world.Add(pos, new Vase()); Symbols.Add(pos, 'v'); break;
                case 1: Items_in_world.Add(pos, new Bone()); Symbols.Add(pos, 'b'); break;
                case 2: Items_in_world.Add(pos, new RustyCoin()); Symbols.Add(pos, 'c'); break;



            }
        }

        public void AddWeapons()
        {
            (int, int) pos;
            if ((pos = RandomizePos()) == (-1, -1))
                return;
            switch (rand.Next(3))
            {
                case 0: Items_in_world.Add(pos, new Sword()); Symbols.Add(pos, 's'); break;
                case 1: Items_in_world.Add(pos, new Axe()); Symbols.Add(pos, 'a'); break;
                case 2: Items_in_world.Add(pos, new GreateSword()); Symbols.Add(pos, 'S'); break;
            }
        }

        public void AddModifiedWeapons()
        {

            (int, int) pos;
            if ((pos = RandomizePos()) == (-1, -1))
                return;

            switch (rand.Next(2))
            {
                case 0: Items_in_world.Add(pos, new Lucky(new Sword())); Symbols.Add(pos, 's'); break;
                case 1: Items_in_world.Add(pos, new PowerfulEffect(new Sword())); Symbols.Add(pos, 's'); break;

            }


        }

        public void AddPotions()
        {
            (int, int) pos;
            if ((pos = RandomizePos()) == (-1, -1))
            { return; }
            switch (rand.Next(3))
            {
                case 0: Items_in_world.Add(pos, new HealthPotion(5)); Symbols.Add(pos, 'H'); break;
                case 1: Items_in_world.Add(pos, new StrenghtPotion(5)); Symbols.Add(pos, 'S'); break;
                case 2: Items_in_world.Add(pos, new WisdomPotion(5)); Symbols.Add(pos, 'W'); break;
            }
        }

        public void AddEnemies()
        {
            (int, int) pos;
            if ((pos = RandomizePos()) == (-1, -1))
            { return; }
            switch (rand.Next(3))
            {
                case 2: Enemies_in_world.Add(pos, new OGR(pos.Item1, pos.Item2)); Symbols.Add(pos, 'E'); break;
                case 0: Enemies_in_world.Add(pos, new Skeleton(pos.Item1, pos.Item2)); Symbols.Add(pos, 'E'); break;
                case 1: Enemies_in_world.Add(pos, new ChillGoblin(pos.Item1, pos.Item2)); Symbols.Add(pos, 'E'); break;
            }
        }

        public void helper(int startx, int starty, int bound_x, int bound_y)
        {
            int path_num = rand.Next(1, 5);
            dungeon[0, 0] = ' ';
            dungeon[1, 0] = ' ';
            dungeon[1, 1] = ' ';

            int rand_finish_posx = bound_x;
            int rand_finish_posy = bound_y;
            int currx = startx;
            int curry = starty;

            Stack<(int, int)> pathStack = new Stack<(int, int)>();

            for (int i = 0; i < 1; i++)
            {
                while (currx != rand_finish_posx || curry != rand_finish_posy)
                {
                    List<int> dirs = new List<int> { 0, 1, 2, 3 };


                    if (currx + 1 > width - 2 || dungeon[curry, currx + 1] == ' ')
                    {
                        dirs.Remove(0);
                    }
                    if (currx - 1 < 1 || dungeon[curry, currx - 1] == ' ')
                    {
                        dirs.Remove(1);
                    }
                    if (curry + 1 > height - 2 || dungeon[curry + 1, currx] == ' ')
                    {
                        dirs.Remove(2);
                    }
                    if (curry - 1 < 1 || dungeon[curry - 1, currx] == ' ')
                    {
                        dirs.Remove(3);
                    }

                    if (dirs.Count == 0)
                    {
                        if (pathStack.Count > 0)
                        {
                            if (CheckApproximate(curry, currx) <= 3)
                                dungeon[curry, currx] = '█';
                            (currx, curry) = pathStack.Pop();
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }

                    int direction = dirs[rand.Next(dirs.Count)];

                    switch (direction)
                    {
                        case 0:
                            currx += 1;
                            break;
                        case 1:
                            currx -= 1;
                            break;
                        case 2:
                            curry += 1;
                            break;
                        case 3:
                            curry -= 1;
                            break;
                    }
                    dungeon[curry, currx] = ' ';
                    pathStack.Push((currx, curry));
                }
            }
        }

        public Dictionary<(int, int), Iitem> GetItems()
        {
            return Items_in_world;
        }
        public Dictionary<(int, int), char> GetSymbols()
        {
            return Symbols;
        }
        public char[,] GetGrid()
        {
            return dungeon;
        }
        public void AddPath()
        {
            List<(int, int)> centers = CheckForRooms();
            if (centers.Count() > 0)
            {
                foreach (var center in centers)
                {
                    CreatePath(center.Item1, center.Item2);
                }
                //return;
            }
            helper(1, 1, width - 1, height - 1);
            fixMaze();
            // locate room centers 
            //draw a random path from 1,1 to 38,18
        }
        private void fixMaze()
        {
            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    if (dungeon[i, j] == '█')
                    {
                        if (CheckApproximate(i, j) >= 5)
                        {
                            dungeon[i, j] = ' ';
                        }
                    }
                }
            }
        }
        private void CreatePath(int x, int y)
        {
            int start_x = 1;
            int start_y = 1;
            while (start_x != x || start_y != y)
            {
                if (start_x != x)
                {
                    start_x++;
                    dungeon[start_y, start_x] = ' ';
                }
                else
                {
                    start_y++;
                    dungeon[start_y, start_x] = ' ';
                }

            }
        }
        private List<(int, int)> CheckForRooms()
        {
            List<(int, int)> centers = new List<(int, int)>();

            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    if (dungeon[j, i] == ' ')
                    {
                        centers.Add((i, j));
                    }
                }
            }

            return centers;
        }
        private int CheckApproximate(int x, int y)
        {
            int ret = 0;
            int[] dx = { 1, -1, 0, 0, 1, 1, -1, -1, 2, -2, -2, 2 };
            int[] dy = { 0, 0, 1, -1, 1, -1, 1, -1, 2, -2, 2, -2 };

            for (int i = 0; i < dx.Length; i++)
            {
                int newX = x + dx[i];
                int newY = y + dy[i];

                if (newX >= 0 && newX < width - 1 && newY >= 0 && newY < height - 1 && dungeon[newY, newX] == '█')
                {
                    ret++;
                }
            }

            return ret;
        }
    }
}
