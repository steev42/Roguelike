using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    // TODO Split renderer from data?
    public float movementSpeedMultiplier = 1.0f;
	
    public Color originalColor;
    private Dictionary<LightSource, float> lightLevel;
    private SpriteRenderer mySpriteRenderer;

    public float totalLight;

    private bool needsUpdate = true;
    private List<IPhysicalObject> tileContents;

    void Start()
    {
        lightLevel = new Dictionary<LightSource, float>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        tileContents = new List<IPhysicalObject>();
    }

    public bool isTileLockedTo(IPhysicalObject o)
    {
        if (tileContents != null)
        {
            foreach (IPhysicalObject content in tileContents)
            {
                Debug.Log("Examining " + content.ToString());
                if (content.isLockedTo(o))
                {
                    Debug.Log("Tile locked due to presence of " + content.ToString());
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
        if (tileContents != null)
        {
            Debug.Log("Tile contains " + tileContents.Count + " objects.");
            foreach (IPhysicalObject content in tileContents)
            {
                Debug.Log("Looking at " + content.ToString());
                if (content.isLockedTo(o))
                {
                    Debug.Log("Tile is occupied.");
                    return false;
                }
            }

            tileContents.Add(o);
            Debug.Log("Successfully joined tile. Now contains " + tileContents.Count);
            return true;
        }

        Debug.Log("tileContents are null");
        return false;
    }

    public void LeaveTile(IPhysicalObject o)
    {
        Debug.Log("Calling LeaveTile");
        if (tileContents != null)
            tileContents.Remove(o);
    }

    public bool ContainsAttackableObject()
    {
        foreach (IPhysicalObject content in tileContents)
        {
            if (content is IAttackableObject)
            {
                Debug.Log("Found something to attack.");
                return true;
            }
            Debug.Log(content.ToString() + " isn't able to be attacked.");
        }
        return false;
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
        if (this.name == "Tile_12_12" && Time.deltaTime % 100 == 0)
            Debug.Log(tileContents.Count);
        
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
            float minLight = GameData.GetActiveCharacter().GetAttribute(CharacterAttributes.MIN_LIGHT_FOR_SIGHT);
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

}
