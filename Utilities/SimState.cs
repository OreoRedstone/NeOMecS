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
        public readonly List<Body> bodies;
        public readonly Vector2 cameraPosition;
        public readonly double gravitationalConstant;

        public SimState()
        {
            simSpeed = 1;
            bodies = new();
            cameraPosition = Vector2.Zero;
            gravitationalConstant = 1;
        }

        public SimState(double simSpeed, Body[] bodies, Vector2 cameraPosition, double gravitationalConstant)
        {
            this.simSpeed = simSpeed;
            this.bodies = bodies.ToList();
            this.cameraPosition = cameraPosition;
            this.gravitationalConstant = gravitationalConstant;
        }

        public void AddBody(Body body)
        {
            bodies.Add(body);
        }
    }
}
