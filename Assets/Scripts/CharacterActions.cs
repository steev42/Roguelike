#if false
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActions
{

    public static void MoveCharacter(CharacterData actingCharacter, Vector2 endLocation)
    {
        GameObject tileObject = GameData.GetTile(endLocation);
        if (tileObject != null)
        {
            TileData td = tileObject.GetComponent<TileData>();
            if (td != null)
            {
                if (td.movementSpeedMultiplier != 0)
                {
                    actingCharacter.UpdateLocation(endLocation);
                }
            }
        }
    }

}
#endif