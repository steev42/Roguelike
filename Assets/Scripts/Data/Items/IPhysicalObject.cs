using UnityEngine;
using System.Collections;

public interface IPhysicalObject
{
    bool isLockedTo(IPhysicalObject o);
    float GetAttribute(string s);
    int GetAttributeInteger(string s);
}
