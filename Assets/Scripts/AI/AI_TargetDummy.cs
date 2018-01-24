using UnityEngine;
using System.Collections;

public class AI_TargetDummy : AI
{
    override public void SetActionWeights()
    {
        // Empty by design.  Target Dummies have no actions and do not need weighting.
    }

    override public ICharacterAction GetNextAction()
    {
        // Do absolutely nothing, ever.
        return new PauseAction(this, long.MaxValue); // Pause forever
    }
}
