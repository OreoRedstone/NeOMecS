using NeOMecS.Physics;

namespace NeOMecS.Utilities;

public class SimState
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
}