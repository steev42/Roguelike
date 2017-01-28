using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowActiveCharacter : MonoBehaviour
{
	
	// Update is called once per frame
	void Update ()
	{
		if (GameData.GetActiveCharacter () != null) {
			transform.position = new Vector3 (GameData.GetActiveCharacter ().location.x, GameData.GetActiveCharacter ().location.y, -1);
		}
	}
}
