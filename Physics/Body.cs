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
    public string guid;

    public string name;
    private string parent;
    public double radius;
    public Colour colour;
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    public double mass;

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

        guid = Guid.NewGuid().ToString();
    }

    public void UpdateAcceleration(Vector2 newAcceleration)
    {
        acceleration = newAcceleration;
    }

    public void ApplyImpulse(Vector2 impulse)
    {
        velocity += impulse / mass;
    }

    public void UpdateVelocity(double elapsedSeconds)
    {
        var deltaVelocity = acceleration * elapsedSeconds;
        velocity += deltaVelocity;
    }

    public void UpdatePosition(double elapsedSeconds)
    {
        var deltaPosition = velocity * elapsedSeconds;
        position += deltaPosition;
    }

    public void UpdatePositionForCollision(Body other)
    {
        if(position == other.position)
        {
            position += Vector2.GetNormalised(velocity) * radius / -2;
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