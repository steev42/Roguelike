using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public float movementSpeedMultiplier = 1.0f;
    public float opacity = 1.0f;
    // Amount of light that gets through this square.
	
    public Color originalColor;
    private Dictionary<LightSource, float> lightLevel;

    public TileData()
    {
        lightLevel = new Dictionary<LightSource, float>();
    }

    public float GetTotalLightLevel()
    {
        float total = 0f;
        foreach (LightSource l in lightLevel.Keys)
        {
            total += lightLevel[l];
        }

        return total;
    }

    public void SetLightLevel(LightSource source, float light)
    {
        if (lightLevel == null)
        {
            lightLevel = new Dictionary<LightSource, float>();
        }
        lightLevel[source] = light;
    }

    public bool isWall()
    {
        return (movementSpeedMultiplier == 0f);
    }

}
