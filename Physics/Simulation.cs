using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeOMecS.Utilities;

namespace NeOMecS.Physics
{
    public static class Simulation
    {
        private static double gravitationalConstant = 1;
        private static List<Body> bodies = new List<Body>();
        private static int simSpeed;

        public static void SimulateStep()
        {
            foreach (Body body in bodies)
            {
                Vector2 acceleration = new Vector2(0, 0);
                foreach (Body other in bodies)
                {
                    if(body != other)
                    {
                        //This finds the vector pointing from body to other, then normalises it.
                        Vector2 direction = Vector2.GetNormalised(other.GetPosition() - body.GetPosition());

                        //The square of the distance between body and other.
                        double distanceSquared = Math.Pow(Vector2.GetDistance(body.GetPosition(), other.GetPosition()), 2);

                        //Multiplies all the values together in accordance with the equation.
                        Vector2 thisAccel = direction * body.GetMass() * gravitationalConstant / distanceSquared;

                        //Adds the current acceleration onto the running total.
                        acceleration += thisAccel;
                    }
                }
                body.UpdateAcceleration(acceleration);
            }
            foreach (Body body in bodies)
            {
                body.UpdateVelocityAndPosition(1f);
            }
        }

        public static Body[] GetBodies()
        {
            return bodies.ToArray();
        }

        public static void AddBody(Body body)
        {
            bodies.Add(body);
        }
    }
}