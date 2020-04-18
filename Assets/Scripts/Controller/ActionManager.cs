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
            EnqueueAction(a.GetInitiatingAI().GetNextAction());
        }
    }

    // Use this for initialization
    void Start()
    {
		
    }
	
    // Update is called once per frame
    void Update()
    {
        if (waiting)
        {
            PerformNextAction();
        }
    }
}
