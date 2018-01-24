using UnityEngine;
using System.Collections;

public interface IAttackableObject : IPhysicalObject
{
    float GetAttribute(string s, bool defense);
    int GetAttributeInteger(string s, bool defense);
}
