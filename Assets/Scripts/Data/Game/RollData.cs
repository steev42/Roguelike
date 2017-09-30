using UnityEngine;
using System.Collections;

public class RollData
{
    private int minRoll;
    private int maxRoll;
    private int numberOfRolls;

    public RollData (int num, int min, int max)
    {
        numberOfRolls = num;
        minRoll = min;
        maxRoll = max;
    }

    public int Roll()
    {
        // TODO
        return 0;
    }


}
