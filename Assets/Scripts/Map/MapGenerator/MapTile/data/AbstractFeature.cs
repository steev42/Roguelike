using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFeature
{
    protected List<DungeonPoint> location;

    public AbstractFeature()
    {
        location = new List<DungeonPoint>();
    }

    public void addLocation(DungeonPoint dp)
    {
        if (null == location)
        {
            location = new List<DungeonPoint>();
        }
        location.Add(dp);
    }

    public List<DungeonPoint> getLocation()
    {
        return location;
    }
}
