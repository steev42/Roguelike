using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallState
{
    OPEN,
    CLOSED,        // There is a wall 
    UNAVAILABLE,   // The wall is open, and is not allowed to be closed.

}
