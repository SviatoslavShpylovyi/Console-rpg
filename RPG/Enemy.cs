using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public interface IEnemyBehave
    {
        public void Behave(World world, Enemy enemy);
    }
   public  class RandomBehavior : IEnemyBehave
{
    public void Behave(World world, Enemy enemy)
    {
        Random rand = new Random();
        int direction = rand.Next(0, 4);

        int newX = enemy.X;
        int newY = enemy.Y;

        switch (direction)
        {
            case 0: newX++; break;
            case 1: newX--; break;
            case 2: newY++; break;
            case 3: newY--; break;
        }

        ChangeEnemyPos(world, enemy, newX, newY);
    }

    public  void ChangeEnemyPos(World world, Enemy enemy, int newX, int newY)
    {
            if (world.CheckCords(newX, newY) && world.dung.dungeon[newY, newX] == ' ' )
            {
                world.dung.Enemies_in_world.Remove((enemy.X, enemy.Y));
                world.dung.Symbols.Remove(((enemy.X, enemy.Y)));
                enemy.X = newX;
                enemy.Y = newY;
                world.dung.Enemies_in_world.Add((enemy.X, enemy.Y), enemy);
                world.dung.Symbols.Add((enemy.X, enemy.Y),'E');
        }
    }
}
    public class ChillBehavior : IEnemyBehave
    {
        public void Behave(World world, Enemy enemy)
        {
            return;
        }
    }
    public class AggresiveBehaviour : IEnemyBehave
    {
        public void Behave(World world, Enemy enemy)
        {
            int distance = 999;
            Player closest_player;
            int dx = 0, dy=0;
            foreach (Player p in world.Players.Values)
            {
                int distanceX = p.GetX() - enemy.X;
                int distanceY = p.GetY() - enemy.Y;
                int temp = distance;
                distance = Math.Min(distance, Math.Abs(distanceX) + Math.Abs(distanceY));
                if (temp > distance)
                {
                    closest_player = p;
                    dx = distanceX;
                    dy = distanceY;
                }
            }
            int ChangedX = 0, ChangedY = 0;
            if (distance <= 5)
            {
                if (Math.Abs(dx) > Math.Abs(dy))
                    ChangedX = dx > 0 ? 1 : -1;
                else if (dy != 0)
                {
                    ChangedY = dy > 0 ? 1 : -1;
                }

                ChangeEnemyPos(world, enemy, enemy.X + ChangedX, enemy.Y + ChangedY);
            }
        }
        public  void ChangeEnemyPos(World world, Enemy enemy, int newX, int newY)
    {
            if (world.CheckCords(newX, newY) && world.dung.dungeon[newY, newX] == ' ' )
            {
                world.dung.Enemies_in_world.Remove((enemy.X, enemy.Y));
                world.dung.Symbols.Remove(((enemy.X, enemy.Y)));
                enemy.X = newX;
                enemy.Y = newY;
                world.dung.Enemies_in_world.Add((enemy.X, enemy.Y), enemy);
                world.dung.Symbols.Add((enemy.X, enemy.Y),'E');
        }
    }
    }
    public interface IEnemy
    {
        string Name { get; }
        int Damage { get; }
        int Armour { get; }
        int Health { get; }

    }
    public class Enemy : IEnemy
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int Health { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Armour { get; }
        public bool AtBatlle = false;
        public IEnemyBehave behave;
        public void SetBehavior(IEnemyBehave behavior)
        {
            behave = behavior;
        }
        public void DoSomething(World world)
        {
            behave.Behave(world, this);          
        }
        public Enemy(string Name, int Damage, int Armour, int health, int X, int Y)
        {
            this.Name = Name;
            this.Damage = Damage;
            this.Armour = Armour;
            this.Health = health;
            this.X = X;
            this.Y = Y;
        }

    }
    public class OGR : Enemy
    {
        public OGR(int X, int Y) : base("OGR", 10, 5, 20, X, Y)
        {
            behave = new RandomBehavior();


        }
    }
    public class Skeleton : Enemy
    {
        public Skeleton(int X, int Y) : base("Skeleton", 14, 0, 40, X, Y)
        {
        behave = new AggresiveBehaviour();    
        }
    }
    public class ChillGoblin : Enemy
    {
        public ChillGoblin(int X, int Y) : base("ChillGoblin", 14, 0, 40, X, Y)

        {
            behave = new ChillBehavior();
        }
    }
}
