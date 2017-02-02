using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public const float DEFAULT_LIGHT = 4.0f;

    static Dictionary <Vector2, GameObject> tileMap;

    static Dictionary <CharacterData, GameObject> characterObjectMap;

    // static List<LightSource> lightSources;

    static CharacterData activeCharacter;

    public static bool isValidMove(Vector2 target)
    {
        GameObject obj = GetTile(target);
        if (obj == null)
        {
            return false;
        }
        TileData tile = obj.GetComponent<TileData>();
        if (tile != null && tile.movementSpeedMultiplier != 0)
        {
            return true;
        }
        //TODO Other conditions, like another creature in this tile?
        return false;
    }

    /*  public static void AddLightSource(LightSource l)
    {
        if (lightSources == null)
        {
            lightSources = new List<LightSource>();
        }

        lightSources.Add(l);
    }

    public static List<LightSource> GetLightSources()
    {
        if (lightSources == null)
        {
            lightSources = new List<LightSource>();
        }
        return lightSources;
    }*/

    public static void SetTile(int x, int y, GameObject td)
    {
        if (tileMap == null)
        {
            tileMap = new Dictionary<Vector2, GameObject>();
        }

        tileMap[new Vector2(x, y)] = td;
    }

    public static GameObject GetTile(Vector2 tileCoord)
    {
        if (tileMap != null && tileMap.ContainsKey(tileCoord))
        {
            return tileMap[tileCoord];
        }
        else
        {
            return null;
        }
    }

    public static GameObject GetTile(int x, int y)
    {
        Vector2 pos = new Vector2(x, y);
        return GetTile(pos);
    }

    public static void SetActiveCharacter(CharacterData activate)
    {
        activeCharacter = activate;
    }

    public static CharacterData GetActiveCharacter()
    {
        return activeCharacter;
    }

    public static GameObject GetActiveCharacterObject()
    {
        if (characterObjectMap == null || characterObjectMap.ContainsKey(activeCharacter) == false)
        {
            Debug.LogWarning("Requesting Character's GameObject when one doesn't exist.");
            return null;
        }
        return characterObjectMap[activeCharacter];

    }

    public static void MapCharacterToObject(CharacterData cd, GameObject go)
    {
        if (characterObjectMap == null)
        {
            characterObjectMap = new Dictionary<CharacterData,GameObject>();
        }
        characterObjectMap[cd] = go;
    }

    public static bool InLineOfSight(Vector2 fromLocation, Vector2 toLocation, int maxVisionRange = int.MaxValue, float squareSize = 1.0f)
    {
        if (Vector2.Distance(fromLocation, toLocation) > maxVisionRange)
            return false;

        // Go just inside squares.  This prevents corner-to-corner shenanigans.
        float half = (squareSize / 2) - .001f;
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

                RaycastHit2D hit = Physics2D.Linecast(fromLocation + Corners[i], toLocation + Corners[j]);
                if (hit.collider == null)
                {
                    return true;
                }
                else
                {
                    Vector2 hitpos = (Vector2)hit.collider.transform.position;
                    if (hitpos.Equals(fromLocation) || hitpos.Equals(toLocation))
                    {
                        return true;
                    }
                }
            }

        }
        return false;
    }


}
