using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : IAttackableObject
{
    public Vector2 location;

    public Queue<ICharacterAction> queuedActions;
    public Vector2 queuedLocation;

    public AI character_ai;
    private CharacterAttributes attributes;

    public CharacterData(Vector2 loc)
    {
        location = loc;
        queuedActions = new Queue<ICharacterAction>();
        attributes = CharacterAttributes.PCAttributes();
    }

    public void UpdateLocation(Vector2 loc)
    {
        if (GameData.GetTile(loc).GetComponent<TileData>().JoinTile(this))
        {
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
