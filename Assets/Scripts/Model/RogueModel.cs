using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueModel : RogueElement
{
    bool dataInitialized = false;
    public CharacterData activeCharacter;
    public List<IObject> objectsOnMap;
    AbstractMapLevel map;

    private CharacterData activeCharacterLastUpdate;
    private SenseEnum activeSense = SenseEnum.VISION;
    bool firstPass = true;

    // Start is called before the first frame update
    void Start()
    {

        // Initialize Level Map
        map = LevelFactory.getInstance().getLevel(LevelFactory.LevelType.TEST_BLANK);
        if (map == null)
        {
            Debug.LogError("Unable to generate map.");
            Application.Quit(-1);
        }


        objectsOnMap = new List<IObject>();
        // Initialize Character Data
        CharacterData cd = new CharacterData(map.GetLocation(new Vector2(15,16)));
        cd.Name = "Player";
        objectsOnMap.Add(cd);
        activeCharacter = cd;
        IlluminatedObject light = new IlluminatedObject();
        light.Intensity = 16f;
        cd.Attach(light);

        // Initialize Enemies 
        CharacterData nuper_data = new CharacterData(map.GetLocation(new Vector2(12, 12)));
        nuper_data.Name = "Target Dummy (10)";
        nuper_data.type = "Nuper";
        objectsOnMap.Add(nuper_data);
    }

    // Update is called once per frame
    void Update()
    {
        // Based on the current sense, find any objects that apply to that sense and set their range on the map.
        if (activeSense == SenseEnum.VISION)
        {
            if (firstPass)Debug.Log("There are " + objectsOnMap.Count + " objects on the map.");
            foreach (IObject obj in objectsOnMap)
            {
                if (obj is IlluminatedObject)
                {
                    if (firstPass) Debug.Log("Object is Illuminated.");
                    IlluminatedObject io = (IlluminatedObject)obj;
                    map.GetLocation(obj.Location).AddSenseSource(io, io.Intensity);
                }

                /*THIS SECTION IS CAUSING A LOCK CONDITION
                 * 
                 * if (obj.AttachedItems != null)
                {
                    foreach (IAttachable a in obj.AttachedItems)
                    {
                        if (a is IlluminatedObject)
                        {
                            IlluminatedObject io = (IlluminatedObject)a;
                            map.GetLocation(obj.Location).AddSenseSource(io, io.Intensity);
                        }
                    }
                }*/
            }
        }
        
        // Based on the current active character, display things that are in line of sight, AND visible to the currently selected sense.
        
        // We only care about objects if they have updated, or if the current active character has updated/moved?  How would we check that?


        /*foreach (IObject obj in objectsOnMap)
        {
            if (obj.isDetectableByCharacter(activeCharacter, activeSense))
                app.view.DisplayObject(obj, true); // TODO not boolean?  Maybe a 'transparent' rating?
        }*/

        if (app.view.level == null)
        {
            Debug.LogError("Unable to update map.");
        }
        else
        {
            app.view.level.Map = map;
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    Vector2 coord = new Vector2(x, y);
                    //if (firstPass) Debug.Log("Checking for diplay of " + coord.ToString());
                    LocationData thisLoc = map.GetLocation(coord);
                    float senseTotal = thisLoc.GetTotalSenseLevel(activeSense);
                    app.view.level.UpdateVisibility(coord, senseTotal);
                    foreach (IPhysicalObject o in thisLoc.GetContents())
                    {
                        //if (firstPass) Debug.Log("Found object in location" + coord.ToString());
                        if (o.isDetectableByCharacter(activeCharacter, activeSense))
                            app.view.DisplayObject(o, senseTotal); // TODO not boolean?  Maybe a 'transparent' rating?
                    }
                }
            }
            firstPass = false;
        }
    }
}
