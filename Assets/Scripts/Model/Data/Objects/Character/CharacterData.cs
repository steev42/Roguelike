using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : IAttackableObject
{
    public Vector2 location;

    public Vector2 queuedLocation;

    public AI character_ai;
    private CharacterAttributes attributes;
    private string characterName;
    public string type;
    LocationData ld;
    private List<IAttachable> attachments;

    public string Name { get { return characterName; } set { characterName = value; } }

    public Vector2 Location { get { return location; } }

    public LocationData LocationData { get { return ld; } 
        set { 
            ld = value;
            location = ld.Location;
            } 
        }

    public List<IAttachable> AttachedItems { get { return attachments; } }

    public CharacterData()
    {
        attributes = CharacterAttributes.PCAttributes();
        type = "Player";
    }

    public CharacterData(LocationData loc)
    {
        LocationData = loc;
        loc.JoinTile(this);
        attributes = CharacterAttributes.PCAttributes();
        type = "Player";
        //UpdateLocation(loc);
        
    }

    public float GetAttribute(string s)
    {
        return attributes.GetAttribute(s);
    }

    public bool isLockedTo(IPhysicalObject o)
    {
        if (o is CharacterData)
        {
            return true; // Only one character can be in a tile at a time.
        }
        return false; // But they don't lock anything else right now.
    }

    public float GetAttribute(string s, bool defense)
    {
        if (defense == false)
            return GetAttribute(s);
        //TODO Figure out armor
        return GetAttribute(s);
    }

    public int GetAttributeInteger(string s, bool defense)
    {
        if (defense == false)
            return GetAttributeInteger(s);
        //TODO Figure out armor
        return GetAttributeInteger(s);
    }

    public int GetAttributeInteger(string s)
    {
        return attributes.GetAttributeInteger(s);
    }

    public void Attach(IAttachable a)
    {
        if (attachments == null)
            attachments = new List<IAttachable>();

        if (attachments.Contains(a))
            return;

        attachments.Add(a);
        a.Location = Location;
    }

    public bool isDetectableByCharacter(CharacterData cd, SenseEnum sense = SenseEnum.VISION)
    {
        float visibility = LocationData.GetTotalSenseLevel(sense);
        if (visibility > 0)
        {
            //Debug.Log(characterName + " is detectable");

            return true;
        }

        //Debug.Log(characterName + " is not detectable");

        return false;
    }
}
