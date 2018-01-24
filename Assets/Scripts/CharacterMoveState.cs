using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveState : MonoBehaviour {

    public void ToggleAttack()
    {
        GameData.toggleAttack();
    }

    public void ToggleMove()
    {
        GameData.toggleMove();
    }
}
