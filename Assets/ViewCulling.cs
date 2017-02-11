using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCulling : MonoBehaviour
{
    private CharacterData rememberedActiveCharacter;
    private Vector2 rememberedCharacterPosition;
    public int viewRange = 16;

    // Use this for initialization
    void Start()
    {
		
    }
	
    // Update is called once per frame
    void Update()
    {
        CharacterData currentCharacter = GameData.GetActiveCharacter();

        if (currentCharacter.Equals(rememberedActiveCharacter) && currentCharacter.location.Equals(rememberedCharacterPosition))
        {
            // Character and position are the same.  Do nothing.
            return;
        }

        rememberedActiveCharacter = currentCharacter;
        rememberedCharacterPosition = currentCharacter.location;

        GameData.CullDisplay(rememberedCharacterPosition, viewRange);
    }
}
