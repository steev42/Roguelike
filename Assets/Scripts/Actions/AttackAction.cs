using System;
using UnityEngine;

public class AttackAction : ICharacterAction
{
    private AI initiatingAI;
    private CharacterData character;
    private Vector2 targetLocation;
    private long timeToMove;
    private long startingTick;

    public AttackAction(AI initiator, long timeToMove, CharacterData cd, Vector2 endMove)
    {
        initiatingAI = initiator;
        this.timeToMove = timeToMove;
        character = cd;
        targetLocation = endMove;
    }

    #region ICharacterAction implementation

    public AI GetInitiatingAI()
    {
        return initiatingAI;
    }

    public void DoAction()
    {
        Debug.Log("Attacking...");
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

