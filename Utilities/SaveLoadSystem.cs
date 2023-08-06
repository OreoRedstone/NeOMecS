using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeOMecS.Utilities
{
    public static class SaveLoadSystem
    {
        public static int Save(SaveState state)
        {
            return -1;
        }

        public static SaveState Load(string path)
        {
            SaveState state = new SaveState();
            return state;
        }
    }
}