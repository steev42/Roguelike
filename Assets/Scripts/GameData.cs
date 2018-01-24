using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public const float DEFAULT_LIGHT = 4.0f;
    public static Color COLOR_NO_LIGHT = Color.red;

    static Dictionary <Vector2, TileData> tileMap;

    static Dictionary <CharacterData, GameObject> characterObjectMap;

    static CharacterData activeCharacter;

    public static Vector2 mapOffset;

    public static bool isValidMove(Vector2 target)
    {
        //TODO This method should be in the MoveAction script instead.
        TileData tile = GetTile(target);
/*        if (obj == null)
        {
            return false;
        }
        TileData tile = obj.GetComponent<TileData>();*/
        if (tile != null && tile.movementSpeedMultiplier != 0)
        {
            return true;
        }
        //TODO Other conditions, like another creature in this tile?
        return false;
    }

    public static void SetTile(int x, int y, TileData td)
    {
        if (tileMap == null)
        {
            tileMap = new Dictionary<Vector2, TileData>();
        }

        tileMap[new Vector2(x, y)] = td;
    }

    public static TileData GetTile(Vector2 tileCoord)
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

    public static TileData GetTile(int x, int y)
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

    public static void CenterViewOn(Vector2 centerOfScreen)
    {
        
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

        //Debug.Log("Checking line of sight between " + fromLocation + " to " + toLocation);
        //TODO Update so this doesn't use linecast, which won't work now.

        if (Vector2.Distance(fromLocation, toLocation) > maxVisionRange)
        {
            return false;
        }

        WuLineAlgorithm line = new WuLineAlgorithm(fromLocation, toLocation);
        float totalPassThrough = 0f;
        int countedTiles = 0;
        Dictionary<Vector2, float> linePoints = line.GetLine();
        foreach (Vector2 pos in linePoints.Keys)
        {
            TileData td = GetTile(pos);
            if (td != null)
            {
                totalPassThrough += td.opacity * (1 - linePoints[pos]);
                countedTiles++;
            }
        }

        totalPassThrough /= countedTiles; // Get Average Pass Through?
        if (totalPassThrough >= .01)
            return true;

        /* // Go just inside squares.  This prevents corner-to-corner shenanigans.
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
*/
        return false;
    }


}
