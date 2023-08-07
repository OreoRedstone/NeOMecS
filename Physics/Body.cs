using NeOMecS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeOMecS.Physics
{
    public class Body
    {
        private string name;
        private string parent;
        private double radius;
        private Colour colour;
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 acceleration;
        private double mass;

        public Body(string name, double radius, Colour colour, Vector2 position, Vector2 velocity, Vector2 acceleration, double mass)
        {
            this.name = name;
            this.radius = radius;
            this.colour = colour;
            this.position = position;
            this.velocity = velocity;
            this.acceleration = acceleration;
            this.mass = mass;
            Simulation.AddBody(this);
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public double GetRadius()
        {
            return radius;
        }

        public double GetMass()
        {
            return mass;
        }

        public void UpdateAcceleration(Vector2 newAcceleration)
        {
            acceleration = newAcceleration;
        }

        public void UpdateVelocityAndPosition(double timeStep)
        {
            velocity += acceleration * timeStep;
            position += velocity * timeStep;
        }

        public string GetName()
        {
            return name;
        }

        public Colour GetColour()
        {
            return colour;
        }
    }
}