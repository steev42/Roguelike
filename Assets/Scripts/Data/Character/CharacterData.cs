using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : IAttackableObject
{
    public Vector2 location;

    public Vector2 queuedLocation;

    public AI character_ai;
    private CharacterAttributes attributes;

    public CharacterData(Vector2 loc)
    {
       // location = loc;
        attributes = CharacterAttributes.PCAttributes();
        location = loc;
    }

    public void UpdateLocation(Vector2 loc)
    {

        if (GameData.GetTile(loc) == null || GameData.GetTile(loc).GetComponent<TileData>() == null)
        {
            Debug.LogWarning("Updating location to null tile?");
            return;
        }   

        if (GameData.GetTile(loc).GetComponent<TileData>().JoinTile(this))
        {
            GameData.GetTile(location).GetComponent<TileData>().LeaveTile(this);
            location = loc;

        }
        else
        {
            Debug.LogWarning("Unable to move into " + loc + ".  Tile locked.");
        }
        //temp.location = loc;
    }

    public float GetAttribute(string s)
    {
        return attributes.GetAttribute(s);
    }

    public int GetAttributeAsInteger (string s)
    {
        return attributes.GetAttributeInteger(s);
    }

    public bool isLockedTo(IPhysicalObject o)
    {
        if (o is CharacterData)
        {
            return true; // Only one character can be in a tile at a time.
        }
        return false; // But they don't lock anything else right now.
    }
}
