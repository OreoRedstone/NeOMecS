using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NeOMecS.Utilities;

namespace NeOMecS.Physics;

public static class Simulation
{
    private static double gravitationalConstant = 1;
    private static List<Body> bodies = new List<Body>();
    private static double simSpeed = 1;

    public static void SimulateStep(long elapsedMilliseconds)
    {
        foreach (Body body in bodies)
        {
            foreach (Body other in bodies)
            {
                if (body == other) continue;

                if (Vector2.GetDistance(body.position, other.position) - (body.radius + other.radius) < 10)
                {
                    // COLLISION HANDLING
                    double coefficientOfRestitution = 1;
                    Vector2 normalVector = (other.position - body.position).Normalised;
                    double impulse = Vector2.DotProduct(-(1 + coefficientOfRestitution) * (body.velocity - other.velocity), normalVector) / Vector2.DotProduct(normalVector, normalVector * ((1 / body.mass) + (1 / other.mass)));
                    body.ApplyImpulse(impulse * normalVector);
                    other.ApplyImpulse(-impulse * normalVector);

                    continue;
                }
            }
        }
        foreach (Body body in bodies)
        {
            body.UpdateVelocityAndPosition(elapsedMilliseconds / 1000.0 / simSpeed);
        }
    }

    public static void CalculateAccelerations()
    {
        foreach (Body body in bodies)
        {
            var acceleration = Vector2.Zero;
            foreach (Body other in bodies)
            {
                if (body == other) continue;

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
    }

    public static Body[] GetBodiesAsArray()
    {
        return bodies.ToArray();
    }

    public static void Reset()
    {
        bodies.Clear();

        /*
        var sun = new Body("The Sun", 100, new Colour(1, 0, 0), new Vector2(100, 0), Vector2.Zero, Vector2.Zero, 100000000, "");
        AddBody(sun);

        var earth = new Body("Earth", 10, new Colour(0, 1, 1), new Vector2(-1000, 0), Vector2.Up * 200, Vector2.Zero, 1000, "The Earth");
        AddBody(earth);

        var moon = new Body("Moon", 5, new Colour(1, 1, 1), new Vector2(-1020, 0), Vector2.Up * 190, Vector2.Zero, 0.001, "The Sun");
        AddBody(moon);
        */
        var bodyA = new Body("BodyA", 100, new Colour(1, 0, 0), new Vector2(1000, 0), Vector2.Up * 100, Vector2.Zero, 1000000000, "");
        var bodyB = new Body("BodyB", 100, new Colour(0, 1, 0), new Vector2(-1000, 0), Vector2.Zero, Vector2.Zero, 1000000000, "");
        AddBody(bodyA);
        AddBody(bodyB);
    }

    public static void AddBody(Body body)
    {
        bodies.Add(body);
    }
}