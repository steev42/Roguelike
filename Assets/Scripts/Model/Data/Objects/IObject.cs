
using UnityEngine;
using System.Collections.Generic;

public interface IObject
{
    Vector2 Location { get; }
    LocationData LocationData {get; set;}
    void Attach(IAttachable a);

    List<IAttachable> AttachedItems { get; }
    
    bool isDetectableByCharacter(CharacterData cd, SenseEnum sense = SenseEnum.VISION);
}