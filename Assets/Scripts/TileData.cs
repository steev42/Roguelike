using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
	public float movementSpeedMultiplier;
	public GameObject obj;

	public TileData (GameObject o)
	{
		movementSpeedMultiplier = 1.0f;
		obj = o;
	}
}
