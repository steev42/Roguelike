using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightedLocation : MonoBehaviour
{
	public Color originalColor;
	public float lightLevel;

	// Update is called once per frame
	void Update ()
	{
		lightLevel = 0f;

		foreach (LightSource l in GameData.GetLightSources()) {
			float distance = Vector2.Distance ((Vector2)transform.position, l.location);

			//distance += 1; // This should make it so that the light range (at canSeeLight=1.0) is equal to the square root of the intensity.
			if (CanSeeLight (transform.position, l.location)) {

				if (distance == 0) {
					lightLevel += l.lightIntensity;
				} else {
					lightLevel += l.lightIntensity / (distance * distance);
				}
			}
		}
		float minLight = GameData.GetActiveCharacter ().minimumLightToSee;
		float maxLight = GameData.DEFAULT_LIGHT;
		float pct = Mathf.Min (maxLight, lightLevel) - minLight / (maxLight - minLight);

		if (lightLevel > minLight) {
			GetComponent<SpriteRenderer> ().color = originalColor;
		} else {
			GetComponent<SpriteRenderer> ().color = new Color (Mathf.Lerp (0, originalColor.r, pct), Mathf.Lerp (0, originalColor.g, pct), Mathf.Lerp (0, originalColor.b, pct));
		}
	}

	bool CanSeeLight (Vector2 myLocation, Vector2 lightLocation, float squareSize = 1.0f)
	{
		float half = (squareSize / 2) - .001f;
		Vector2[] Corners = {Vector2.zero,
			new Vector2 (-half, -half),
			new Vector2 (half, -half),
			new Vector2 (half, half),
			new Vector2 (-half, half)
		};
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				if (myLocation.x == 4 && myLocation.y == 1) {
					Debug.DrawLine (myLocation + Corners [i], lightLocation + Corners [j]);
				}
				RaycastHit2D hit = Physics2D.Linecast (myLocation + Corners [i], lightLocation + Corners [j]);
				if (hit.collider == null) {
					return true;
				}
			}

		}
		return false;
	}

}
