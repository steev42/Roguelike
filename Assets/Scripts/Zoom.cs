using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    private float closestZoom = 3f;
    // 6 squares vertically
    private float furthestZoom = 16f;
    // 32 squares vertically.
	
    // Update is called once per frame
    void Update()
    {
        float clampZoom = Mathf.Min(furthestZoom, furthestZoom * Camera.main.aspect);
        //furthestZoom = Camera.main.aspect

        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, closestZoom, clampZoom);
    }
}
