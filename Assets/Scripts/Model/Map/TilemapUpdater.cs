using System.Collections.Generic;
using UnityEngine;

public class TilemapUpdater : MonoBehaviour {

    private List<LocationData> locationsToUpdate;

	// Use this for initialization
	void Start () {
        locationsToUpdate = new List<LocationData>();
	}
	
	// Update is called once per frame
	void Update () {
		if (locationsToUpdate.Count > 0)
        {
            // Update the tiles.
            foreach (LocationData updating_location in locationsToUpdate)
            {
                foreach (IPhysicalObject obj in updating_location.GetContents())
                {

                }
            }

            locationsToUpdate.Clear(); // Reset for the next iteration.
        }
	}

    public void UpdateRequired(LocationData ld)
    {
        if (locationsToUpdate == null)
        {
            locationsToUpdate = new List<LocationData>();
        }

        if (locationsToUpdate.Contains(ld))
            return;

        locationsToUpdate.Add(ld);
    }
}
