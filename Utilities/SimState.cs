using NeOMecS.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeOMecS.Utilities
{
    public class SimState
    {
        public readonly double simSpeed;
        public readonly Body[] bodies;
        public readonly Vector2 cameraPosition;
        public readonly double gravitationalConstant;

        public SimState()
        {
            this.simSpeed = 1;
            this.bodies = new Body[0];
            cameraPosition = Vector2.Zero;
            gravitationalConstant = 1;
        }

        public SimState(double simSpeed, Body[] bodies, Vector2 cameraPosition, double gravitationalConstant)
        {
            this.simSpeed = simSpeed;
            this.bodies = bodies;
            this.cameraPosition = cameraPosition;
            this.gravitationalConstant = gravitationalConstant;
        }
    }
}
