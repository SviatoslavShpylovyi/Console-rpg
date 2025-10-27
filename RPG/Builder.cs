using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class DungeonBuilder 
    {

        private Dungeon dungeon;
        private int width;
        private int height;
        public DungeonBuilder(int width, int height)
        {
            this.width = width;
            this.height = height;
            dungeon = new Dungeon(width, height);
        }
        public DungeonBuilder EmptyDungeon()
        {
            dungeon.EmptyDungeon();
            return this;
        }
         public DungeonBuilder FilledDungeon()
        {
            dungeon.FilledDungeon();
            return this;
        }
        public DungeonBuilder AddPath()
        {
            dungeon.AddPath();
            return this;
        }
        public DungeonBuilder AddChamber(int n = 4, int width = 4, int height = 4)
        {
            dungeon.AddChamber(n, width, height);
            return this;
        }
        public DungeonBuilder CentralRoom(int new_width, int new_height)
        {
            dungeon.CentralRoom(new_width, new_height);
            return this;
        }
        public DungeonBuilder AddItem()
        {
            dungeon.AddItem();
            return this;
        }
        public DungeonBuilder AddWeapons()
        {
            dungeon.AddWeapons();
            return this;
        }
        public DungeonBuilder AddModifiedWeapons()
        {
            dungeon.AddModifiedWeapons();
            return this;
        }
        public DungeonBuilder AddPotions()
        {
            dungeon.AddPotions();
            return this;
        }
        public DungeonBuilder AddEnemies()
        {
            dungeon.AddEnemies();
            return this;
        }
         public Dungeon GameWorld()
        {
            return dungeon;
        }
        public Dungeon NormalGeneration()
        {
            dungeon.EmptyDungeon();
            dungeon.FilledDungeon();
            dungeon.AddChamber();
            dungeon.AddPath();
            int weapon_num;
            int enemy_num;
           Random rand = new Random();
            weapon_num = rand.Next(3, 8);
            enemy_num = rand.Next(4, 6);
            for(int i =0; i< weapon_num; i++) {
                dungeon.AddWeapons();
            }
            for (int i = 0; i < enemy_num; i++)
            {
                dungeon.AddEnemies();
            }
            for(int i =0; i< weapon_num; i++) {
                dungeon.AddItem();
                dungeon.AddPotions();
            }

            return dungeon;
        }
    }
}