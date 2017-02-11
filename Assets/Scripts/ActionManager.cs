using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

    private ActionTree initiativeTracker;
    private long currentTick = 0;
    public bool waiting = true;

    public void EnqueueAction(ICharacterAction a)
    {
        a.SetStartingTick(currentTick);

        if (initiativeTracker == null)
        {
            initiativeTracker = new ActionTree(a);
        }
        else
        {
            initiativeTracker.EnqueueAction(a);
        }


        //PerformNextAction ();
    }

    private void PerformNextAction()
    {

        // This check is not truly needed, since the Update() method calls the GetNextActor,
        // which checks for valid initativeTracker.  Kept to double-check.
        if (initiativeTracker == null)
        {
            return;
        }
        else
        {
            waiting = false;
            ICharacterAction a = initiativeTracker.DequeueAction(null);
            if (a != null)
            {
                a.DoAction();
                currentTick = a.GetCompletionTick();
                if (!(a is PauseAction))
                {
                    Debug.Log("Action" + a.GetType() + " completed at tick " + currentTick);
                }
            }
            waiting = true;
        }
    }

    private AI GetNextActor()
    {
        if (initiativeTracker == null)
        {
            return null;
        }
        else
        {
            return initiativeTracker.PeekNextAction().GetInitiatingAI();
        }
    }

    // Use this for initialization
    void Start()
    {
		
    }
	
    // Update is called once per frame
    void Update()
    {
        // waiting for the next action to be performed.  This semaphore prevents multiple actions
        // from occurring at the same time.
        if (waiting)
        {
            AI actor = GetNextActor();
            if (actor == null)
                return; // No action to perform.

            if (actor is AI_Player)
            {
                GameData.SetActiveCharacter(actor.Character);
            }

            PerformNextAction();
            EnqueueAction(actor.GetNextAction());
        }
    }
}
