using UnityEngine;
using System.Collections;

public interface IPhysicalObject
{
    bool isLockedTo(IPhysicalObject o);
}
