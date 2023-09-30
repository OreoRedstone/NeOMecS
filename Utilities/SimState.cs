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
        public double simSpeed;
        public List<Body> bodies;
        public Vector2 cameraPosition;
        public double gravitationalConstant;

        public SimState()
        {
            simSpeed = 1;
            bodies = new();
            cameraPosition = Vector2.Zero;
            gravitationalConstant = 1;
        }

        public SimState(double simSpeed, List<Body> bodies, Vector2 cameraPosition, double gravitationalConstant)
        {
            this.simSpeed = simSpeed;
            this.bodies = bodies;
            this.cameraPosition = cameraPosition;
            this.gravitationalConstant = gravitationalConstant;
        }

        public void AddBody(Body body)
        {
            bodies.Add(body);
        }

        public SimState(SimState other)
        {
            simSpeed = other.simSpeed;
            cameraPosition = other.cameraPosition;
            gravitationalConstant = other.gravitationalConstant;
            bodies = new List<Body>();
            foreach (Body body in other.bodies)
            {
                bodies.Add(new Body(body));
            }
        }
    }
}
