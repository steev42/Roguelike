using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour
{

    //public GameObject tile_prefab;

    public Sprite floor_sprite;
    public Sprite wall_sprite;

    public Sprite player_sprite;

    public Sprite nuper_sprite;
    
    public enum mapStyle { MapImage, TileGenerator };

    public mapStyle mapInitializeStyle;
    public Texture2D mapImage;


    bool isInitialized = false;

    // Use this for initialization
    void Update()
    {

        if (isInitialized)
            return;

        isInitialized = true;

        LevelFactory.getInstance().getLevel(LevelFactory.LevelType.TEST_BLANK);

        /*GameObject tile_holder = new GameObject();
        tile_holder.name = "Tiles";

        MapLoaderImage mli = tile_holder.AddComponent<MapLoaderImage>();
        mli.floor_sprite = floor_sprite;
        mli.wall_sprite = wall_sprite;
        mli.mapImage = mapImage;*/

        //CharacterData cd = new CharacterData(new Vector2(15, 16));

      /*  GameObject player_go = new GameObject();
        player_go.name = "Player";
        //player_go.transform.position = new Vector2 (0, 0);
        //player_go.AddComponent<PCMovement> ();
        AI_Player pc = player_go.AddComponent<AI_Player>();
        pc.Character = cd;
        GameData.SetActiveCharacter(cd); // TODO This isn't the way to do this...
        SpriteRenderer player_sr = player_go.AddComponent<SpriteRenderer>();
        player_sr.color = Color.white;
        player_sr.sprite = player_sprite;
        player_sr.sortingLayerName = "Units";
        UpdateCharacterSpriteLocation scr = player_go.AddComponent<UpdateCharacterSpriteLocation>();
        scr.cd = cd;
        LightSource l = player_go.AddComponent<LightSource>();  // TODO Eventually mapped to an inventory object rather than character.
        l.UpdateIntensity(16.0f);*/
        
       /* GameData.MapCharacterToObject(cd, player_go);*/
        //GameData.GetTile(cd.location).GetComponent<TileData>().JoinTile(cd);

       
        // Create a target dummy for testing purposes...

       /* CharacterData nuper_data = new CharacterData(new Vector2(12, 12)); 

        GameObject nuper_go = new GameObject();
        nuper_go.name = "Target Dummy (10)";
        //nuper_go.transform.position = new Vector2 (5, 5);
        AI_TargetDummy ai = nuper_go.AddComponent<AI_TargetDummy>();
        ai.Character = nuper_data;
        SpriteRenderer nuper_sr = nuper_go.AddComponent<SpriteRenderer>();
        nuper_sr.color = Color.magenta;
        nuper_sr.sprite = nuper_sprite;
        nuper_sr.sortingLayerName = "Units";
        UpdateCharacterSpriteLocation nscr = nuper_go.AddComponent<UpdateCharacterSpriteLocation>();
        nscr.cd = nuper_data;

        GameData.MapCharacterToObject(nuper_data, nuper_go);
        //GameData.GetTile(nuper_data.location).GetComponent<TileData>().JoinTile(nuper_data);*/

    }
}
