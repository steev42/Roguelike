using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NaturalCavern : AbstractMapTile
{

    public const string SEEDS = "Seed Points";

    private List<DungeonPoint> chamber;


    public NaturalCavern(DungeonPoint dp) : base(dp)
    {
    }

    public override void setDefaultOptions()
    {
        base.setDefaultOptions();
        options[SEEDS] = new TileOption(SEEDS, 3);
        setOption(FRACTAL_AVE_HORIZ_SPLIT_SIZE, 10);
        setOption(FRACTAL_AVE_VERT_SPLIT_SIZE, 10);
        setOption(FRACTAL_MIN_HORIZ_SPLIT_SIZE, 5);
        setOption(FRACTAL_MAX_HORIZ_SPLIT_SIZE, 15);
        setOption(FRACTAL_MIN_VERT_SPLIT_SIZE, 5);
        setOption(FRACTAL_MAX_VERT_SPLIT_SIZE, 15);
    }

    static int countLevel = 0;

    public bool isExpansionValid(DungeonPoint expandTo,
            List<List<DungeonPoint>> openPoints)
    {
        // First, check that the expansion point falls within the size of the
        // dungeon complex
        if (expandTo.getX() < 1
                || expandTo.getX() > options[WIDTH].getOptionValue()
                || expandTo.getY() < 1
                || expandTo.getY() > options[HEIGHT].getOptionValue())
        {
            return false;
        }
        // If it is in the array, check that the new point is open
        foreach (List<DungeonPoint> iList in openPoints)
        {
            foreach (DungeonPoint dp in iList)
            {
                if (expandTo.equals(dp))
                {
                    return false;
                }
            }
        }

        // Have passed all validation conditions.
        return true;
    }

    public override void setDoors(List<DoorData> list)
    {
        doors = list;
    }


    public override GridList<AbstractMapTile> makeDoors(GridList<AbstractMapTile> cells)
    { // TODO Finish this method
        int height = options[HEIGHT].getOptionValue();
        int width = options[WIDTH].getOptionValue();
        if (doors == null)
            doors = new List<DoorData>();

        if (getWallState(CardinalPoints.NORTH) == WallState.OPEN)
        {
            int a = Random.Range(0, width) + 1;
            DoorData dd = new DoorData();
            dd.addLocation(new DungeonPoint(a, 1));
            doors.Add(dd);
        }
        if (getWallState(CardinalPoints.EAST) == WallState.OPEN)
        {
            int a = Random.Range(0, height) + 1;
            DoorData dd = new DoorData();
            dd.addLocation(new DungeonPoint(width, a));
            doors.Add(dd);
        }
        if (getWallState(CardinalPoints.SOUTH) == WallState.OPEN)
        {
            int a = Random.Range(0, width) + 1;
            DoorData dd = new DoorData();
            dd.addLocation(new DungeonPoint(a, height));
            doors.Add(dd);
        }
        if (getWallState(CardinalPoints.WEST) == WallState.OPEN)
        {
            int a = Random.Range(0, width) + 1;
            DoorData dd = new DoorData();
            dd.addLocation(new DungeonPoint(1, a));
            doors.Add(dd);
        }

        foreach (DoorData dd in doors)
        {
            foreach (AbstractMapTile at in cells)
            {
                at.passDownDoors(doors);
            }
        }

        return cells;
    }

    public override void makeFeatures()
    {
        // TODO Auto-generated method stub

    }

    public override void makeFloorPlan()
    {
        int height = options[HEIGHT].getOptionValue();
        int width = options[WIDTH].getOptionValue();
        chamber = new List<DungeonPoint>(); // Clear previous data
        List<DungeonPoint> expansionPoints = new List<DungeonPoint>();
        List<List<DungeonPoint>> openPoints = new List<List<DungeonPoint>>();

        // Set doors as initial openings
        if (doors != null)
        {
            foreach (DoorData dp in doors)
            {
                List<DungeonPoint> ldp = new List<DungeonPoint>();
                ldp.AddRange(dp.getLocation());
                expansionPoints.AddRange(dp.getLocation());
                openPoints.Add(ldp);
            }
        }

        // Find additional seeds
        for (int i = 0; i < options[SEEDS].getOptionValue(); i++)
        {
            bool generateNewIndex = true;

            DungeonPoint dp = new DungeonPoint(0, 0);
            List<DungeonPoint> ldp = new List<DungeonPoint>();

            while (generateNewIndex)
            {
                generateNewIndex = false;
                int openX = Random.Range(0, width) + 1;
                int openY = Random.Range(0, height) + 1;

                dp = new DungeonPoint(openX, openY);

                // Check to make sure it's not already open.
                // Should we make sure it's not within a range of an open point?
                foreach (List<DungeonPoint> opens in openPoints)
                {
                    if (opens.Contains(dp))
                    {
                        generateNewIndex = true;
                    }
                }
            }

            expansionPoints.Add(dp);
            ldp.Add(dp);
            openPoints.Add(ldp);
        }

        // Expand until all the starting points are connected
        while (openPoints.Count > 1 && expansionPoints.Count > 0)
        {
            DungeonPoint expandFrom = expansionPoints[Random.Range(0, expansionPoints.Count)];
            DungeonPoint north = new DungeonPoint(expandFrom.getX(),
                    expandFrom.getY() - 1);
            DungeonPoint south = new DungeonPoint(expandFrom.getX(),
                    expandFrom.getY() + 1);
            DungeonPoint east = new DungeonPoint(expandFrom.getX() + 1,
                    expandFrom.getY());
            DungeonPoint west = new DungeonPoint(expandFrom.getX() - 1,
                    expandFrom.getY());

            bool northValid = isExpansionValid(north, openPoints);
            bool southValid = isExpansionValid(south, openPoints);
            bool eastValid = isExpansionValid(east, openPoints);
            bool westValid = isExpansionValid(west, openPoints);

            List<DungeonPoint> validExpansions = new List<DungeonPoint>();
            if (northValid)
            {
                validExpansions.Add(north);
            }
            if (eastValid)
            {
                validExpansions.Add(east);
            }
            if (southValid)
            {
                validExpansions.Add(south);
            }
            if (westValid)
            {
                validExpansions.Add(west);
            }

            if (validExpansions.Count == 0)
            {
                // No valid expansion points. Remove this point from expansion
                // list and get the next point.
                expansionPoints.Remove(expandFrom);
                continue;
            }

            // See which of the open point lists contains the chosen point
            List<DungeonPoint> myList = null;
            int myListIndex = -1;
            foreach (List<DungeonPoint> opens in openPoints)
            {
                if (opens.Contains(expandFrom))
                {
                    myList = opens;
                    myListIndex = openPoints.IndexOf(opens);
                }
            }

            // Handle expansion direction
            int cardinal = Random.Range(0, validExpansions.Count);
            DungeonPoint expandTo = expandFrom;
            expandTo = validExpansions[cardinal];

            // Check if the expansion will end up next to another
            // list; if so, combine them and remove one of the lists.

            List<List<DungeonPoint>> removeList = new List<List<DungeonPoint>>();
            foreach (List<DungeonPoint> opens in openPoints)
            {
                if (openPoints.IndexOf(opens) == myListIndex)
                {
                    // Don't need to check for combined lists.
                    continue;
                }
                else
                {
                    DungeonPoint _north = new DungeonPoint(expandFrom.getX(),
                            expandFrom.getY() - 1);
                    DungeonPoint _south = new DungeonPoint(expandFrom.getX(),
                            expandFrom.getY() + 1);
                    DungeonPoint _east = new DungeonPoint(
                            expandFrom.getX() + 1, expandFrom.getY());
                    DungeonPoint _west = new DungeonPoint(
                            expandFrom.getX() - 1, expandFrom.getY());

                    if (opens.Contains(_north) || opens.Contains(_south)
                            || opens.Contains(_east) || opens.Contains(_west))
                    {
                        // Expansion is connected to this list.
                        // Combine them, and remove the other list.
                        openPoints[myListIndex].AddRange(opens);
                        removeList.Add(opens);
                        // No break; if one expansion connects three or
                        // more lists, the for loop will combine them all.
                    }
                }
            }
            foreach (List<DungeonPoint> remove in removeList)
            {
                openPoints.Remove(remove);
            }

            // All bad cases accounted for. New point is valid, and does
            // not connect to other lists. Set it as an open point in
            // current list, and set it available for expansion.
            expansionPoints.Add(expandTo);
            openPoints[openPoints.IndexOf(myList)].Add(expandTo);

        } // while (openPoints.Count > 1 && expansionPoints.Count > 0)

        // Have generated the cavern.
    }

    /**
     * Divide the current grid up according to given values, using a
     * randomization generator that weights towards the average.
     * 
     * @param min
     *            The minimum size to split
     * @param ave
     *            The average size to split
     * @param max
     *            The maximum size to split
     * @param full
     *            The size of the grid to split in this direction
     * @return A list of ints containing the size of each split.
     */
    private List<int> findSplits(int min, int ave, int max, int full)
    {
        int marker = 0;
        List<int> splits = new List<int>();
        while (marker < full)
        {
            int a = 5;
            //TODO int a = RandomNumberGenerator.getNormalBetween(min, ave, max);
            if (marker + a > full)
            {
                a = full - marker;
            }
            else if (marker + a + min > full)
            {
                if ((full - marker) > max)
                {
                }

                a = full - marker;
            }
            splits.Add(a);
            marker += a;
        }
        return splits;
    }

    public override GridList<AbstractMapTile> fractalize()
    {
        int min_vert = getOption(FRACTAL_MIN_VERT_SPLIT_SIZE);
        int ave_vert = getOption(FRACTAL_AVE_VERT_SPLIT_SIZE);
        int max_vert = getOption(FRACTAL_MAX_VERT_SPLIT_SIZE);
        int min_horz = getOption(FRACTAL_MIN_HORIZ_SPLIT_SIZE);
        int ave_horz = getOption(FRACTAL_AVE_HORIZ_SPLIT_SIZE);
        int max_horz = getOption(FRACTAL_MAX_HORIZ_SPLIT_SIZE);
        int w = getOption(WIDTH);
        int h = getOption(HEIGHT);

        StringBuilder sb = new StringBuilder();
        sb.Append("  Minimum Vertical Split Size : " + min_vert + "\n\t");
        sb.Append("  Average Vertical Split Size : " + ave_vert + "\n\t");
        sb.Append("  Maximum Vertical Split Size : " + max_vert + "\n\t");
        sb.Append("Minimum Horizontal Split Size : " + min_horz + "\n\t");
        sb.Append("Average Horizontal Split Size : " + ave_horz + "\n\t");
        sb.Append("Maximum Horizontal Split Size : " + max_horz + "\n\t");
        sb.Append("                        Width : " + w + "\n\t");
        sb.Append("                       Height : " + h);

        GridList<AbstractMapTile> rv = new GridList<AbstractMapTile>();

        List<int> splitNorthSouth = findSplits(min_vert, ave_vert,
                max_vert, w); // Lines drawn North to South
        List<int> splitEastWest = findSplits(min_horz, ave_horz,
                max_horz, h); // Lines drawn East to West

        if (splitNorthSouth.Count > 1 || splitEastWest.Count > 1)
        {

            int debug_row = 0;
            int debug_col = 0;
            // There is at least one division.
            foreach (int vs in splitNorthSouth)
            {
                debug_row++;
                debug_col = 1;

                List<AbstractMapTile> alt = new List<AbstractMapTile>();
                foreach (int hs in splitEastWest)
                {

                    AbstractMapTile t = MapTileFactory.getInstance().getTile(new DungeonPoint(debug_col, debug_row));
                    t.setOption(HEIGHT, vs);
                    t.setOption(WIDTH, hs);

                    if (t != null)
                    {
                        alt.Add(t);
                    }
                    debug_col++;
                }

            }
        }
        return rv;
    }
} // class
