using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeOMecS.Utilities;

namespace NeOMecS.Physics;

public static class Simulation
{
    private static double gravitationalConstant = 0.000000000001;
    private static List<Body> bodies = new List<Body>();
    private static double simSpeed = 0.0000001;

    public static void SimulateStep(long elapsedMilliseconds)
    {
        foreach (Body body in bodies)
        {
            var acceleration = new Vector2(0, 0);
            foreach (Body other in bodies)
            {
                if (body == other) continue;

                if (Vector2.GetDistance(body.position, other.position) < body.radius + other.radius)
                {
                    body.UpdatePositionForCollision(other);

                    continue;
                }

                //This finds the vector pointing from body to other, then normalises it.
                Vector2 direction = Vector2.GetNormalised(other.position - body.position);

                //The square of the distance between body and other.
                double distanceSquared = Math.Pow(Vector2.GetDistance(body.position, other.position), 2);

                //Multiplies all the values together in accordance with the equation.
                Vector2 thisAccel = direction * other.mass * gravitationalConstant / distanceSquared;

                //Adds the current acceleration onto the running total.
                acceleration += thisAccel;
            }
            body.UpdateAcceleration(acceleration);
        }
        foreach (Body body in bodies)
        {
            body.UpdateVelocityAndPosition(elapsedMilliseconds / 1000 / simSpeed);
        }
    }

    public static Body[] GetBodiesAsArray()
    {
        return bodies.ToArray();
    }

    public static void AddBody(Body body)
    {
        bodies.Add(body);
    }
}