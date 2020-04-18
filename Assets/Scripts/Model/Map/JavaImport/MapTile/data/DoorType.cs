using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    OPEN, 			// The wall is open, leaving no doors between.
    NONE,           // There is no door, just a wall.
    VISIBLE,        // The wall contains a visible door.
    SECRET,         // The wall looks closed, but contains a secret door.    
}

