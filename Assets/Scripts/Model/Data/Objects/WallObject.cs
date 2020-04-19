using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WallObject : IPhysicalObject
{
    private DirectionEnum orientation;
    private string style;       // Natural, Worked, etc.  Eventually probably not a string.
    private string material;    // Wood, Stone, Marble, etc.  Eventually probably not a string.
    List<IAttachable> attachedItems;
    Vector2 location;
    LocationData ld;

    public WallObject(DirectionEnum _orientation)
    {
        orientation = _orientation;
        style = "Natural"; // TODO Update
        material = "Stone"; // TODO Update
        attachedItems = new List<IAttachable>();
    }

    public Vector2 Location { get { return location; }  }
    public LocationData LocationData
    {
        get { return ld; }
        set
        {
            ld = value;
            location = ld.Location;
        }
    }
    public List<IAttachable> AttachedItems { get { return attachedItems; } }

    public void Attach(IAttachable a)
    {
        if (attachedItems?.Contains(a) == false)
            attachedItems?.Add(a);
    }

    public float GetAttribute(string s)
    {
        throw new AttributeNotFoundException("Walls do not have attributes");
    }

    public int GetAttributeInteger(string s)
    {
        throw new AttributeNotFoundException("Walls do not have attributes");
    }

    public bool isDetectableByCharacter(CharacterData cd, SenseEnum sense = SenseEnum.VISION)
    {
        float visibility = LocationData.GetTotalSenseLevel(sense);
        if (visibility > 0)
            return true;
        return false;
        //throw new System.NotImplementedException();
    }

    public bool isLockedTo(IPhysicalObject o)
    {
        if (orientation == DirectionEnum.FULL_TILE)
            return true;
        else
            return false;
    }
}