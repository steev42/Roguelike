using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    public float lightIntensity;
   
    private List<Vector2> previouslyLitTiles;

    void Start()
    {
        previouslyLitTiles = new List<Vector2>();
    }

    void Update()
    {
        UpdateLightingLevels();
    }

    private void UpdateLightingLevels()
    {
        // Division by 4 doubles the range; this allows for lighting overlaps.
        float minLightToConsider = GameData.GetActiveCharacter().minimumLightToSee / 4; 
        int maxRange = (int)(Mathf.Sqrt(lightIntensity / minLightToConsider));
        List<Vector2> currentlyLitTiles = new List<Vector2>();

        // X squared only; y doesn't exist yet, and we'll continue along the y=0 axis the whole way out 
        // Max of 1 or x squared prevents divide by 0 errors at the light origin; origin is always considered
        // to have a range of 1 for our purposes (not diminishing the light)
        for (int x = -maxRange; x <= maxRange; x++)
        {
            for (int y = -maxRange; y <= maxRange; y++)
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
            }
        }

        foreach (Vector2 v in previouslyLitTiles)
        {
            TileData td = GameData.GetTile(v);
            if (td != null)
            {
                td.SetLightLevel(this, 0);
            }
        }

        previouslyLitTiles = currentlyLitTiles;
    }


}
