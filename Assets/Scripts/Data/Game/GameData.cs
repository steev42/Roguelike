using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public const float DEFAULT_LIGHT = 4.0f;

    static Dictionary <Vector2, GameObject> tileMap;

    static Dictionary <CharacterData, GameObject> characterObjectMap;

    static bool inMoveState = true;

    public static void toggleMove()
    {
        inMoveState = true;
    }

    public static void toggleAttack()
    {
        inMoveState = false;
    }

    public static bool isInAttackState()
    {
        return !inMoveState;
    }

    // static List<LightSource> lightSources;

    static CharacterData activeCharacter;

    public static bool isValidMove(Vector2 target)
    {
        if (inMoveState == false)
            return false;

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

    public static bool isValidAttack(Vector2 target)
    {
        if (inMoveState == true)
            return false;

        GameObject obj = GetTile(target);
        if (obj == null)
        {
            return false;
        }

        TileData tile = obj.GetComponent<TileData>();
        //TODO Check to see if there's a valid target
        return true;
    }


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
        {
            return false;
        }

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
                Vector2 start = fromLocation + Corners[i];
                Vector2 end = toLocation + Corners[j];
                RaycastHit2D hit = Physics2D.Linecast(start, end);
                if (hit.collider == null)
                {
                    //  Debug.Log("From " + start + " to " + end + " hits nothing.");
                    return true;
                }
                else
                {
                    Vector2 hitpos = (Vector2)hit.collider.transform.position;
                    if (hitpos.Equals(fromLocation) || hitpos.Equals(toLocation))
                    {
                        //         Debug.Log("From " + start + " to " + end + " hits a wall, but one of those IS a wall.");
                        return true;
                    }
                    //      Debug.Log("From " + start.ToString() + " to " + end.ToString() + " hits a wall.");
                }
            }

        }
        return false;
    }


}
