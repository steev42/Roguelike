using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public const float DEFAULT_LIGHT = 4.0f;

    static Dictionary <Vector2, TileData> tileMap;
    static Dictionary <CharacterData, GameObject> characterObjectMap;

    static List<LightSource> lightSources;

    static CharacterData activeCharacter;

    public static bool isValidMove(Vector2 target)
    {
        TileData tile = GetTile(target);
        if (tile != null && tile.movementSpeedMultiplier != 0)
        {
            return true;
        }
        //TODO Other conditions, like another creature in this tile?
        return false;
    }

    public static void AddLightSource(LightSource l)
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


}
