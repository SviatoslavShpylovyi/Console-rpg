using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class InstructionMaker
    {
        private Instructions Instructions;
        public InstructionMaker() { Instructions = new Instructions(); }
        public string GetInstruction(Dungeon dung)
        {
            bool item=false;
            bool enemy =false;
            if (dung.Items_in_world.Count() > 0)
            {
                item = true;
            }
            if (dung.Enemies_in_world.Count() > 0)
            {
                enemy = true;
            }
           return  Instructions.ItemInstruction(item).InvetoryInstruction(item).EnemyInstruction(enemy).MovingInstruction().Build();
        }
    }
}
