using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class IlluminatedObject : ISenseProperty
{
    IObject attachedObject;

    private float lightIntensity;
    private Color lightColor;
    private Vector2 location;
    private Vector2 lastAppliedLocation = Vector2.positiveInfinity; // Default value that won't happen so first update is forced.

    public SenseEnum Sense { get { return SenseEnum.VISION; } }

    public void AttachToObject(IObject o)
    {
        if (o != null)
        {
            attachedObject = o;
            o.Attach(this);
        }
    }

    public Vector2 Location { get { return location; } set { location = value; } }

    public float Intensity { get { return lightIntensity; } set { lightIntensity = value; } }

    public bool NeedsUpdate 
    {
        get 
        {
            if (lastAppliedLocation == Vector2.positiveInfinity)
                return true;
            if (location == null)
                return true;
            if (location == lastAppliedLocation)
                return false;
            return true;
            //return (location != lastAppliedLocation); 
        }
    }
    
    public Vector2 UpdatedAt { get { return lastAppliedLocation; } set { lastAppliedLocation = value; } }

}