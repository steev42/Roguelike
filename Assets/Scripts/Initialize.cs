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

    // Use this for initialization
    void Start()
    {
        LightSource temp = new LightSource();
        temp.location = new Vector2(0, 0);
        temp.lightIntensity = 1;
        GameData.AddLightSource(temp);
		
        CharacterData cd = new CharacterData(new Vector2(0, 0));

        GameObject player_go = new GameObject();
        player_go.name = "Player";
        //player_go.transform.position = new Vector2 (0, 0);
        //player_go.AddComponent<PCMovement> ();
        AI_Player pc = player_go.AddComponent<AI_Player>();
        pc.Character = cd;
        SpriteRenderer player_sr = player_go.AddComponent<SpriteRenderer>();
        player_sr.color = Color.white;
        player_sr.sprite = player_sprite;
        UpdateCharacterSpriteLocation scr = player_go.AddComponent<UpdateCharacterSpriteLocation>();
        scr.cd = cd;

        GameData.SetActiveCharacter(cd); // TODO This isn't the way to do this...


        CharacterData nuper_data = new CharacterData(new Vector2(5, 5)); 

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

        GameObject tile_holder = new GameObject();
        tile_holder.name = "Tiles";

        for (int x = -10; x < 10; x++)
        {
            for (int y = -10; y < 10; y++)
            {
                GameObject tile_go = new GameObject();
                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector2(x, y);
                tile_go.transform.SetParent(tile_holder.transform);
                SpriteRenderer sr = tile_go.AddComponent<SpriteRenderer>();
                sr.color = Color.gray;

                TileData td = new TileData(tile_go);

                //if (x == 3 || x == -3 || y == -3 || y == 3) {
                if (Random.Range(0, 10) == 0)
                {
                    sr.sprite = wall_sprite;
                    td.movementSpeedMultiplier = 0.0f;
                    //tile_go.AddComponent<Rigidbody2D> ();
                    tile_go.AddComponent<BoxCollider2D>();
                }
                else
                {
                    sr.sprite = floor_sprite;
                }

                LightedLocation ll = tile_go.AddComponent<LightedLocation>();
                ll.originalColor = Color.gray;

                GameData.SetTile(x, y, td);
            }
        }
    }
}
