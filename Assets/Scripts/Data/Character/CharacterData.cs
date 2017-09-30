using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
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
        location = loc;
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
}
