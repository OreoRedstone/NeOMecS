using System;
using System.Collections.Generic;
using System.Linq;

namespace NeOMecS.Physics;

public class Universe : ParentableObject, IEquatable<Universe>
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

    public List<Body> GetBodiesOrdered()
    {
        var orderedBodies = bodies.OrderBy(x => x.GetParentNestingCount(this)).ToList();
        List<Body> newList = new();
        foreach (var item in orderedBodies)
        {
            newList.Add(item);
        }
        foreach (var body in orderedBodies)
        {
            if(body.parent == this)
            {
                newList = BringChildrenUp(newList, body);
            }
        }

        return newList;
    }

    private List<Body> BringChildrenUp(List<Body> list, Body parent)
    {
        if (GetBodyChildren(parent).Count > 0)
        {
            foreach (var child in GetBodyChildren(parent))
            {
                list.Remove(child);
                list.Insert(list.FindIndex(b => b == parent) + 1, child);
                list = BringChildrenUp(list, child);
            }
        }
        return list;
    }

    private List<Body> GetBodyChildren(Body body)
    {
        var children = new List<Body>();
        foreach( Body b in bodies)
        {
            if(b.parent == body) children.Add(b);
        }
        return children;
    }

    public bool Equals(Universe? other)
    {
        if (other == null) return false;

        return bodies.Equals(other.bodies) && gravitationalConstant.Equals(other.gravitationalConstant);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        Universe typedObj = (Universe)obj;

        return Equals(typedObj);
    }
}
