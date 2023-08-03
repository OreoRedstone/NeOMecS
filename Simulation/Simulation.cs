using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeOMecS.Simulation
{
    class Simulation
    {
        private List<Body> bodies;
        private int simSpeed;

        public void SimulateStep()
        {

        }

        public Body[] GetBodies()
        {
            return bodies.ToArray();
        }
    }
}