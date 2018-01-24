using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDisplay : MonoBehaviour
{

    public Vector2 OffsetFromOrigin;

    private Vector2 storedGameOffset;
    private TileData data;

    private SpriteRenderer mySpriteRenderer;
    public Sprite floor_sprite;
    public Sprite wall_sprite;

    // Use this for initialization
    void Start()
    {
        storedGameOffset = GameData.mapOffset;
        data = GameData.GetTile(storedGameOffset + OffsetFromOrigin);
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
	
    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Looking for data in tile " + (storedGameOffset + OffsetFromOrigin));

        if (GameData.mapOffset != storedGameOffset)
        {
            // mapOffset has changed!
            storedGameOffset = GameData.mapOffset;
        }

        data = GameData.GetTile(storedGameOffset + OffsetFromOrigin);

        if (mySpriteRenderer == null || data == null)
            return;
        
        //TODO Update this with a sprite manager.
        if (data.isWall())
        {
            mySpriteRenderer.sprite = wall_sprite;
        }
        else
        {
            mySpriteRenderer.sprite = floor_sprite;
        }


        float minLight = GameData.GetActiveCharacter().minimumLightToSee;
        float maxLight = GameData.DEFAULT_LIGHT;

        float total = data.GetTotalLightLevel();

        float pct = Mathf.Min(maxLight, total) - minLight / (maxLight - minLight);

        if (total == 0)
        {
            mySpriteRenderer.color = GameData.COLOR_NO_LIGHT;
        }
        else if (total > minLight)
        {
            mySpriteRenderer.color = data.originalColor;
        }
        else
        {
            mySpriteRenderer.color = Color.Lerp(GameData.COLOR_NO_LIGHT, data.originalColor, pct);
        }
    }
}
