using System;
using UnityEngine;

public class MoveAction : ICharacterAction
{
    private AI initiatingAI;
    //private CharacterData character;
    private Vector2 targetLocation;
    private long timeToMove;
    private long startingTick;

    public MoveAction(AI initiator, long timeToMove, CharacterData cd, Vector2 endMove)
    {
        initiatingAI = initiator;
        this.timeToMove = timeToMove;
        //character = cd;
        targetLocation = endMove;
    }

    #region ICharacterAction implementation

    public AI GetInitiatingAI()
    {
        return initiatingAI;
    }

    public void DoAction()
    {
        if (GameData.isValidMove(targetLocation))
        {
            initiatingAI.Character.UpdateLocation(targetLocation);
            //character.UpdateLocation(targetLocation);
            //character.obj.transform.position = targetLocation;
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

