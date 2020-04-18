using System;
using UnityEngine;

public class MoveAction : ICharacterAction
{
    private AI initiatingAI;
    private CharacterData character;
    private Vector2 targetLocation;
    private long timeToMove;
    private long startingTick;
    private string description;

    public MoveAction(AI initiator, long timeToMove, CharacterData cd, Vector2 endMove, string desc = "")
    {
        initiatingAI = initiator;
        this.timeToMove = timeToMove;
        character = cd;
        targetLocation = endMove;
        description = desc;
    }

    #region ICharacterAction implementation

    public AI GetInitiatingAI()
    {
        return initiatingAI;
    }

    public void DoAction()
    {
        Debug.Log("Performing action : " + description + "; Move to " + targetLocation.ToString());
        if (true) //GameData.isValidMove(character, targetLocation))
        {
            character.UpdateLocation(targetLocation);
            Debug.Log("Character set to " + targetLocation.ToString());
            //character.obj.transform.position = targetLocation;
        }
        else
        {
            //Debug.Log("Unable to move to " + targetLocation + ".  Listed as not a valid move.");
        }
    }


    public long TimeToCompleteAction()
    {
        return timeToMove;
    }

    public void SetStartingTick(long tick)
    {
        startingTick = tick;
    }

    public long GetCompletionTick()
    {
        return startingTick + timeToMove;
    }


    #endregion
}

