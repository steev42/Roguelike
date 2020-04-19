using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractMapLevel
{
    public abstract class Parameters
    {

    }

    public abstract void GenerateLevel();
    public abstract void SetParameters(Parameters p);
    private Parameters levelParameters;

    private Dictionary<Vector2, LocationData> data = new Dictionary<Vector2, LocationData>();
    public Dictionary<Vector2, LocationData> mapData { get { return data; } }

    public LocationData GetLocation(Vector2 coord)
    {
        if (data.ContainsKey(coord))
            return data[coord];
        else
            return null;
    }

    public abstract int Width { get; }
    public abstract int Height { get; }
}
