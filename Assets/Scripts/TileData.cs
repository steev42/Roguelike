using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public float movementSpeedMultiplier;
	
    public Color originalColor;
    private Dictionary<LightSource, float> lightLevel;
    private SpriteRenderer mySpriteRenderer;

    void Start()
    {
        lightLevel = new Dictionary<LightSource, float>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (mySpriteRenderer == null)
            return;

        float minLight = GameData.GetActiveCharacter().minimumLightToSee;
        float maxLight = GameData.DEFAULT_LIGHT;
        float pct = Mathf.Min(maxLight, lightLevel) - minLight / (maxLight - minLight);

        if (lightLevel == 0)
        {
            mySpriteRenderer.color = Color.black;
        }
        else if (lightLevel > minLight)
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
        lightLevel[source] = light;
    }

}
