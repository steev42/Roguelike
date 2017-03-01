using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCharacterSpriteLocation : MonoBehaviour
{
    public CharacterData cd;
		
    // Update is called once per frame
    void Update()
    {
        this.transform.position = cd.location + GameData.mapOffset;
    }
}
