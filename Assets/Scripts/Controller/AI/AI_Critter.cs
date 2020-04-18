using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Critter : AI
{

    override public void  SetActionWeights()
    {
        throw new System.NotImplementedException();
    }

    override public ICharacterAction GetNextAction()
    {
        // Figure out which of 8 directions are available.  Then choose one to move to at random.
        List<Vector2> directions = new List<Vector2>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2 pos = new Vector2(x, y);
                if (pos.Equals(Vector2.zero))
                {
                    continue; // A no-move, don't consider it.   
                }

               /* if (GameData.isValidMove(Character, Character.location + pos))
                {
                    directions.Add(pos);
                }*/
            }
        }

        if (directions.Count > 0)
        {
            int choice = Random.Range(0, directions.Count);
            //Debug.Log("Critter moving in " + directions[choice].ToString());
            return new MoveAction(this, 9, Character, Character.location + directions[choice]);
        }

        //Got this far, but there are no valid moves.  Something has gone wrong...
        Debug.LogWarning("CritterAI: No Valid Moves found.");
        return new PauseAction(this, long.MaxValue); // Pause forever
    }
		
}
