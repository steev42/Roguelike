using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowActiveCharacter : RogueElement
{
	
	// Update is called once per frame
	void Update ()
	{
/*		if (GameData.GetActiveCharacter () != null) {
			transform.position = new Vector3 (GameData.GetActiveCharacter ().location.x, GameData.GetActiveCharacter ().location.y, -1);
		}*/

		if (app.model.activeCharacter != null)
        {
			transform.position = new Vector3(app.model.activeCharacter.location.x, app.model.activeCharacter.location.y, -1);
        }
	}
}
