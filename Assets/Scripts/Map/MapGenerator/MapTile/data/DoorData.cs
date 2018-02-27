using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorData : AbstractFeature
{

    private DoorType type = DoorType.OPEN;

private int width = 1;

public DoorData()
{

}

public DoorData(DoorData dd)
{
    type = dd.getType();
    width = dd.getWidth();
    location = dd.getLocation();
}

public DoorData(int w)
{
    width = w;
}

public DoorData(int w, DoorType t)
{
    width = w;
    type = t;
}

/**
 * @return the type
 */
public DoorType getType()
{
    return type;
}

/**
 * @param type
 *            the type to set
 */
public void setType(DoorType type)
{
    this.type = type;
}

/**
 * @return the width
 */
public int getWidth()
{
    return width;
}

/**
 * @param width
 *            the width to set
 */
public void setWidth(int width)
{
    this.width = width;
}

/**
 * Recenters the location of the door based on the row and col offsets. If
 * any location is out of range due to this recentering, remove the
 * location.
 * Return the number of locations remaining.
 * 
 * @param colOffset
 *            Col offset added to the current X locations.
 * @param rowOffset
 *            Row offset added to the current Y locations.
 * @param width
 *            Max width (X) recentering can place locations.
 * @param height
 *            Max height (Y) recentering can place locations.
 * @return The number of locations remaining after recentering.
 */
public int recenterLocations(int colOffset, int rowOffset, int width,
        int height)
{
    List<DungeonPoint> removeList = new List<DungeonPoint>();
    foreach (DungeonPoint dp in location)
    {
        dp.setX(dp.getX() + colOffset);
        dp.setY(dp.getY() + rowOffset);
        if (dp.getX() < 0 || dp.getX() > width || dp.getY() < 0
                || dp.getY() > height)
        {
            removeList.Add(dp);
        }
    }
    foreach (DungeonPoint dp in removeList)
    {
        location.Remove(dp);
    }

    return location.Count;
}

}
