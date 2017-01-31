using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightedLocation : MonoBehaviour
{
    public Color originalColor;
    public float lightLevel;

    private CharacterData storedCharacter = null;
    private Vector2 storedLocation = new Vector2(-1, -1);

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CharacterData activeCharacter = GameData.GetActiveCharacter();
        if (activeCharacter.Equals(storedCharacter) && activeCharacter.location == storedLocation)
        {
            // No change since last update.  No need to redraw the map.
            return;
        }

        storedLocation = activeCharacter.location;
        storedCharacter = activeCharacter;

        lightLevel = 0f;


        // Does the active character have line of sight to this location.  If not, let's not check the lights.
        if (CanSeeLight(activeCharacter.location, transform.position))
        {
            //Debug.Log("Checking " + GameData.GetLightSources().Count + " light sources.");
            foreach (LightSource l in GameData.GetLightSources())
            {
                float distance = Vector2.Distance((Vector2)transform.position, l.location);

                //distance += 1; // This should make it so that the light range (at canSeeLight=1.0) is equal to the square root of the intensity.
                if (CanSeeLight(transform.position, l.location))
                {

                    if (distance == 0)
                    {
                        lightLevel += l.lightIntensity;
                    }
                    else
                    {
                        lightLevel += l.lightIntensity / (distance * distance);
                    }
                }
            }
        }

        float minLight = GameData.GetActiveCharacter().minimumLightToSee;
        float maxLight = GameData.DEFAULT_LIGHT;
        float pct = Mathf.Min(maxLight, lightLevel) - minLight / (maxLight - minLight);

        if (lightLevel == 0)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }
        else if (lightLevel > minLight)
        {
            GetComponent<SpriteRenderer>().color = originalColor;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(Mathf.Lerp(0, originalColor.r, pct), Mathf.Lerp(0, originalColor.g, pct), Mathf.Lerp(0, originalColor.b, pct));
        }
    }

    bool CanSeeLight(Vector2 myLocation, Vector2 lightLocation, float squareSize = 1.0f)
    {
        float half = (squareSize / 2) - .001f;
        // Make corners just off on the two corners (in different directions) to prevent straight corner-to-corner pass throughs not colliding.
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

                RaycastHit2D hit = Physics2D.Linecast(myLocation + Corners[i], lightLocation + Corners[j]);
                if (hit.collider == null)
                {
                    return true;
                }
                else
                {
                    Vector2 hitpos = (Vector2)hit.collider.transform.position;
                    if (hitpos.Equals(myLocation) || hitpos.Equals(lightLocation))
                    {
                        return true;
                    }
                }
            }

        }
        return false;
    }

}
