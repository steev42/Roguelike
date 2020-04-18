using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public class BitImageMapLevel : AbstractMapLevel
{

    public class BitImageParameters : Parameters
    {
        public string imagePath;
        public string resourceName;
        public bool simpleGeneration = true;

        public BitImageParameters() { }
    }

    private BitImageParameters levelParameters;

    public override int Width { get { return width; } }
    private int width;
    private int height;
    public override int Height { get { return height; } }

    public BitImageMapLevel()
    {

    }

    public override void GenerateLevel()
    {
        if (levelParameters == null)
        {
            throw new NullReferenceException("No parameters to generate level.");
        }
        //byte[] fileData;
        //fileData = File.ReadAllBytes(levelParameters.resourceName);

        Texture2D tex = Resources.Load<Texture2D>(levelParameters.resourceName);
        if (tex == null)
        {
            Debug.Log("Unable to load texture " + levelParameters.resourceName);
            return;
        }

        //TODO: Try from sprite if we can't find texture.

        Color32[] pixels = tex.GetPixels32();
        width = tex.width;
        height = tex.height;
        if (levelParameters.simpleGeneration)
        {
            for (int x = 0; x < tex.width; x++)
            {
                for (int y = 0; y < tex.height; y++)
                {
                    LocationData ld = new LocationData(x,y);

                    if (pixels[(y * tex.width) + x].Equals(new Color32(0, 0, 0, 255)))
                    {
                        WallObject wall = new WallObject(DirectionEnum.FULL_TILE);
                        ld.JoinTile(wall);
                    }
                    else
                    {
                        // Do nothing for a floor.
                    }

                    mapData.Add(new Vector2(x, y), ld);
                }
            }

        }
        else
            throw new NotImplementedException("Unable to generate 4-quadrant map at this time.");
    }

    public override void SetParameters(Parameters p)
    {
        levelParameters = (BitImageParameters) p;
    }
}