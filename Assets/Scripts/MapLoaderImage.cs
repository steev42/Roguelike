using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoaderImage : MonoBehaviour
{

    public Texture2D mapImage;
    public Sprite wall_sprite;
    public Sprite floor_sprite;

    // Use this for initialization
    void Start()
    {
        Color32[] pixels = mapImage.GetPixels32();

        for (int x = 0; x < mapImage.width; x++)
        {
            for (int y = 0; y < mapImage.height; y++)
            {
                TileData td = new TileData();

                if (pixels[(y * mapImage.width) + x].Equals(new Color32(0, 0, 0, 255)))
                {
                    td.movementSpeedMultiplier = 0.0f;
                    td.originalColor = Color.white;
                    td.opacity = 0.0f; // Wall...
                }
                else
                {
                    td.originalColor = Color.gray;                    
                }
                GameData.SetTile(x, y, td);
            }
        }
    }
	
}
