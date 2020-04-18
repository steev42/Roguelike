using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTree
{

    private ICharacterAction root;
    private ActionTree leftNode;
    private ActionTree rightNode;

    public ActionTree(ICharacterAction action)
    {
        root = action;
        leftNode = null;
        rightNode = null;
    }

    public ICharacterAction DequeueAction(ActionTree parent)
    {
        ICharacterAction rv;

        if (leftNode != null)
        {
            rv = leftNode.DequeueAction(this);
        }
        else
        {
            rv = root;
            if (parent != null)
            {
                // There should now be nothing to my left.  Remove myself from the equation,
                // and make my parent look to my right from now on.
                parent.leftNode = rightNode;
            }
            else if (rightNode != null)
            {
                // Replace self with minimum action of the right node, removing it in the process.
                root = rightNode.DequeueAction(null);
            }
            else
            {	
                root = null; // IS THIS CORRECT?
            }
        }

        return rv;
    }

    public ICharacterAction PeekNextAction()
    {
        if (leftNode != null)
        {
            return leftNode.PeekNextAction();
        }
        else
        {
            return root;
        }
    }

    public List<ICharacterAction> GetActionQueue()
    {
        if (root == null)
        {
            return null;
        }

        List<ICharacterAction> queue = new	List<ICharacterAction>();
        if (leftNode != null)
        {
            queue.AddRange(leftNode.GetActionQueue());
        }
        queue.Add(root);
        if (rightNode != null)
        {
            queue.AddRange(rightNode.GetActionQueue());
        }

        return queue;
    }

    public void EnqueueAction(ICharacterAction action)
    {
        // This case should only happen IF all other actions
        // have been dequeued.
        if (root == null)
        {
            root = action;
            return;
        }
            
        // NOTE: The = means that for actions completing on the same tick,
        // actions enqueued later wil complete first.  This makes sense because
        // those actions would logically be faster, so making them complete first
        // is consistent.

        if (action.GetCompletionTick() <= root.GetCompletionTick())
        {
            if (leftNode == null)
            {
                leftNode = new ActionTree(action);
            }
            else
            {
                leftNode.EnqueueAction(action);
            }
        }
        else
        {
            if (rightNode == null)
            {
                rightNode = new ActionTree(action);
            }
            else
            {
                rightNode.EnqueueAction(action);
            }
        }
    }

}
