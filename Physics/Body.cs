using NeOMecS.Utilities;
using System;
using System.Collections.Generic;

namespace NeOMecS.Physics;

public class Body : ParentableObject
{
    public string guid;

    public ParentableObject parent;
    public double radius;
    public Colour colour;
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    public double mass;

    public Body(string name, double radius, Colour colour, Vector2 position, Vector2 velocity, Vector2 acceleration, double mass, ParentableObject parent)
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

    public Body(Body other)
    {
        guid = other.guid;
        name = other.name;
        radius = other.radius;
        colour = other.colour;
        position = other.position;
        velocity = other.velocity;
        acceleration = other.acceleration;
        mass = other.mass;
        parent = other.parent;
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

    //Gets the number of graph nodes this body has above it, before the root is reached.
    public int GetParentNestingCount()
    {
        var parents = new List<ParentableObject>();
        Body body = this;
        int layers = -1;
        var shouldContinue = true;
        while(shouldContinue)
        {
            layers++;
            if(body.parent.GetType() == typeof(Body))
            {
                body = (Body)body.parent;
                if (parents.Contains(body)) break;
                parents.Add(body);
            }
            else if(body.parent.GetType() == typeof(Universe))
            {
                shouldContinue = false;
            }
        }
        return layers;
    }
}
