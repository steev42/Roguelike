using System;

public interface ICharacterAction
{
    void DoAction();

    long TimeToCompleteAction();

    void SetStartingTick(long tick);

    long GetCompletionTick();

    AI GetInitiatingAI();
}

