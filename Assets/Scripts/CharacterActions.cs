using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActions
{

	public static void MoveCharacter (CharacterData actingCharacter, Vector2 endLocation)
	{
		if (GameData.GetTile ((int)endLocation.x, (int)endLocation.y).movementSpeedMultiplier != 0) {
			actingCharacter.UpdateLocation (endLocation);
			//actingCharacter.obj.transform.position = endLocation;
		}
	}

}
