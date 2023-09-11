using NeOMecS.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeOMecS.Physics;

public class Body
{
    public string name { get; private set; }
    private string parent;
    public double radius { get; private set; }
    public Colour colour { get; private set; }
    public Vector2 position { get; private set; }
    public Vector2 velocity { get; private set; }
    public Vector2 acceleration { get; private set; }
    public double mass { get; private set; }

    public Body(string name, double radius, Colour colour, Vector2 position, Vector2 velocity, Vector2 acceleration, double mass, string parent)
    {
        this.name = name;
        this.radius = radius;
        this.colour = colour;
        this.position = position;
        this.velocity = velocity;
        this.acceleration = acceleration;
        this.mass = mass;
        this.parent = parent;
    }

    public void UpdateAcceleration(Vector2 newAcceleration)
    {
        var closestBody = Simulation.GetBodiesAsArray().Where(b => b != this).Min(b => Vector2.GetDistance(b.position, position) - b.radius) - radius;
        if (closestBody < 100) return;
        acceleration = newAcceleration;
    }

    public void UpdateVelocityAndPosition(double elapsedSeconds)
    {
        var closestBody = Simulation.GetBodiesAsArray().Where(b => b != this).Min(b => Vector2.GetDistance(b.position, position) - b.radius - radius);
        if (closestBody < 100) return;

        velocity += acceleration * elapsedSeconds;

        position += velocity * elapsedSeconds;
    }

    public void UpdatePositionForCollision(Body other)
    {
        if(position == other.position)
        {
            position = position - velocity.Normalized * radius / 2;
        }
        else
        {
            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;

            Vector2 centralPoint = (position + other.position) / 2;
            var distanceFromCentralPoint = (radius + other.radius) / 2;
            var bodyToOther = Vector2.GetNormalised(other.position - position);

            position = centralPoint - (bodyToOther * distanceFromCentralPoint);
        }
    }
}