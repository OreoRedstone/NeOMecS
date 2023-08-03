using NeOMecS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeOMecS.Physics
{
    class Body
    {
        private double radius;
        private Colour colour;
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 acceleration;
        private double mass;

        public Body(double radius, Colour colour, Vector2 position, Vector2 velocity, Vector2 acceleration, double mass)
        {
            this.radius = radius;
            this.colour = colour;
            this.position = position;
            this.velocity = velocity;
            this.acceleration = acceleration;
            this.mass = mass;
        }

        public Vector2 GetPosition()
        {
            return position;
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
    }
}