using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFeatures
{
    public WallState state; // Open, Closed, Unavailable
    public WallStyle style; // Open, Worked, Natural
    public DoorType door; // None, Open, Visible, Secret
    public bool isLocked; // true if door is locked

    public void setFeatures(WallState _state, WallStyle _style)
    {
        state = _state;
        style = _style;
        door = DoorType.NONE;
        isLocked = false;
    }

    public void setFeatures(WallState _state, WallStyle _style, DoorType _door)
    {
        state = _state;
        style = _style;
        door = _door;
        isLocked = false;
    }

    public void setFeatures(WallState _state, WallStyle _style, DoorType _door,
            bool _lock)
    {
        state = _state;
        style = _style;
        door = _door;
        isLocked = _lock;
    }
}
