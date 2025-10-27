using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public  class Instructions
    {
        public StringBuilder instructions;
        public Instructions() {
            instructions = new StringBuilder();
        }
        public Instructions ItemInstruction(bool flag)
        { 
            if(flag) { 
            instructions.AppendLine("E - to pick up item");
            }

            return this;

        }
        public Instructions MovingInstruction()
        {
            instructions.AppendLine("WASD- to move");
            return this;
        }
        public Instructions EnemyInstruction(bool flag)
        {
            if(flag) { 
            instructions.AppendLine(" %% - To damage enemies");
            }
            return this;

        }
        public Instructions InvetoryInstruction(bool flag) {
           if(flag) { 
            instructions.AppendLine("Press I to open your inventory");
           }
            return this;
        }
        public string Build()
        {
            return instructions.ToString();
        }

    }
}
