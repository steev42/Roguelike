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
                GameObject tile_go = new GameObject();
                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector2(x, y);
                tile_go.transform.SetParent(this.transform);
                SpriteRenderer sr = tile_go.AddComponent<SpriteRenderer>();
                sr.color = Color.gray;
                //TileDataOriginal td = tile_go.AddComponent<TileDataOriginal>();
                //td.originalColor = Color.gray;
            
                // TileData td = new TileData(tile_go);

                if (pixels[(y * mapImage.width) + x].Equals(new Color32(0, 0, 0, 255)))
                {
                    sr.sprite = wall_sprite;
                    //td.movementSpeedMultiplier = 0.0f;
                    //tile_go.AddComponent<Rigidbody2D> ();
                    tile_go.AddComponent<BoxCollider2D>();
                }
                else
                {
                    sr.sprite = floor_sprite;
                }

                /*LightedLocation ll = tile_go.AddComponent<LightedLocation>();
                ll.originalColor = Color.gray;*/
                //GameData.SetTile(x, y, tile_go);
            }
        }
    }
	
}
