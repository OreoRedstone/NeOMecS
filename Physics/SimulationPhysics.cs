﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NeOMecS.Interface;
using NeOMecS.Utilities;

namespace NeOMecS.Physics;

public static class SimulationPhysics
{
    public static SimState SimulateStep(SimState state, long elapsedMilliseconds)
    {
        for (int i = 0; i < state.universe.bodies.Count - 1; i++)
        {
            var body = state.universe.bodies[i];
            for (int j = 1; j < state.universe.bodies.Count; j++)
            {
                if (j <= i) continue;
                var other = state.universe.bodies[j];

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
        foreach (Body body in state.universe.bodies)
        {
            var accelerations = new Dictionary<Body, Vector2>();
            var totalAccel = Vector2.Zero;
            foreach (Body other in state.universe.bodies)
            {
                if (body == other) continue;

                //This finds the vector pointing from body to other, then normalises it.
                Vector2 direction = Vector2.GetNormalised(other.position - body.position);

                //The square of the distance between body and other.
                double distanceSquared = Math.Pow(Vector2.GetDistance(body.position, other.position), 2);

                //Multiplies all the values together in accordance with the equation.
                Vector2 thisAccel = direction * other.mass * state.universe.gravitationalConstant / distanceSquared;

                //Adds the current acceleration onto the running total.
                accelerations.Add(other, thisAccel);
                totalAccel += thisAccel;
            }
            body.UpdateAcceleration(totalAccel);

            var greatestAccel = new KeyValuePair<Body, Vector2>();
            foreach (var entry in accelerations)
            {
                if (greatestAccel.Value == null) greatestAccel = entry;
                if (entry.Value.Magnitude > greatestAccel.Value.Magnitude) greatestAccel = entry;
            }
            if (greatestAccel.Value.Magnitude / totalAccel.Magnitude > 0.99) body.parent = greatestAccel.Key;
            else body.parent = Simulation.simulation.universe;
        }

        return state;
    }

    public static Dictionary<Body, List<Vector2>> CalculateOrbits(SimState state)
    {
        Dictionary<Body, List<Vector2>> paths = new();
        foreach (Body body in state.universe.bodies)
        {
            if (body.parent.GetType() == typeof(Body))
            {
                var parent = (Body)body.parent;
                var orbitalVelocity = body.velocity - parent.velocity;
                var orbitalPosition = body.position - parent.position;
                var orbitEnergy = (orbitalVelocity.Magnitude * orbitalVelocity.Magnitude / 2) - (state.universe.gravitationalConstant * (body.mass + parent.mass) / orbitalPosition.Magnitude);
                var l = body.mass * orbitalPosition.Magnitude * orbitalVelocity.Magnitude * Math.Sin(Vector2.Angle(orbitalPosition, orbitalVelocity));
                var eccentricity = Math.Sqrt(1 + (2 * orbitEnergy * l * l) / (body.mass * body.mass * body.mass * state.universe.gravitationalConstant * state.universe.gravitationalConstant * parent.mass * parent.mass));
                List<Vector2> positions = new();
                for (int i = 0; i < 360; i++)
                {
                    var r = (l * l) / ((body.mass * body.mass * parent.mass * state.universe.gravitationalConstant) * (1 + eccentricity * Math.Cos(i * Math.PI / 180)));
                    positions.Add(Vector2.PolarToVector2(r, i * Math.PI / 180, parent.position));
                }
                paths.Add(body, positions);
            }
        }

        return paths;
    }
}