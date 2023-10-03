﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeOMecS.Physics;

public class Universe : ParentableObject
{
    public List<Body> bodies;
    public double gravitationalConstant;

    public Universe()
    {
        name = "Universe";
        bodies = new();
        gravitationalConstant = 1;
    }

    public Universe(List<Body> bodies, double gravitationalConstant)
    {
        name = "Universe";
        this.bodies = bodies;
        this.gravitationalConstant = gravitationalConstant;
    }

    public Universe(Universe other)
    {
        gravitationalConstant = other.gravitationalConstant;
        bodies = new();
        foreach(Body b in other.bodies)
        {
            bodies.Add(new Body(b));
        }
    }

    public void AddBody(Body body)
    {
        bodies.Add(body);
    }
}