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

    public Texture2D mapImage;

    public int CreateGridHeight = 32;
    public int CreateGridWidth = 32;
    public int OffscreenBufferSize = 5;

    // Use this for initialization
    void Start()
    {
        GameObject tile_holder = new GameObject();
        tile_holder.name = "Tiles";

        for (int x = 0; x < CreateGridWidth; x++)
        {
            for (int y = 0; y < CreateGridHeight; y++)
            {
                GameObject tile_go = new GameObject();
                tile_go.transform.SetParent(tile_holder.transform);
                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector2(x, y);
                SpriteRenderer sr = tile_go.AddComponent<SpriteRenderer>();
                sr.color = Color.gray;
                sr.sortingLayerName = "Floor";
                TileDisplay td = tile_go.AddComponent<TileDisplay>();
                //TODO Replace this with a sprite manager of some sort.
                td.floor_sprite = floor_sprite;
                td.wall_sprite = wall_sprite;
                td.OffsetFromOrigin = new Vector2(x, y);
            }
        }

		
        CharacterData cd = new CharacterData(new Vector2(0, 16));

        GameObject tile_holder = new GameObject();
        tile_holder.name = "Tiles";
        MapLoaderImage mli = tile_holder.AddComponent<MapLoaderImage>();
        mli.floor_sprite = floor_sprite;
        mli.wall_sprite = wall_sprite;
        mli.mapImage = mapImage;


        CharacterData cd = new CharacterData(new Vector2(15, 16));

        GameObject player_go = new GameObject();
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
        LightSource l = player_go.AddComponent<LightSource>();
        l.UpdateIntensity(16.0f);
        
        GameData.MapCharacterToObject(cd, player_go);

       


        /*    CharacterData nuper_data = new CharacterData(new Vector2(5, 5)); 

        GameObject nuper_go = new GameObject();
        nuper_go.name = "Test Nuper";
        //nuper_go.transform.position = new Vector2 (5, 5);
        AI_Critter ai = nuper_go.AddComponent<AI_Critter>();
        ai.Character = nuper_data;
        SpriteRenderer nuper_sr = nuper_go.AddComponent<SpriteRenderer>();
        nuper_sr.color = Color.magenta;
        nuper_sr.sprite = nuper_sprite;
        UpdateCharacterSpriteLocation nscr = nuper_go.AddComponent<UpdateCharacterSpriteLocation>();
        nscr.cd = nuper_data;

        GameData.MapCharacterToObject(nuper_data, nuper_go);
*/


        MapLoaderImage mli = tile_holder.AddComponent<MapLoaderImage>();
        mli.floor_sprite = floor_sprite;
        mli.wall_sprite = wall_sprite;
        mli.mapImage = mapImage;

    }
}
