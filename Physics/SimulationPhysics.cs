﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NeOMecS.Utilities;

namespace NeOMecS.Physics;

public static class SimulationPhysics
{
    public static SimState SimulateStep(SimState state, long elapsedMilliseconds)
    {
        for (int i = 0; i < state.bodies.Count - 1; i++)
        {
            var body = state.bodies[i];
            for (int j = 1; j < state.bodies.Count; j++)
            {
                if (j <= i) continue;
                var other = state.bodies[j];

                body.UpdatePosition(elapsedMilliseconds / 1000.0 / state.simSpeed);
                other.UpdatePosition(elapsedMilliseconds / 1000.0 / state.simSpeed);

                if (Vector2.GetDistance(body.position, other.position) - (body.radius + other.radius) < 1)
                {
                    // COLLISION HANDLING
                    double coefficientOfRestitution = 1;
                    Vector2 normalVector = (other.position - body.position).Normalised;
                    double impulse = Vector2.DotProduct(-(1 + coefficientOfRestitution) * (body.velocity - other.velocity), normalVector) / Vector2.DotProduct(normalVector, normalVector * ((1 / body.mass) + (1 / other.mass)));
                    body.ApplyImpulse(impulse * normalVector);
                    other.ApplyImpulse(-impulse * normalVector);

                    continue;
                }

                body.UpdateVelocity(elapsedMilliseconds / 1000.0 / state.simSpeed);
                other.UpdateVelocity(elapsedMilliseconds / 1000.0 / state.simSpeed);
            }
        }

        return state;
    }

    public static SimState CalculateAccelerations(SimState state)
    {
        foreach (Body body in state.bodies)
        {
            var acceleration = Vector2.Zero;
            foreach (Body other in state.bodies)
            {
                if (body == other) continue;

                //This finds the vector pointing from body to other, then normalises it.
                Vector2 direction = Vector2.GetNormalised(other.position - body.position);

                //The square of the distance between body and other.
                double distanceSquared = Math.Pow(Vector2.GetDistance(body.position, other.position), 2);

                //Multiplies all the values together in accordance with the equation.
                Vector2 thisAccel = direction * other.mass * state.gravitationalConstant / distanceSquared;

                //Adds the current acceleration onto the running total.
                acceleration += thisAccel;
            }
            body.UpdateAcceleration(acceleration);
        }

        return state;
    }
}