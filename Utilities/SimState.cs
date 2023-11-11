using NeOMecS.Physics;
using System;

namespace NeOMecS.Utilities;

public class SimState : IEquatable<SimState>
{
    public double simSpeed;
    public Vector2 cameraPosition;
    public Universe universe;

    public SimState()
    {
        simSpeed = 1;
        cameraPosition = Vector2.Zero;
        universe = new Universe();
    }

    public SimState(double simSpeed, Universe universe, Vector2 cameraPosition)
    {
        this.simSpeed = simSpeed;
        this.cameraPosition = cameraPosition;
        this.universe = universe;
    }

    public SimState(SimState other)
    {
        simSpeed = other.simSpeed;
        cameraPosition = other.cameraPosition;
        universe = new Universe(other.universe);
    }

    public bool Equals(SimState? other)
    {
        if (other == null) return false;

        return simSpeed.Equals(other.simSpeed) && cameraPosition.Equals(other.cameraPosition) && universe.Equals(other.universe);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        SimState typedObj = (SimState)obj;

        return Equals(typedObj);
    }
}
