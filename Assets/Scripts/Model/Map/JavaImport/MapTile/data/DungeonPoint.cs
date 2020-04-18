using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DungeonPoint
{
    /**
     * The X coordinate is the West-East coordinate (left to right).
     */
    private int X;
    /**
     * The Y coordinate is the North-South coordinate (up to down).
     */
    private int Y;
    /**
     * The Z coordinate is the Top-Bottom coordinate (different images).
     */
    private int Z;

    /**
     * Constructor that sets the x and y coordinates. Z coordinate
     * defaults to 0.
     */
    public DungeonPoint(int x, int y)
    {
        X = x;
        Y = y;
        Z = 0;
    }

    /**
     * Constructor that sets x, y, and z coordinates.
     */
    public DungeonPoint(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /**
     * Get West-East coordinate (left to right).
     * 
     * @return the x
     */
    public int getX()
    {
        return X;
    }

    /**
     * Set West-East coordinate (left to right).
     * 
     * @param x
     *            the x to set
     */
    public void setX(int x)
    {
        X = x;
    }

    /**
     * Get North-South coordinate (up to down).
     * 
     * @return the y
     */
    public int getY()
    {
        return Y;
    }

    /**
     * Set North-South coordinate (up to down).
     * 
     * @param y
     *            the y to set
     */
    public void setY(int y)
    {
        Y = y;
    }

    /**
     * Get Top-Bottom coordinate (different images).
     * 
     * @return the z
     */
    public int getZ()
    {
        return Z;
    }

    /**
     * Set Top-Bottom coordinate (different images).
     * 
     * @param z
     *            the z to set
     */
    public void setZ(int z)
    {
        Z = z;
    }

    public int hashCode()
    {
        const int prime = 31;
        int result = 1;
        result = prime * result + X;
        result = prime * result + Y;
        result = prime * result + Z;
        return result;
    }

    public bool equals(System.Object obj)
    {
        if (this == obj)
            return true;
        if (obj == null)
            return false;
        if (GetType() != obj.GetType())
            return false;
        DungeonPoint other = (DungeonPoint)obj;
        if (X != other.X)
            return false;
        if (Y != other.Y)
            return false;
        if (Z != other.Z)
            return false;
        return true;
    }

    public string toString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("DungeonPoint [X=");
        builder.Append(X);
        builder.Append(", Y=");
        builder.Append(Y);
        builder.Append(", Z=");
        builder.Append(Z);
        builder.Append("]");
        return builder.ToString();
    }

}
