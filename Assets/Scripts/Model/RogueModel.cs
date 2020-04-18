using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueModel : RogueElement
{
    bool dataInitialized = false;
    public CharacterData activeCharacter;
    public List<IObject> objectsInGame;
    AbstractMapLevel map;

    // Start is called before the first frame update
    void Start()
    {
        objectsInGame = new List<IObject>();
        // Initialize Character Data
        CharacterData cd = new CharacterData(new Vector2(15,16));
        cd.Name = "Player";
        objectsInGame.Add(cd);
        activeCharacter = cd;
       // app.view.DisplayObject(cd, true);

        // Initialize Level Map
        map = LevelFactory.getInstance().getLevel(LevelFactory.LevelType.TEST_BLANK);
        if (map == null)
        {
            Debug.LogError("Unable to generate map.");
            Application.Quit(-1);
        }
        

        // Initialize Enemies 
        CharacterData nuper_data = new CharacterData(new Vector2(12,12));
        nuper_data.Name = "Target Dummy (10)";
        nuper_data.type = "Nuper";
        objectsInGame.Add(nuper_data);
      //  app.view.DisplayObject(nuper_data, true);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (IObject obj in objectsInGame)
        {
            app.view.DisplayObject(obj, true);
        }
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
                    app.view.level.UpdateVisibility(new Vector2(x, y), true);
                }
            }
        }
    }
}
