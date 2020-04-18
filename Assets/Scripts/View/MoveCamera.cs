using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

	private Vector3 doubleLastPosition;
	private Vector3 lastPosition;
	private Vector3 cameraOffset = Vector3.zero;
	private bool wasDown = false;

	
	// Update is called once per frame
	void Update ()
	{
		bool isDown = Input.GetMouseButton (0);

		if (!wasDown && isDown) {			// Clicked
			lastPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
		} else if (wasDown && isDown) {			// Dragged
			Vector3 DragPos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
			Vector3 delta = lastPosition - DragPos;
			if (doubleLastPosition == DragPos) {
				delta = Vector3.zero;
				doubleLastPosition = lastPosition;
				lastPosition = DragPos;
			} else {
				doubleLastPosition = lastPosition;
				lastPosition = DragPos;
			}
			cameraOffset = delta;


		} else if (wasDown && !isDown) {			// released
			cameraOffset = Vector3.zero;
		}

		wasDown = isDown;
		transform.position = transform.position + cameraOffset;
	}
}
