using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
    public float minimumLightToSee;
    //public GameObject obj;
    public Vector2 location;

    public Queue<ICharacterAction> queuedActions;
    public Vector2 queuedLocation;

    public AI character_ai;

    //LightSource temp;

    public CharacterData(Vector2 loc)
    {
        //obj = o;
        minimumLightToSee = 1.0f;

        //location = o.transform.position;
        location = loc;

        //temp = new LightSource ();
        //temp.location = location;
        //temp.lightIntensity = 16;
        //GameData.AddLightSource (temp);

        queuedActions = new Queue<ICharacterAction>();
    }

    public void UpdateLocation(Vector2 loc)
    {
        location = loc;
        //temp.location = loc;
    }
}
