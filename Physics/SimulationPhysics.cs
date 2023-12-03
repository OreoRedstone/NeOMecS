using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NeOMecS.Interface;
using NeOMecS.Utilities;

namespace NeOMecS.Physics;

public static class SimulationPhysics
{
    public static readonly Stopwatch stepTimer = Stopwatch.StartNew();

    public static SimState SimulateStep(SimState state)
    {
        double elapsedSeconds = stepTimer.ElapsedTicks / (double)Stopwatch.Frequency * state.simSpeed;
        for (int i = 1; i < state.universe.bodies.Count; i++)
        {
            var body = state.universe.bodies[i];
            for (int j = 0; j < i; j++)
            {
                var other = state.universe.bodies[j];
                
                if (Vector2.GetDistance(body.position, other.position) - (body.radius + other.radius) < 1)
                {
                    // COLLISION HANDLING
                    double coefficientOfRestitution = 0.8;
                    Vector2 normalVector = (other.position - body.position).Normalised;
                    double impulse = Vector2.DotProduct(-(1 + coefficientOfRestitution) * (body.velocity - other.velocity), normalVector) / Vector2.DotProduct(normalVector, normalVector * ((1 / body.mass) + (1 / other.mass)));
                    body.ApplyImpulse(impulse * normalVector);
                    other.ApplyImpulse(-impulse * normalVector);
                    
                    continue;
                }
            }
            body.UpdateVelocity(elapsedSeconds);
            body.UpdatePosition(elapsedSeconds);
        }
        state.universe.bodies[0].UpdateVelocity(elapsedSeconds);
        state.universe.bodies[0].UpdatePosition(elapsedSeconds);
        stepTimer.Restart();
        return state;
    }

    public static SimState CalculateAccelerations(SimState state, SimulationWindow simWindow)
    {
        if (state.universe.bodies.Count < 2) return state;
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

            var previousParent = body.parent;
            var modifiedAccelerations = new Dictionary<Body, Vector2>();
            foreach (var entry in accelerations)
            {
                if(entry.Key.mass < body.mass) continue;
                var newValue = entry.Value / Vector2.GetDistance(body.position, entry.Key.position);
                modifiedAccelerations.Add(entry.Key, newValue);
            }

            try
            {
                if (body.mass == 0) throw new Exception();

                if(modifiedAccelerations.MaxBy(b => b.Value.Magnitude).Key.mass > body.mass)
                    body.parent = modifiedAccelerations.MaxBy(b => b.Value.Magnitude).Key;
                else
                    body.parent = state.universe;
            }
            catch (Exception)
            {
                body.parent = state.universe;
            }

            if (body.parent != previousParent) simWindow.UpdateBodySidebar();
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
                if (Double.IsNaN(body.acceleration.Magnitude)) continue;
                var parent = (Body)body.parent;
                var gravitationalParameter = state.universe.gravitationalConstant * (parent.mass + body.mass);
                var orbitalVelocity = body.velocity - parent.velocity;
                var orbitalPosition = body.position - parent.position;
                var h = orbitalPosition.Magnitude * orbitalVelocity.Magnitude * Math.Sin(Vector2.Angle(orbitalPosition, orbitalVelocity));
                var eccentricityVector = (((orbitalVelocity.Magnitude * orbitalVelocity.Magnitude / gravitationalParameter) - (1 / orbitalPosition.Magnitude)) * orbitalPosition) - (Vector2.DotProduct(orbitalPosition, orbitalVelocity) / gravitationalParameter * orbitalVelocity);
                List <Vector2> positions = new();
                var degrees = eccentricityVector.Magnitude > 1 ? 0 : 4 * Math.PI;
                for (double theta = 0; theta <= degrees; theta += 0.01)
                {
                    var r = (h * h) / (gravitationalParameter * (1 + eccentricityVector.Magnitude * Math.Cos(theta)));
                    var newPos = Vector2.PolarToVector2(r, theta + eccentricityVector.Theta, parent.position);
                    
                    if(positions.Count > 1 && Vector2.GetDistance(newPos, positions[^1]) > Vector2.GetDistance(positions[^1], positions[^2]) * 5)
                    {
                        continue;
                    }
                    
                    positions.Add(newPos);
                }
                paths.Add(body, positions);
            }
        }

        return paths;
    }
}