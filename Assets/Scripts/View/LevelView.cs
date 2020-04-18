using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LevelView : RogueElement
{
    public Sprite wallSprite;
    public Sprite floorSprite;

    private AbstractMapLevel level;
    public AbstractMapLevel Map { set { level = value; } }

    private Dictionary<Vector2, bool> visibilityUpdates;
    private Dictionary<Vector2, GameObject> displayedTiles;

    void Start()
    {
        visibilityUpdates = new Dictionary<Vector2, bool>();
        displayedTiles = new Dictionary<Vector2, GameObject>();
    }

    void Update()
    {
        if (level != null)
        {
            foreach(Vector2 loc in visibilityUpdates.Keys)
            {
                if (level.mapData.ContainsKey(loc) == false)
                { 
                    // TODO Log an error?
                    continue;
                }
                Sprite s = floorSprite;

                if (true == visibilityUpdates[loc])
                {
                    List<IPhysicalObject> objs = level.mapData[loc].GetContents();
                    foreach (IPhysicalObject obj in objs)
                    {
                        if (obj is WallObject)
                        {
                            s = wallSprite;
                            break; // This is a wall; don't need to check more contents (for now)
                        }
                    }

                    if (displayedTiles.ContainsKey(loc))
                    {
                        continue;
                        /*GameObject tile_go = displayedTiles[loc];
                        tile_go.transform.position = loc;
                        SpriteRenderer sr = tile_go.GetComponent<SpriteRenderer>();
                        sr.color = Color.gray;
                        sr.sprite = s;*/
                    }
                    else
                    {
                        GameObject tile_go = new GameObject();
                        tile_go.name = "Tile_" + loc.x + "_" + loc.y;
                        tile_go.transform.position = loc;
                        tile_go.transform.SetParent(this.transform);
                        SpriteRenderer sr = tile_go.AddComponent<SpriteRenderer>();
                        sr.color = Color.gray;
                        sr.sprite = s;
                        displayedTiles[loc] = tile_go;
                    }                    
                }
                else
                {
                    if (displayedTiles.ContainsKey(loc))
                    {
                        GameObject obj = displayedTiles[loc];
                        displayedTiles.Remove(loc);
                        Destroy(displayedTiles[loc]);
                    }
                }
            }
            visibilityUpdates.Clear();
        }
        else
        {
            // TODO Log an error?
            visibilityUpdates.Clear();
        }
    }

    public void UpdateVisibility(Vector2 loc, bool isVisible)
    {
        visibilityUpdates[loc] = isVisible;
    }


}
