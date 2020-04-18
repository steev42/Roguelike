using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
	
	// Update is called once per frame
	void Update ()
	{
		Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis ("Mouse ScrollWheel");

		Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, 3f, 25f);
	}
}
