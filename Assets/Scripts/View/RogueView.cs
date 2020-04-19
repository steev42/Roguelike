using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueView : RogueElement
{
    [System.NonSerialized]
    public LevelView level;

    public Sprite playerSprite;
    public Sprite nuperSprite;
    public Sprite wallSprite;

    public Dictionary<IObject, GameObject> displayedObjects;

    List<IObject> flaggedForUpdate;
    
    // Start is called before the first frame update
    void Start()
    {
        level = GetComponent<LevelView>();
        if (level == null)
        {
            Debug.LogWarning("No Level view");
        }
        displayedObjects = new Dictionary<IObject, GameObject>();
        flaggedForUpdate = new List<IObject>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (IObject o in flaggedForUpdate)
        {
            if (displayedObjects.ContainsKey(o))
            {
                continue;
            }
            else
            {
                if (o is CharacterData)
                {
                    CharacterData cd = (CharacterData)o;
                    if (cd.type == "Player")
                    {
                        AddPlayerCharacter(cd);
                    }
                    else if (cd.type == "Nuper")
                    {
                        AddEnemy(cd);
                    }
                }
            }
        }

        flaggedForUpdate.Clear();
    }

    public void DisplayObject(IObject o, float visibility)
    {
        if (visibility > 0)
        {
            flaggedForUpdate.Add(o);
        }
        else
        {
            if (displayedObjects.ContainsKey(o))
            {
                GameObject obj = displayedObjects[o];
                displayedObjects.Remove(o);
                Destroy(displayedObjects[o]);
            }
        }
    }

    private void AddEnemy(CharacterData cd)
    {
        GameObject go = new GameObject();
        go.name = cd.Name;
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.color = Color.magenta;
        sr.sprite = nuperSprite;
        sr.sortingLayerName = "Units";
        UpdateCharacterSpriteLocation scr = go.AddComponent<UpdateCharacterSpriteLocation>();
        scr.cd = cd;
        displayedObjects[cd] = go;
    }

    private void AddPlayerCharacter(CharacterData cd)
    {
        GameObject go = new GameObject();
        go.name = cd.Name;
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.color = Color.white;
        sr.sprite = playerSprite;
        sr.sortingLayerName = "Units";
        UpdateCharacterSpriteLocation scr = go.AddComponent<UpdateCharacterSpriteLocation>();
        scr.cd = cd;
        displayedObjects[cd] = go;
    }
}
