using UnityEngine;
using System.Collections.Generic;

public class LocationData
{
    private readonly Vector2 coordinate;
    public Vector2 Location { get { return coordinate; } }
    public float movementSpeedMultiplier = 1.0f; // Is this needed anymore?  Can we calculate this based on other variables?

    private AbstractMapLevel map;
    public AbstractMapLevel Map { get { return map; } }

    private Dictionary<ISenseProperty, float> senseSources;
    private Dictionary<SenseEnum, float> totalSenseLevels;
    private Dictionary<SenseEnum, float> senseAbsorption;

    private Dictionary<ISenseProperty, LocationData> expansionXNeighbor;
    private Dictionary<ISenseProperty, LocationData> expansionYNeighbor;
    private Dictionary<ISenseProperty, LocationData> expansionXYNeighbor;


    /*private Dictionary<LightSource, float> lightLevel;
    public float totalLight;*/

    private bool needsUpdate = true;
    private List<IPhysicalObject> physicalObjects;
     
    // private Dictionary<DirectionEnum, WallStyle> walls;  // Walls are now just physical objects in the location.

    public LocationData(int x, int y, AbstractMapLevel parentMap)
    {
        senseSources = new Dictionary<ISenseProperty, float>();
        totalSenseLevels = new Dictionary<SenseEnum, float>();
        senseAbsorption = new Dictionary<SenseEnum, float>();
        physicalObjects = new List<IPhysicalObject>();
        coordinate = new Vector2(x, y);
        map = parentMap;
        expansionXNeighbor = new Dictionary<ISenseProperty, LocationData>();
        expansionYNeighbor = new Dictionary<ISenseProperty, LocationData>();
        expansionXYNeighbor = new Dictionary<ISenseProperty, LocationData>();
        SendUpdateNotification();
    }

