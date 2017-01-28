using System;
using UnityEngine;

public abstract class AI : MonoBehaviour
{

    protected CharacterData aiFor;

    // This method determines the weighting of the characters actions.
    abstract public void SetActionWeights();

    // Actually determine what this character will do next.
    abstract public ICharacterAction GetNextAction();

    public CharacterData Character { get { return aiFor; } set { aiFor = value; } }

    public void Start()
    {
        // Setup our first move.  This should make it so that we are now queried when it's time to 
        // do our next move.
        GameObject.Find("GameManager").GetComponent<ActionManager>().EnqueueAction(GetNextAction());
    }
}


