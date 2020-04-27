﻿using UnityEngine;
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
       // Debug.Log("Called JoinTile");
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
            o.LocationData = this;
            SendUpdateNotification();
           // Debug.Log("Successfully joined tile. Now contains " + physicalObjects.Count);
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
            //Debug.Log(content.ToString() + " isn't able to be attacked.");
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

    public float GetPassThruSenseLevel(SenseEnum sense, ISenseProperty source = null)
    {
        float absorption = 0f;
        if (senseAbsorption.ContainsKey(sense))
            absorption = senseAbsorption[sense];

        if (source == null)
        {
            // Not looking for specific source.  Give total.
            if (totalSenseLevels.ContainsKey(sense))
                return (1 - absorption) * totalSenseLevels[sense];
            else
                return 0f;
        }
        else
        {
            if (senseSources.ContainsKey(source))
                return (1 - absorption) * totalSenseLevels[sense];
            else
                return 0f;
        }
    }

    public void UpdateOffset(Vector2 loc, ISenseProperty source, float intensity)
    {
        //Debug.Log("Spreading light from " + source.Location.ToString() + "; checking " + loc.ToString());
        float n1 = 0f;
        float n2 = 0f;
        float n3 = 0f;
        int foundNeighbors = 0;
        LocationData ld = map.GetLocation(loc);
        if (ld == null)
            return;
        int xDiff1 = 0;
        int yDiff1 = 0;
        int xDiff2 = 0;
        int yDiff2 = 0;
        int xDiff3 = 0;
        int yDiff3 = 0;

        if (loc.x == source.Location.x)
        {
            yDiff1=yDiff2=yDiff3 = (loc.y > source.Location.y ? -1 : 1);
            xDiff1 = -1;
            xDiff3 = 1;
        }
        else if (loc.y == source.Location.y)
        {
            xDiff1 = xDiff2 = xDiff3 = (loc.x > source.Location.x ? -1 : 1);
            yDiff1 = -1;
            yDiff3 = 1;
        }
        else
        {
            xDiff1 = xDiff2 = (loc.x > source.Location.x ? -1 : 1);
            yDiff2 = yDiff3 = (loc.y > source.Location.y ? -1 : 1);
        }

        if (map.GetLocation(loc + new Vector2(xDiff1, yDiff1)) != null)
        {
            n1 = map.GetLocation(loc + new Vector2(xDiff1, yDiff1)).GetPassThruSenseLevel(source.Sense, source);
            //Debug.Log("Neighbor 1 = " + (loc + new Vector2(xDiff1, yDiff1)).ToString());
            foundNeighbors++;
        }
        if (map.GetLocation(loc + new Vector2(xDiff2, yDiff2)) != null)
        {
            //Debug.Log("Neighbor 2 = " + (loc + new Vector2(xDiff2, yDiff2)).ToString());
            n2 = map.GetLocation(loc + new Vector2(xDiff2, yDiff2)).GetPassThruSenseLevel(source.Sense, source);
            foundNeighbors++;
        }
        if (map.GetLocation(loc + new Vector2(xDiff3, yDiff3)) != null)
        {
            //Debug.Log("Neighbor 3 = " + (loc + new Vector2(xDiff3, yDiff3)).ToString());
            n3 = map.GetLocation(loc + new Vector2(xDiff3, yDiff3)).GetPassThruSenseLevel(source.Sense, source);
            foundNeighbors++;
        }
        float dist = Vector2.Distance(loc, source.Location);
        //Debug.Log("Distance = " + dist);
        //Debug.Log("Setting intensity to " + (((n1 + n2 + n3) / foundNeighbors) / (dist * dist)));
        ld.AddSenseSource(source, ((n1 + n2 + n3) / foundNeighbors) / (dist * dist), false);
    }



    public void AddSenseSource(ISenseProperty source, float intensity, bool isLocationOfSource = false)
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
        //Debug.Log("Passed Intensity: " + passedIntensity);
        if (isLocationOfSource == false)
            return;

        //Debug.Log("Spreading Light one step");
        
        map.GetLocation(new Vector2(Location.x - 1, Location.y))?.AddSenseSource(source, passedIntensity);
        map.GetLocation(new Vector2(Location.x + 1, Location.y))?.AddSenseSource(source, passedIntensity);
        map.GetLocation(new Vector2(Location.x, Location.y - 1))?.AddSenseSource(source, passedIntensity);
        map.GetLocation(new Vector2(Location.x, Location.y + 1))?.AddSenseSource(source, passedIntensity);
        
        map.GetLocation(new Vector2(Location.x - 1, Location.y - 1))?.AddSenseSource(source, passedIntensity / Mathf.Sqrt(2));
        map.GetLocation(new Vector2(Location.x + 1, Location.y - 1))?.AddSenseSource(source, passedIntensity / Mathf.Sqrt(2));
        map.GetLocation(new Vector2(Location.x + 1, Location.y + 1))?.AddSenseSource(source, passedIntensity / Mathf.Sqrt(2));
        map.GetLocation(new Vector2(Location.x - 1, Location.y + 1))?.AddSenseSource(source, passedIntensity / Mathf.Sqrt(2));
        
        float maxDistance = Mathf.Sqrt(intensity / Parameters.ABSOLUTE_MINIMUM_SENSE);
        //Debug.Log("Max Distance = " + maxDistance);
        for (int i = 2; i <= maxDistance; i++)
        {
            
            // Do cardinals; +/- i for X and Y respectively...
            UpdateOffset(new Vector2(Location.x - i, Location.y), source, intensity);
            UpdateOffset(new Vector2(Location.x + i, Location.y), source, intensity);
            UpdateOffset(new Vector2(Location.x, Location.y + i), source, intensity);
            UpdateOffset(new Vector2(Location.x, Location.y - i), source, intensity);

            for (int j = 1; j <= i; j++)
            {
                // Then spread +/- away on both X and Y axis until you get to the corners.  ONLY do the corners once.
                if (i==j) // Avoid double-hitting the corners of the square
                {
                    UpdateOffset(new Vector2(Location.x - j, Location.y - j), source, intensity);
                    UpdateOffset(new Vector2(Location.x - j, Location.y + j), source, intensity);
                    UpdateOffset(new Vector2(Location.x + j, Location.y - j), source, intensity);
                    UpdateOffset(new Vector2(Location.x + j, Location.y + j), source, intensity);
                }
                else
                {
                    UpdateOffset(new Vector2(Location.x + j, Location.y + i), source, intensity);
                    UpdateOffset(new Vector2(Location.x - j, Location.y + i), source, intensity);
                    UpdateOffset(new Vector2(Location.x + j, Location.y - i), source, intensity);
                    UpdateOffset(new Vector2(Location.x - j, Location.y - i), source, intensity);
                    UpdateOffset(new Vector2(Location.x + i, Location.y + j), source, intensity);
                    UpdateOffset(new Vector2(Location.x - i, Location.y + j), source, intensity);
                    UpdateOffset(new Vector2(Location.x + i, Location.y - j), source, intensity);
                    UpdateOffset(new Vector2(Location.x - i, Location.y - j), source, intensity);
                }
            }
            
        }
        source.UpdatedAt = source.Location;
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
