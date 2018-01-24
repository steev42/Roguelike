using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    [SerializeField]
    private float lightIntensity;
   
    private List<Vector2> previouslyLitTiles;
    public Dictionary<Vector2, float> currentLighting = new Dictionary<Vector2, float>();
    private Vector3 lastPosition;

    private bool requiresUpdate = true;
    private bool firstDebugOfUpdate = true;
     
    void Start()
    {
        previouslyLitTiles = new List<Vector2>();
        lastPosition = this.transform.position;
    }

    void Update()
    {
        firstDebugOfUpdate = true;
        GameObject td_obj = GameData.GetTile(transform.position);
        float localLight = 0.0f;
        if (td_obj != null)
        {
            TileData td = td_obj.GetComponent<TileData>();
            localLight = td.GetLightLevel(this);
        }

        if (this.transform.position.Equals(lastPosition) == false || requiresUpdate || localLight != lightIntensity)
        {
            //  Debug.Log("Moved from " + lastPosition.ToString() + " to " + this.transform.position.ToString() + ".  Performing lighting update.");
            currentLighting = new Dictionary<Vector2, float>();
            UpdateLightingLevels();
            lastPosition = this.transform.position;

            requiresUpdate = false;
        }
    }

    public void UpdateIntensity(float intensity)
    {
        lightIntensity = intensity;
        requiresUpdate = true;
    }

    public void UpdateColor(Color c)
    {

    }

    private float GetDistanceFromSource(Vector2 pos)
    {
        return (pos-(Vector2)transform.position).magnitude;
    }

    private float GetDistanceFromSource(int x, int y)
    {
        Vector2 pos = new Vector2(x, y);
        return GetDistanceFromSource(pos);
    }

    private void UpdateLightingOnTileBasedOnNeighbors(int x, int y)
    {
        Vector2 pos = (Vector2)transform.position + new Vector2(x, y);

        // Get the 3 closer neighbors.  Should always and ONLY be 3.  If not...well, something is weird, but try anwyay.
        List<Vector2> closerSquares = new List<Vector2>();
        for (int _x = -1; _x <= 1; _x++)
        {
            for (int _y = -1; _y <= 1; _y++)
            {
                Vector2 neighbor = pos + new Vector2(_x, _y);

                if (GetDistanceFromSource(neighbor) < GetDistanceFromSource(pos))
                {
                    closerSquares.Add(neighbor);
                }
            }
        }

        float totalNeighborLight = 0.0f;
        foreach (Vector2 v in closerSquares)
        {
            Vector2 neighborFinalPosition = v;
            if (currentLighting.ContainsKey(neighborFinalPosition))
            {
                if (firstDebugOfUpdate) Debug.Log("Neighbor at " + v.ToString() + " has lighting of " + currentLighting[neighborFinalPosition] * GetDistanceFromSource(v) * GetDistanceFromSource(v));
                totalNeighborLight += (currentLighting[neighborFinalPosition] * GetDistanceFromSource(v) * GetDistanceFromSource (v));
            }
        }

        float averageLightIntensity = totalNeighborLight / closerSquares.Count;  //This is the effective light strength of the source, based on neighbors.
        UpdateLightingOnTile(pos - (Vector2)transform.position, averageLightIntensity / (GetDistanceFromSource(pos) * GetDistanceFromSource(pos)));

        firstDebugOfUpdate = false;

    }

    private void UpdateLightingOnTile(Vector2 pos, float intensity)
    {
        Vector2 lightPosition = this.transform.position;
        Vector2 checkPosition = lightPosition + pos;
        GameObject td_obj = GameData.GetTile(checkPosition);
        if (td_obj != null)
        {
            TileData td = td_obj.GetComponent<TileData>();
            td.SetLightLevel(this, intensity);
            currentLighting[checkPosition] = intensity;
        }
        previouslyLitTiles.Remove(checkPosition);
    }

    private void UpdateLightingOnTile(int x, int y, float intensity)
    {
        UpdateLightingOnTile(new Vector2(x, y), intensity);
    }

    private void UpdateLightingLevels()
    {
        // Division by 4 doubles the range; this allows for lighting overlaps.
        float minLightToConsider = GameData.GetActiveCharacter().minimumLightToSee / 4; 
        int maxRange = (int)(Mathf.Sqrt(lightIntensity / minLightToConsider));

        // Set inner 9 squares.
        for (int x = -1; x<= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2 pos = (Vector2)transform.position + new Vector2(x, y);
                // maxRange provides a second line of defense against spreading too far.
                // This mainly helps with diagonals, since we're looking at full range for
                // both x and y.
                if (GameData.InLineOfSight(transform.position, pos, maxRange))
                {
                    TileData td = GameData.GetTile(pos);
                    if (td != null)
                    {
                        td.SetLightLevel(this, lightIntensity / (Mathf.Max(1, (x * x) + (y * y))));
                        currentlyLitTiles.Add(pos);
                        if (previouslyLitTiles.Contains(pos))
                        {
                            previouslyLitTiles.Remove(pos);
                        }
                    }
                }
                if (x == 0 || y == 0)
                    UpdateLightingOnTile(x, y, lightIntensity);
                else
                    UpdateLightingOnTile(x, y, lightIntensity / 2); // (1*1+1*1)
            }
        }

        // Then extend outwards from this to as far as the light may reach.  Start with cardinal directions
        // (where x or y are the same as the light source), and move outwards from there.
        for (int i = 2; i < maxRange; i++)
        {
            // Cardinal extents of the current range
            UpdateLightingOnTile(i, 0, (lightIntensity / (i*i)));
            UpdateLightingOnTile(-i, 0, (lightIntensity / (i * i)));
            UpdateLightingOnTile(0, i, (lightIntensity / (i * i)));
            UpdateLightingOnTile(0, -i, (lightIntensity / (i * i)));

            // Do corners.
            float cornerDistSquared = ((i * i) + (i * i));
            UpdateLightingOnTile(i, i, (lightIntensity / cornerDistSquared));
            UpdateLightingOnTile(-i, i, (lightIntensity / cornerDistSquared));
            UpdateLightingOnTile(i, -i, (lightIntensity / cornerDistSquared));
            UpdateLightingOnTile(-i, -i, (lightIntensity / cornerDistSquared));

            // Move outward from the center.
            for (int a = 1; a < i; a++)
            {
                UpdateLightingOnTileBasedOnNeighbors(i, a);
                UpdateLightingOnTileBasedOnNeighbors(a, i);
                UpdateLightingOnTileBasedOnNeighbors(i, -a);
                UpdateLightingOnTileBasedOnNeighbors(-a, i);

                UpdateLightingOnTileBasedOnNeighbors(-i, a);
                UpdateLightingOnTileBasedOnNeighbors(a, -i);
                UpdateLightingOnTileBasedOnNeighbors(-i, -a);
                UpdateLightingOnTileBasedOnNeighbors(-a, -i);
            }


            
        }

        //// X squared only; y doesn't exist yet, and we'll continue along the y=0 axis the whole way out 
        //// Max of 1 or x squared prevents divide by 0 errors at the light origin; origin is always considered
        //// to have a range of 1 for our purposes (not diminishing the light)
        //for (int x = -maxRange; x <= maxRange; x++)
        //{
        //    for (int y = -maxRange; y <= maxRange; y++)
        //    {
        //        Vector2 pos = (Vector2)transform.position + new Vector2(x, y);
        //        // maxRange provides a second line of defense against spreading too far.
        //        // This mainly helps with diagonals, since we're looking at full range for
        //        // both x and y.
        //        if (GameData.InLineOfSight(transform.position, pos, maxRange))
        //        {
        //            GameObject td_obj = GameData.GetTile(pos);
        //            if (td_obj != null)
        //            {
        //                TileData td = td_obj.GetComponent<TileData>();
        //                td.SetLightLevel(this, lightIntensity / (Mathf.Max(1, (x * x) + (y * y))));
        //                currentlyLitTiles.Add(pos);
        //                if (previouslyLitTiles.Contains(pos))
        //                {
        //                    previouslyLitTiles.Remove(pos);
        //                }
        //            }
        //        }
        //    }
        //}

        foreach (Vector2 v in previouslyLitTiles)
        {
            TileData td = GameData.GetTile(v);
            if (td != null)
            Debug.Log("Clearing lighting from " + v + "because it is no longer lit.");
            GameObject td_obj = GameData.GetTile(v);
            if (td_obj != null)
            {
                td.SetLightLevel(this, 0);
            }
        }
        previouslyLitTiles.Clear();
        previouslyLitTiles.AddRange(currentLighting.Keys);
        Debug.Log("Currently lighting " + previouslyLitTiles.Count + " tiles.");
    }


}
