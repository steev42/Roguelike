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
        int total = 0;
        for (int i = 1; i <= numberOfRolls; i++)
        {
            int roll = Random.Range(minRoll, maxRoll);
            Debug.Log("Roll " + i + ": " + roll);
            total += roll;
        }
        return total;
    }


}
