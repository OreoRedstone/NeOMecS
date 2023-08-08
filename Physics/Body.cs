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
        public string name { get; private set; }
        private string parent;
        public double radius { get; private set; }
        public Colour colour { get; private set; }
        public Vector2 position { get; private set; }
        private Vector2 velocity;
        private Vector2 acceleration;
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
            Simulation.AddBody(this);
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