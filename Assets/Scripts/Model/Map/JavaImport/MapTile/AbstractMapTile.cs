using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMapTile
{
    public const string HEIGHT = "Height";
    public const string WIDTH = "Width";
    public const string DPI = "Dots Per Inch";
    public const string PIXELS_PER_SQUARE = DPI;
    public const string PADDING = "Padding";

    public const string FRACTAL_RATE = "Fractal Rate";
    public const string FRACTAL_MIN_VERT_SPLIT_SIZE = "Minimum Vertical";
    public const string FRACTAL_MAX_VERT_SPLIT_SIZE = "Maximum Vertical";
    public const string FRACTAL_AVE_VERT_SPLIT_SIZE = "Average Vertical";
    public const string FRACTAL_MIN_HORIZ_SPLIT_SIZE = "Minimum Horizontal";
    public const string FRACTAL_MAX_HORIZ_SPLIT_SIZE = "Maximum Horizontal";
    public const string FRACTAL_AVE_HORIZ_SPLIT_SIZE = "Average Horizontal";

    /**
     * Private instance of the random number generator
     */
    protected static Random generator;

    private DungeonPoint coordinate;
    private Dictionary<CardinalPoints, WallFeatures> box;

    protected List<DoorData> doors;

    /**
     * Store the options used to generate the random map.
     */
    protected Dictionary<string, TileOption> options;

    protected GridList<AbstractMapTile> cells;

    /**
     * 
     */
    public AbstractMapTile(DungeonPoint dp)
    {
        setDefaultOptions();

        coordinate = dp;
        box = new Dictionary<CardinalPoints, WallFeatures>();
        box.Add(CardinalPoints.NORTH, new WallFeatures());
        box.Add(CardinalPoints.EAST, new WallFeatures());
        box.Add(CardinalPoints.SOUTH, new WallFeatures());
        box.Add(CardinalPoints.WEST, new WallFeatures());
        WallFeatures wf = new WallFeatures();
        wf.setFeatures(WallState.CLOSED, WallStyle.WORKED);
        box.Add(CardinalPoints.UP, wf);
        box.Add(CardinalPoints.DOWN, wf);
    }

    /**
     * This method sets the default options for the Tile. If it is overridden,
     * ensure that it is either called via super.setDefaultOptions() or that
     * any methods using these options are also overridden.
     */
    public virtual void setDefaultOptions()
    {
        options = new Dictionary<string, TileOption>();

        setOption(HEIGHT, 30);
        setOption(WIDTH, 30);
        // TODO : Split Paint Options into another storage facility;
        // they shouldn't have the ability to be varied by tile.
        setOption(PADDING, 0);
        setOption(DPI, 20);
    }

    /**
     * Simple set option for the tile. This method allows the version
     * number to remain at its default.
     * 
     * @param option
     *            The name of the option to set
     * @param value
     *            The integer value of the option
     */
    public virtual void setOption(string option, int value)
    {
        options.Add(option, new TileOption(option, value));
    }

    /**
     * Set an option to the given TileOption. This method is the
     * easiest way to set an option with a version number.
     * 
     * @param option
     *            The name of the option to set
     * @param value
     *            The TileOption object that contains the option data
     */
    public virtual void setOption(string option, TileOption value)
    {
        options.Add(option, value);
    }

    /**
     * @return the options
     */
    public virtual Dictionary<string, TileOption> getOptions()
    {
        return options;
    }

    /**
     * Get the value of a given option.
     * 
     * @param option
     *            The string value in which the option is stored
     * @return the value of the option, or 0 if it cannot be found.
     */
    public virtual int getOption(string option)
    {
        if (options.ContainsKey(option))
        {
            return options[option].getOptionValue();
        }
        else
            return 0;
    }

    /**
     * @param options
     *            the options to set
     */
    public virtual void setOptions(Dictionary<string, TileOption> options)
    {
        this.options = options;
    }

    /**
     * This is a list of openings into the tile. Border doors should be of
     * the format X,Y, where X and Y are the coordinate related to the tile,
     * and should be between 0 and width or height (respectively). A door that
     * is not on a border (0,Y; X,0; width,Y; X, height) will be treated as an
     * additional seed; perhaps an opening into another floor?
     * 
     * @param list
     *            The DoorData that correspond to the doors on this tile
     */
    public abstract void setDoors(List<DoorData> list);

    /**
     * This is the default generation code. By default, this randomly splits
     * the tile into multiple tiles, forms connections between them, and then
     * generates a tile on each (which can then repeat the process). Once the
     * tiles have all been generated, creates a floor plan on each, and adds
     * any additional features.
     */
    public virtual void generate()
    {
        cells = fractalize();
        if (cells.Count != 0) // If there are ANY divisions at all
        {
            cells = createMaze(cells);

            foreach (AbstractMapTile c in cells)
            {
                c.generate();
                cells = makeDoors(cells);
            }
        }
        else
        {
            makeFloorPlan();
            makeFeatures();
        }
    }

    /**
     * Take the given dungeon grid, and initialize all of the outer walls to the
     * given state. Returns the list of all outside walls for purposes of adding
     * to list of all connections.
     * 
     * Wall entries that are outside the grid are automatically set to
     * <code>WallState.UNAVAILABLE</code>.
     * 
     * 
     * @param walls
     *            The grid to initialize
     * @param wallRows
     *            The number of rows in the grid
     * @param wallColumns
     *            The number of columns in the grid
     * @param desiredBorderState
     *            The state in which to set the outside walls.
     * @return A list containing all of the border walls.
     */
    private List<DungeonPoint> initializeOutsideBorder(GridList<WallState> walls, int wallRows, int wallColumns, WallState desiredBorderState)
    {
        // Create the first connected wall; all border cells are connected to
        // this.
        List<DungeonPoint> border = new List<DungeonPoint>();

        for (int row = 0; row < wallRows; row++)
        {
            for (int col = 0; col < wallColumns; col++)
            {
                // Set cells that are outside the grid to unavailable.
                if (row % 2 == 0 && col == (wallColumns - 1))
                {
                    // Even numbered row (horizontal borders) and last column
                    walls.setCell(row, col, WallState.UNAVAILABLE);
                }
                // North of Top Row
                else if (row == 0)
                {
                    // All of the top row's north are at row 2x(cell row=0) and
                    // column=cell column
                    walls.setCell(row, col, desiredBorderState);
                    border.Add(new DungeonPoint(row, col));
                }

                // South of Bottom Row
                else if (row == (wallRows - 1))
                {
                    // The last row, except for the const column.
                    walls.setCell(row, col, desiredBorderState);
                    border.Add(new DungeonPoint(row, col));
                }

                // West of First Column
                else if ((row % 2 == 1 && col == 0))
                {
                    // Odd number row (vertical borders) and first column
                    walls.setCell(row, col, desiredBorderState);
                    border.Add(new DungeonPoint(row, col));
                }

                // East of Last Column
                else if ((row % 2 == 1 && col == (wallColumns - 1)))
                {
                    // Odd number row (vertical borders) and last column
                    walls.setCell(row, col, desiredBorderState);
                    border.Add(new DungeonPoint(row, col));
                }
            }
        }
        // Be sure to add the border to the list of connected walls.
        return (border);
    }

    // TODO javadoc
    /**
     * @param walls
     * @param wallRows
     * @param wallColumns
     * @param desiredBorderState
     * @return
     */
    private List<DungeonPoint> setAvailableSelections(
            GridList<WallState> walls, int wallRows,
            int wallColumns, WallState desiredBorderState)
    {
        List<DungeonPoint> selectFrom = new List<DungeonPoint>();
        for (int row = 1; row < wallRows - 1; row++) // Not first or last
        {
            for (int col = 0; col < wallColumns - 1; col++) // Not right
            {
                if ((row % 2 == 0 || col != 0)) // Anything that isn't left.
                {
                    // Make sure it's open
                    walls.setCell(row, col, WallState.OPEN);
                    // Add it to the randomization list.
                    selectFrom.Add(new DungeonPoint(row, col));
                }
            }
        }
        return selectFrom;
    }

    /**
     * Make an array that contains all the neighboring walls of the given
     * dungeon point. This assumes the DungeonPoint is from an array where
     * horizontal walls are in the even column numbers and vertical walls are
     * int the odd column numbers.
     * 
     * @param dp
     *            The point to find neighbors of
     * @return An array of DungeonPoints, organized in 147963 or 789321
     *         for horizontal or vertical lines, respectively (as judged on a
     *         numpad).
     */
    private List<DungeonPoint> getNeighbors(DungeonPoint dp)
    {
        int row = dp.getX();
        int col = dp.getY();

        // Create a list of the neighboring DungeonPoints
        // Neighbors change based on whether this wall is horizontal or
        // vertical

        List<DungeonPoint> neighbors = new List<DungeonPoint>();

        if (col % 2 == 0) // Even column indicates horizontal lines
        {
            neighbors.Add(new DungeonPoint(row + 1, col)); // Down Left
            neighbors.Add(new DungeonPoint(row, col - 1)); // Left
            neighbors.Add(new DungeonPoint(row - 1, col)); // Up Left
            neighbors.Add(new DungeonPoint(row - 1, col + 1)); // Up Right
            neighbors.Add(new DungeonPoint(row, col + 1)); // Right
            neighbors.Add(new DungeonPoint(row + 1, col + 1)); // Down Right
        }
        else
        // Odd Y indicates vertical
        {
            neighbors.Add(new DungeonPoint(row - 1, col - 1)); // Up Left
            neighbors.Add(new DungeonPoint(row - 2, col)); // Up
            neighbors.Add(new DungeonPoint(row - 1, col - 1)); // Up Right
            neighbors.Add(new DungeonPoint(row + 1, col)); // Down Right
            neighbors.Add(new DungeonPoint(row + 2, col)); // Down
            neighbors.Add(new DungeonPoint(row + 1, col - 1)); // Down Left
        }

        return neighbors;
    }

    /**
     * Determine whether the currentWall is available for drawing a line.
     * 
     * @param currentWall
     *            The wall to check
     * @param neighbors
     *            A list of all neighbor walls
     * @param connects
     *            A double list containing all the current wall connections
     * @return true if the line should be drawn.
     */
    private bool shouldDrawLine(DungeonPoint currentWall,
            List<DungeonPoint> neighbors,
            List<List<DungeonPoint>> connects)
    {
        int left = getLeftConnectionIndex(neighbors, connects);
        int right = getRightConnectionIndex(neighbors, connects);

        if ((left != -1) && (left == right))
        {
            // The two sides are the same wall, so we don't want to close it
            // off.
            return false;
        }

        // Otherwise, it's ok to draw.
        return true;
    }

    /**
     * Get the index of the neighbors in the first three cells of the list.
     * 
     * @param neighbors
     *            The neighbors to check
     * @param connects
     *            The list of current connections
     * @return The index of the array containing the neighbor points or -1 if
     *         none are currently being used.
     */
    private int getLeftConnectionIndex(List<DungeonPoint> neighbors,
            List<List<DungeonPoint>> connects)
    {
        if (neighbors.Count == 6)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < connects.Count; j++)
                {
                    if (connects[j].Contains(neighbors[i]))
                    {
                        return j;
                    }
                }
            }
        }

        return -1;
    }

    /**
     * Get the index of the neighbors in the last three cells of the list.
     * 
     * @param neighbors
     *            The neighbors to check
     * @param connects
     *            The list of current connections
     * @return The index of the array containing the neighbor points or -1 if
     *         none are currently being used.
     */
    private int getRightConnectionIndex(List<DungeonPoint> neighbors,
            List<List<DungeonPoint>> connects)
    {
        if (neighbors.Count == 6)
        {
            for (int i = 3; i < 6; i++)
            {
                for (int j = 0; j < connects.Count; j++)
                {
                    if (connects[j].Contains(neighbors[i]))
                    {
                        return j;
                    }
                }
            }
        }

        return -1;
    }

    /**
     * Determine whether the currentWall is available for drawing a line.
     * 
     * @param currentWall
     *            The wall to check
     * @param neighbors
     *            A list of all neighbor walls
     * @param connects
     *            A double list containing all the current wall connections
     * @return true if the line should be drawn.
     */
    private void connectWalls(DungeonPoint currentWall,
            List<DungeonPoint> neighbors,
            List<List<DungeonPoint>> connects)
    {
        int left = getLeftConnectionIndex(neighbors, connects);
        int right = getRightConnectionIndex(neighbors, connects);

        if (left == -1 && right == -1)
        {
            // No neighboring walls. Add self to connects as a new array.
            List<DungeonPoint> anotherConnection = new List<DungeonPoint>();
            anotherConnection.Add(currentWall);
            connects.Add(anotherConnection);
        }

        else if (left == -1)
        {
            // Right side has a neighboring wall. Add self to that list.
            connects[right].Add(currentWall);
        }

        else if (right == -1)
        {
            // Left side has a neighboring wall. Add self to that list.
            connects[left].Add(currentWall);
        }

        else
        {
            // The two sides are in different lists. Combine the lists and
            // remove one of them.
            connects[left].AddRange(connects[right]);
            connects.RemoveAt(right);
        }
    }

    /**
     * The maze algorithm to determine separation between cells. This algorithm
     * assumes a perfect grid; if the <code>fractalize()</code> method of the
     * class doesn't create such a grid, this method MUST be overridden as well.
     * 
     * @param cells
     *            A double array containing all of this tile's cells in
     *            width-height order. Note that it is assumed that all rows
     *            in this array have the same size as
     *            <code>cells.get(0).Count</code> though this is not enforced.
     * @return The parameter, with walls updated according to this algorithm.
     */
    public virtual GridList<AbstractMapTile> createMaze(GridList<AbstractMapTile> cells)
    {
        // Make a two dimensional array. This array will be arranged in such
        // a manner: horizontal borders will be at even Y values; vertical
        // borders will be at odd Y (width) values. A horizontal line and
        // vertical line would share an X (height) value if their shared vertex
        // was at the upper-left corner.
        //
        // The following example has .'s to force proper spacing when Eclipse
        // saves. They have no meaning.
        //
        // ..0 1 2 3 4
        // 0 .- - - - -
        // 1 | | | | |
        // 2 .- - - - -
        //
        // Note that this makes an extra horizontal value for each of their
        // rows within the array. This code needs to expect and allow for that
        // occurrence. There is also one more row than twice the number of
        // cells.
        //

        // Set some convenience variables

        int _height = cells.getNumberOfRows();
        int wallRows = (_height * 2) + 1;

        int _width = cells.getNumberOfColumns();
        int wallColumns = _width + 1;

        GridList<WallState> walls = new GridList<WallState>(wallRows,
                wallColumns, WallState.OPEN);

        // Now that the array is initialized, step through, and make sure that
        // the border of the 'maze' is walled off. Place all other walls in
        // an availability array for random access.

        // Random Access Array
        List<DungeonPoint> selectFrom = new List<DungeonPoint>();

        // List of connected walls
        // This is not a GridList because it can have arrays of varying length.
        List<List<DungeonPoint>> connects = new List<List<DungeonPoint>>();

        // Set all of the outer borders to CLOSED
        connects.Add(initializeOutsideBorder(walls, wallRows, wallColumns,
                WallState.CLOSED));

        // Get all of the rest of them, setting them to OPEN on the way.
        selectFrom = this.setAvailableSelections(walls, wallRows, wallColumns,
                WallState.OPEN);

        // Now that initialization is done, make the maze.
        while (selectFrom.Count > 0)
        {
            // Get a random wall
            int randomNumber = Random.Range(0, selectFrom.Count);
            DungeonPoint currentWall = selectFrom[randomNumber];
            int row = currentWall.getX();
            int col = currentWall.getY();

            // Create a list of the neighboring DungeonPoints
            List<DungeonPoint> neighbors = getNeighbors(currentWall);

            if (shouldDrawLine(currentWall, neighbors, connects))
            {
                connectWalls(currentWall, neighbors, connects);
                // Set value of wall
                walls.setCell(row, col, WallState.CLOSED);
            }
            else
            {
                walls.setCell(row, col, WallState.UNAVAILABLE);
            }
            // Remove self from available list
            selectFrom.RemoveAt(randomNumber);

        } // While loop

        // Update the cell data to know what each wall state is.
        // System.out.println("Grid Size: " + cells.getNumberOfColumns()
        // + " columns by " + cells.getNumberOfRows() + " rows.");
        // System.out.println("Walls Size: " + walls.getNumberOfColumns()
        // + " columns by " + walls.getNumberOfRows() + " rows.");
        foreach (AbstractMapTile cell in cells)
        {
            int cellRow = cells.rowIndexOf(cell);
            int cellColumn = cells.columnIndexOf(cell, cellRow);

            // System.out.println("Setting walls for Cell at Coordinates Row "
            // + cellRow + ", Column " + cellColumn);
            // System.out.println("\tNorth: Row " + cellColumn + ", Column "
            // + (cellRow * 2));
            // System.out.println("\tWest: Row " + cellColumn + ", Column "
            // + ((cellRow * 2) + 1));
            // System.out.println("\tSouth: Row " + cellColumn + ", Column "
            // + ((cellRow * 2) + 2));
            // System.out.println("\tEast: Row " + (cellColumn + 1) +
            // ", Column "
            // + ((cellRow * 2) + 1));

            cell.setWallState(CardinalPoints.NORTH,
                    walls.getCell((cellRow * 2), cellColumn));
            cell.setWallState(CardinalPoints.WEST,
                    walls.getCell((cellRow * 2) + 1, cellColumn));
            cell.setWallState(CardinalPoints.SOUTH,
                    walls.getCell((cellRow * 2) + 2, cellColumn));
            cell.setWallState(CardinalPoints.EAST,
                    walls.getCell((cellRow * 2) + 1, cellColumn));

        }

        return cells;
    } // createMaze

    /**
     * By contract, this method is run after the makeFloorPlan() method, and
     * creates and establishes any overlays of the floor plan. This would
     * include such items as stalagmites, rough terrain, water, etc.
     * It should not include elevation, traps, or other such hazards.
     */
    public abstract void makeFeatures();

    /**
     * This method is what actually determines the layout of the dungeon within
     * this tile. This method is required for any tile.
     * 
     * @param exits
     *            The DungeonPoint set as detailed by the cell containing
     *            this tile
     */
    public abstract void makeFloorPlan();

    /**
     * Given a list of cells with their connections, determine exactly which
     * Dungeon Points are the connection between them. The default algorithm
     * assumes that the Cells are arranged in a perfect grid, though allows for
     * varied sized grids.
     * 
     * @param cells
     *            A list of cells that are connected in a maze fashion.
     * @return The parameter with all cells updated with Door coordinates.
     */
    public virtual GridList<AbstractMapTile> makeDoors(GridList<AbstractMapTile> cells)
    {
        return cells;
    }

    public virtual int getWidth()
    {
        return options[WIDTH].getOptionValue();
    }

    public virtual int getHeight()
    {
        return options[HEIGHT].getOptionValue();
    }

    /**
     * Perform the function needed to break down this tile into (potentially)
     * multiple tiles.
     * 
     * @return A grid of cells, or an empty grid if no more breaks can be made.
     */
    public abstract GridList<AbstractMapTile> fractalize();

    public virtual DungeonPoint getCoordinates()
    {
        return coordinate;
    }

    public virtual WallFeatures getFeatures(CardinalPoints direction)
    {
        return box[direction];
    }

    public virtual void setWallState(CardinalPoints direction, WallState state)
    {
        WallFeatures f = getFeatures(direction);
        f.state = state;
        box[direction] = f;
    }

    public virtual WallState getWallState(CardinalPoints direction)
    {
        return getFeatures(direction).state;
    }

    public virtual WallStyle getWallStyle(CardinalPoints direction)
    {
        return getFeatures(direction).style;
    }

    public virtual DoorType getDoor(CardinalPoints direction)
    {
        return getFeatures(direction).door;
    }

    public virtual bool isDoorLocked(CardinalPoints direction)
    {
        return getFeatures(direction).isLocked;
    }

    public virtual void open(CardinalPoints direction)
    {
        WallFeatures f = getFeatures(direction);
        f.state = WallState.OPEN;
        f.door = DoorType.OPEN;
        f.isLocked = false;
        box[direction] = f;
    }

    public virtual void close(CardinalPoints direction)
    {
        WallFeatures f = getFeatures(direction);
        f.state = WallState.CLOSED;
        f.style = WallStyle.WORKED;
        f.door = DoorType.NONE;
        f.isLocked = false;
        box[direction] = f;
    }

    public virtual void closeNatural(CardinalPoints direction)
    {
        WallFeatures f = getFeatures(direction);
        f.state = WallState.CLOSED;
        f.style = WallStyle.NATURAL;
        f.door = DoorType.NONE;
        f.isLocked = false;
        box[direction] = f;
    }

    public virtual bool setLocked(CardinalPoints direction)
    {
        WallFeatures f = getFeatures(direction);
        if (f.door != DoorType.VISIBLE || f.door != DoorType.SECRET)
        {
            return false;
        }
        f.isLocked = true;
        box[direction] = f;
        return true;
    }

    public virtual void unlock(CardinalPoints direction)
    {
        WallFeatures f = getFeatures(direction);
        f.isLocked = false;
        box[direction] = f;

    }

    public virtual void placeDoor(CardinalPoints direction)
    {
        placeDoor(direction, false);
    }

    public virtual void placeDoor(CardinalPoints direction, bool locked)
    {
        WallFeatures f = getFeatures(direction);
        f.door = DoorType.VISIBLE;
        f.isLocked = false;
        box[direction] = f;
    }

    public virtual void placeSecretDoor(CardinalPoints direction)
    {
        placeSecretDoor(direction, false);
    }

    public virtual void placeSecretDoor(CardinalPoints direction, bool locked)
    {
        WallFeatures f = getFeatures(direction);
        f.door = DoorType.SECRET;
        f.isLocked = false;
        box[direction] = f;
    }

    public virtual void passDownDoors(List<DoorData> _doors)
    {
        if (doors == null)
            doors = new List<DoorData>();

        foreach (DoorData dd in _doors)
        {
            int left = dd.recenterLocations(-coordinate.getX(),
                    -coordinate.getY(), getWidth(), getHeight());
            if (left > 0)
            {
                doors.Add(dd);
            }
            foreach (AbstractMapTile at in cells)
            {
                at.passDownDoors(doors);
            }
        }
    }
}

