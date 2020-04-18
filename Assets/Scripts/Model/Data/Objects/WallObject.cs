using UnityEngine;
using UnityEditor;

public class WallObject : IPhysicalObject
{
    private DirectionEnum orientation;
    private string style;       // Natural, Worked, etc.  Eventually probably not a string.
    private string material;    // Wood, Stone, Marble, etc.  Eventually probably not a string.

    public WallObject(DirectionEnum _orientation)
    {
        orientation = _orientation;
        style = "Natural"; // TODO Update
        material = "Stone"; // TODO Update
    }

    public float GetAttribute(string s)
    {
        throw new AttributeNotFoundException("Walls do not have attributes");
    }

    public int GetAttributeInteger(string s)
    {
        throw new AttributeNotFoundException("Walls do not have attributes");
    }

    public bool isLockedTo(IPhysicalObject o)
    {
        if (orientation == DirectionEnum.FULL_TILE)
            return true;
        else
            return false;
    }
}