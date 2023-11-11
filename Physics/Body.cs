﻿using NeOMecS.Utilities;
using System;

namespace NeOMecS.Physics;

public class Body : ParentableObject, IEquatable<Body>
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

    public int GetParentNestingCount(ParentableObject universe)
    {
        Body body = this;
        int layers = -1;
        while(body != universe)
        {
            layers++;
            if(body.parent.GetType() == typeof(Body))
            {
                body = (Body)body.parent;
            }
            else if(body.parent.GetType() == typeof(Universe))
            {
                break;
            }
        }
        return layers;
    }

    public bool Equals(Body? other)
    {
        if (other == null) return false;

        return mass.Equals(other.mass) && radius.Equals(other.radius) && position.Equals(other.position) && velocity.Equals(other.velocity);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        Body typedObj = (Body)obj;

        return Equals(typedObj);
    }
}