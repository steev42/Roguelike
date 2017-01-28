using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
	public const float DEFAULT_LIGHT = 4.0f;

	static Dictionary <Vector2, TileData> tileMap;

	static List<LightSource> lightSources;

	static CharacterData activeCharacter;

	public static bool isValidMove (Vector2 target)
	{
		if (GetTile (target).movementSpeedMultiplier != 0) {
			return true;
		}
		//TODO Other conditions, like another creature in this tile?
		return false;
	}

	/*static ActionTree initiativeTracker;

	public static void EnqueueAction (ActionDefinition a)
	{
		if (initiativeTracker == null) {
			initiativeTracker = new ActionTree (a);
		} else {
			initiativeTracker.EnqueueAction (a);
		}
			
		PerformNextAction ();
	}

	public static void PerformNextAction ()
	{
		if (initiativeTracker == null) {
			return;
		} else {
			ActionDefinition a = initiativeTracker.DequeueAction (null);
			a.doAction ();
		}
	}*/

	public static void AddLightSource (LightSource l)
	{
		if (lightSources == null) {
			lightSources = new List<LightSource> ();
		}

		lightSources.Add (l);
	}

	public static List<LightSource> GetLightSources ()
	{
		if (lightSources == null) {
			lightSources = new List<LightSource> ();
		}
		return lightSources;
	}

	public static void SetTile (int x, int y, TileData td)
	{
		if (tileMap == null) {
			tileMap = new Dictionary<Vector2, TileData> ();
		}

		tileMap [new Vector2 (x, y)] = td;
	}

	public static TileData GetTile (Vector2 tileCoord)
	{
		if (tileMap.ContainsKey (tileCoord)) {
			return tileMap [tileCoord];
		} else {
			return null;
		}
	}

	public static TileData GetTile (int x, int y)
	{
		Vector2 pos = new Vector2 (x, y);
		return GetTile (pos);
	}

	public static void SetActiveCharacter (CharacterData activate)
	{
		activeCharacter = activate;
	}

	public static CharacterData GetActiveCharacter ()
	{
		return activeCharacter;
	}
}
