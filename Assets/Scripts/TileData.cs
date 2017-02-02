using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public float movementSpeedMultiplier = 1.0f;
	
    public Color originalColor;
    private Dictionary<LightSource, float> lightLevel;
    private SpriteRenderer mySpriteRenderer;

    void Start()
    {
        lightLevel = new Dictionary<LightSource, float>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
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

        float minLight = GameData.GetActiveCharacter().minimumLightToSee;
        float maxLight = GameData.DEFAULT_LIGHT;

        float total = GetTotalLightLevel();

        float pct = Mathf.Min(maxLight, total) - minLight / (maxLight - minLight);


        if (total == 0)
        {
            mySpriteRenderer.color = Color.red;
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

    public void SetLightLevel(LightSource source, float light)
    {
        if (lightLevel == null)
        {
            lightLevel = new Dictionary<LightSource, float>();
        }
        lightLevel[source] = light;
    }

}
