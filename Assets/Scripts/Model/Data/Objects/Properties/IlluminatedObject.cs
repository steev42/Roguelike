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

    public void AttachToObject(IObject o)
    {
        if (o != null)
        {
            attachedObject = o;
            o.Attach(this);
        }
    }

    public void LocationUpdated(Vector2 newLocation)
    {
        location = newLocation;
    }

    public float Intensity { get { return lightIntensity; } set { lightIntensity = value; } }

    public Vector2 Location { get { return location; } }

}