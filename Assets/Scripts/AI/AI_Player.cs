﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Player : AI
{
    bool isAlreadyMoving = false;
    private Vector2 moveVector = Vector2.zero;

    override public void SetActionWeights()
    {
        // Empty by design.  Player actions do not need weighting.
    }

    override public ICharacterAction GetNextAction()
    {
        if (moveVector.Equals(Vector2.zero))
        {
            return new PauseAction(this, 0);
        }

        if (GameData.isValidAttack(Character.location + moveVector))
        {
            AttackAction a = new AttackAction(this, 15, aiFor, Character.location + moveVector);
            moveVector = Vector2.zero;
            return a;
        }
        else if (GameData.isValidMove(Character, Character.location + moveVector))
        {
            MoveAction a = new MoveAction(this, 10, aiFor, Character.location + moveVector);
            moveVector = Vector2.zero; // reset the vector
            return a;
        }
        else
        {
            //Debug.Log("Attempting an invalid move.");
            moveVector = Vector2.zero; // Reset, since it must be an invalid move.
            return new PauseAction(this, 0);
        }
    }

    public void Update()
    {
        if (moveVector == Vector2.zero)
        {
            int horizontal = (int)Input.GetAxisRaw("Horizontal");
            int vertical = (int)Input.GetAxisRaw("Vertical");

            if (horizontal == 0 && vertical == 0)
            {
                isAlreadyMoving = false;
            }
            else
            {
                if (isAlreadyMoving == false)
                {
                    // This combination of if statements makes it so that you have to let go of the buttons before a second move
                    // will be enqueued.
                    moveVector = new Vector2(horizontal, vertical);
                    isAlreadyMoving = true;
                }
            }
        }
    }
}
