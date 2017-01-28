using System;

public class PauseAction : ICharacterAction
{
    private AI initiatingAI;
    private long duration;
    private long startingTick;

    public PauseAction(AI initiator, long numberOfTicks)
    {
        initiatingAI = initiator;
        duration = numberOfTicks;
    }

    #region ICharacterAction implementation

    public AI GetInitiatingAI()
    {
        return initiatingAI;
    }

    public void DoAction()
    {
        // Do Nothing.
    }

    public long TimeToCompleteAction()
    {
        return duration;
    }


    public void SetStartingTick(long tick)
    {
        startingTick = tick;
    }

    public long GetCompletionTick()
    {
        return startingTick + duration;
    }

    #endregion
}


