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
    public float totalLight;

    private bool needsUpdate = true;

    void Start()
    {
        lightLevel = new Dictionary<LightSource, float>();
    }

    public float GetLightLevel(LightSource l)
    {
        if (lightLevel != null && lightLevel.ContainsKey(l))
        {
            return lightLevel[l];
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


    void Update()
    {
        if (mySpriteRenderer == null)
            return;

        if (needsUpdate == false)
            return;

        CharacterData currentChar = GameData.GetActiveCharacter();
        if (GameData.InLineOfSight(currentChar.location, this.transform.position) == false)  // TODO NOT transform.position anymore...right?
        {
            mySpriteRenderer.color = Color.black;
        }
        else  // In line of sight.  Update based on lighting.
        {
            float minLight = GameData.GetActiveCharacter().minimumLightToSee;
            float maxLight = GameData.DEFAULT_LIGHT;

            float total = GetTotalLightLevel();
            totalLight = total;

            float pct = Mathf.Min(maxLight, total) - minLight / (maxLight - minLight);


            if (total == 0)
            {
                mySpriteRenderer.color = Color.black;
            }
            else if (total > minLight)
            {
                mySpriteRenderer.color = originalColor;
            }
            else
            {
                mySpriteRenderer.color = new Color(Mathf.Lerp(0, originalColor.r, pct), Mathf.Lerp(0, originalColor.g, pct), Mathf.Lerp(0, originalColor.b, pct));
            }
        }

        needsUpdate = false;
    }

    public void SetLightLevel(LightSource source, float light)
    {
        if (lightLevel == null)
        {
            lightLevel = new Dictionary<LightSource, float>();
        }
        lightLevel[source] = light;

        needsUpdate = true;
    }

    public bool isWall()
    {
        return (movementSpeedMultiplier == 0f);
    }

}
