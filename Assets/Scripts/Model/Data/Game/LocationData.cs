using UnityEngine;
using System.Collections.Generic;

public class LocationData
{
    public readonly Vector2 coordinate;
    public float movementSpeedMultiplier = 1.0f; // Is this needed anymore?  Can we calculate this based on other variables?

    private Dictionary<LightSource, float> lightLevel;
    public float totalLight;

    private bool needsUpdate = true;
    private List<IPhysicalObject> physicalObjects;
     
    // private Dictionary<DirectionEnum, WallStyle> walls;  // Walls are now just physical objects in the location.

    public LocationData(int x, int y)
    {
        lightLevel = new Dictionary<LightSource, float>();
        physicalObjects = new List<IPhysicalObject>();
        coordinate = new Vector2(x, y);
        SendUpdateNotification();
    }

    public bool InLineOfSight(Vector2 toLocation, int maxVisionRange = int.MaxValue, float squareSize = 1.0f)
    {
        if (Vector2.Distance(coordinate, toLocation) > maxVisionRange)
        {
            return false;
        }

        // Go just inside squares.  This prevents corner-to-corner shenanigans.
        float half = (squareSize / 2) - .001f;
        Vector2[] Corners =
            {
                new Vector2(-half, -half),
                new Vector2(half, half),
                new Vector2(-half, half),
                new Vector2(half, -half)
            };
        for (int i = 0; i < Corners.Length; i++)
        {
            for (int j = 0; j < Corners.Length; j++)
            {
                Vector2 start = coordinate + Corners[i];
                Vector2 end = toLocation + Corners[j];
                RaycastHit2D hit = Physics2D.Linecast(start, end);
                if (hit.collider == null)
                {
                    //  Debug.Log("From " + start + " to " + end + " hits nothing.");
                    return true;
                }
                else
                {
                    Vector2 hitpos = (Vector2)hit.collider.transform.position;
                    if (hitpos.Equals(coordinate) || hitpos.Equals(toLocation))
                    {
                        //         Debug.Log("From " + start + " to " + end + " hits a wall, but one of those IS a wall.");
                        return true;
                    }
                    //      Debug.Log("From " + start.ToString() + " to " + end.ToString() + " hits a wall.");
                }
            }

        }
        return false;
    }

    public List<IPhysicalObject> GetContents()
    {
        return physicalObjects;
    }

    private void SendUpdateNotification()
    {
    }

    public bool IsTileLockedTo(IPhysicalObject o)
    {
        if (physicalObjects != null)
        {
            foreach (IPhysicalObject content in physicalObjects)
            {
               // Debug.Log("Examining " + content.ToString());
                if (content.isLockedTo(o))
                {
                    //Debug.Log("Tile locked due to presence of " + content.ToString());
                    return true;
                }
            }
            return false;
        }
        return false;
    }

    public bool JoinTile(IPhysicalObject o)
    {
        //Debug.Log("Called JoinTile");
        if (physicalObjects != null)
        {
            //Debug.Log("Tile contains " + physicalObjects.Count + " objects.");
            foreach (IPhysicalObject content in physicalObjects)
            {
               // Debug.Log("Looking at " + content.ToString());
                if (content.isLockedTo(o))
                {
                    //Debug.Log("Tile is occupied.");
                    return false;
                }
            }

            physicalObjects.Add(o);
            SendUpdateNotification();
            //Debug.Log("Successfully joined tile. Now contains " + physicalObjects.Count);
            return true;
        }

        //Debug.Log("physicalObjects are null");
        return false;
    }

    public void LeaveTile(IPhysicalObject o)
    {
        //Debug.Log("Calling LeaveTile");
        if (physicalObjects != null)
        {
            physicalObjects.Remove(o);
            SendUpdateNotification();
        }
    }

    public bool ContainsAttackableObject()
    {
        return (GetAttackableObject() != null);
    }

    public IAttackableObject GetAttackableObject()
    {
        foreach (IPhysicalObject content in physicalObjects)
        {
            if (content is IAttackableObject)
            {
                return (IAttackableObject)content;
            }
            Debug.Log(content.ToString() + " isn't able to be attacked.");
        }
        return null;
    }

    public float GetLightLevel (LightSource ls)
    {
        if (lightLevel != null && lightLevel.ContainsKey(ls))
        {
            return lightLevel[ls];
        }
        else
        {
            return 0;
        }
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

    public void SetLightLevel (LightSource ls, float level)
    {
        if (lightLevel == null)
        {
            lightLevel = new Dictionary<LightSource, float>();
        }
        lightLevel[ls] = level;

        SendUpdateNotification();
    }

}