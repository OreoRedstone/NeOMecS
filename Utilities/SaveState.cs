using NeOMecS.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeOMecS.Utilities
{
    public class SaveState
    {
        int simSpeed;
        Body[] bodies;

        public SaveState()
        {
            this.simSpeed = 0;
            this.bodies = new Body[0];
        }

        public SaveState(int simSpeed, Body[] bodies)
        {
            this.simSpeed = simSpeed;
            this.bodies = bodies;
        }
    }
}