    public float GetSenseIntensity(ISenseProperty source)
    {
        if (senseSources.ContainsKey(source))
            return senseSources[source];
        return 0f;
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
        Debug.Log("Called JoinTile");
        if (physicalObjects != null)
        {
            Debug.Log("Tile contains " + physicalObjects.Count + " objects.");
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
            o.LocationData = this;
            SendUpdateNotification();
            Debug.Log("Successfully joined tile. Now contains " + physicalObjects.Count);
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
            // TODO -- what do we do with LocationData if we don't have one?
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

    public float GetSenseLevel (ISenseProperty ls)
    {
        if (senseSources != null && senseSources.ContainsKey(ls))
        {
            return senseSources[ls];
        }
        else
            return 0f;
    }

    public float GetTotalSenseLevel(SenseEnum sense)
    {
        if (totalSenseLevels != null && totalSenseLevels.ContainsKey(sense))
        {
            return totalSenseLevels[sense];
        }
        else
        {
            return 0f;
        }
    }

    public void AddSenseSource(ISenseProperty source, float intensity)
    {
        if (intensity < Parameters.ABSOLUTE_MINIMUM_SENSE)
            return;

        if (senseSources.ContainsKey(source))
        {
            totalSenseLevels[Sense.GetSense(source)] += (intensity - senseSources[source]); // Increase total by the difference between what this source was before and what it is now.
            senseSources[source] = intensity;
        }
        else
        {
            senseSources[source] = intensity;
            totalSenseLevels[Sense.GetSense(source)] = intensity;
        }

        float passedIntensity = intensity;

        if (senseAbsorption.ContainsKey(Sense.GetSense(source)))
            passedIntensity = (intensity * senseAbsorption[Sense.GetSense(source)]);

        map.GetLocation(new Vector2(Location.x - 1, Location.y))?.ExpandSenseSource(source, passedIntensity, this);
        map.GetLocation(new Vector2(Location.x + 1, Location.y))?.ExpandSenseSource(source, passedIntensity, this);
        map.GetLocation(new Vector2(Location.x, Location.y - 1))?.ExpandSenseSource(source, passedIntensity, this);
        map.GetLocation(new Vector2(Location.x, Location.y + 1))?.ExpandSenseSource(source, passedIntensity, this);

        /*map.GetLocation(new Vector2(Location.x - 1, Location.y - 1))?.ExpandSenseSource(source, passedIntensity, this);
        map.GetLocation(new Vector2(Location.x + 1, Location.y - 1))?.ExpandSenseSource(source, passedIntensity, this);
        map.GetLocation(new Vector2(Location.x + 1, Location.y - 1))?.ExpandSenseSource(source, passedIntensity, this);
        map.GetLocation(new Vector2(Location.x - 1, Location.y + 1))?.ExpandSenseSource(source, passedIntensity, this);  */     
    }


    public void ExpandSenseSource(ISenseProperty source, float intensity, LocationData expandingFrom)
    {

        float distance = Vector2.Distance(source.Location, Location);

        if (expandingFrom.Location.x == Location.x && expandingFrom.Location.y == Location.y)
        {
            // We should NOT be here.
            return;
        }
        else if (expandingFrom.Location.x == Location.x)
        {
            expansionXNeighbor[source] = expandingFrom;
        }
        else if (expandingFrom.Location.y == Location.y)
        {
            expansionYNeighbor[source] = expandingFrom;
        }
        else
        {
            expansionXYNeighbor[source] = expandingFrom;
        }

        float totalIntensity = 0f;
        int neighbors = 0;

        if (expansionXNeighbor.ContainsKey(source))
        {
            float XDist = Vector2.Distance(source.Location, expansionXNeighbor[source].Location);
            totalIntensity += (intensity * XDist * XDist);
            neighbors++;
        }
        if (expansionYNeighbor.ContainsKey(source))
        {
            float YDist = Vector2.Distance(source.Location, expansionYNeighbor[source].Location);
            totalIntensity += (intensity * YDist * YDist);
            neighbors++;
        }
        if (expansionXYNeighbor.ContainsKey(source))
        {
            float XYDist = Vector2.Distance(source.Location, expansionXYNeighbor[source].Location);
            totalIntensity += (intensity * XYDist * XYDist);
            neighbors++;
        }
        if (neighbors > 0)
            totalIntensity /= neighbors;
        else
            return;


        float savedIntensity = totalIntensity / (distance * distance);
        if (savedIntensity < Parameters.ABSOLUTE_MINIMUM_SENSE)
            return;

        if (senseSources.ContainsKey(source))
        {
            totalSenseLevels[Sense.GetSense(source)] += (savedIntensity - senseSources[source]); // Increase total by the difference between what this source was before and what it is now.
            senseSources[source] = savedIntensity;
        }
        else
        {
            senseSources[source] = savedIntensity;
            totalSenseLevels[Sense.GetSense(source)] = savedIntensity;
        }

        float passedIntensity = savedIntensity;

        if (senseAbsorption.ContainsKey(Sense.GetSense(source)))
        {
            passedIntensity = (passedIntensity * senseAbsorption[Sense.GetSense(source)]);
        }

        // Cardinals continue to move along only the cardinals.
        if (source.Location.x == Location.x)
        {
            int yDir = ((Location.y - source.Location.y) < 0 ? -1 : 1);

            map.GetLocation(new Vector2(Location.x, Location.y + yDir)).ExpandSenseSource(source, passedIntensity, this);
        }
        else if (source.Location.y == Location.y)
        {
            int xDir = ((Location.x - source.Location.x) < 0 ? -1 : 1);

            map.GetLocation(new Vector2(Location.x + xDir, Location.y)).ExpandSenseSource(source, passedIntensity, this);
        }
        else
        {
            int xDir = ((Location.x - source.Location.x) < 0 ? -1 : 1);
            int yDir = ((Location.y - source.Location.y) < 0 ? -1 : 1);

            map.GetLocation(new Vector2(Location.x + xDir, Location.y))?.ExpandSenseSource(source, passedIntensity, this);
            map.GetLocation(new Vector2(Location.x + xDir, Location.y + yDir))?.ExpandSenseSource(source, passedIntensity, this);
            map.GetLocation(new Vector2(Location.x, Location.y + yDir))?.ExpandSenseSource(source, passedIntensity, this);
        }
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
}

