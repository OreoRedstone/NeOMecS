using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeOMecS.Utilities
{
    public static class SaveLoadSystem
    {
        public static int Save(SimState state)
        {
            return -1;
        }

        public static SimState Load(string path)
        {
            SimState state = new SimState();
            return state;
        }
    }
}