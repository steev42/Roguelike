using System;
using System.Collections.Generic;
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
        /* Steps needed to complete this action:
         * 1. Determine target -- What can be attacked that is in the target square?
         * 2. Determine attackers weapon -- What weapon did they use?  This should probably be a parameter, as attacker can have multiple weapons.
         * 3. Determine targets' armor -- what armor are they wearing?
         * 4. Do calculations for hit chance
         * 5. Check if hit was successful
         * 6. If hit, Do calculations for damage(s)
         * 7. Determine final damage
         * 8. Do any character updates needed (damage/healing/etc.)
         */

        // Get target
        IAttackableObject defender = GameData.getTileDefender(targetLocation);

        // Get weapon
        WeaponDefinition weapon = new WeaponDefinition(); // TODO This is only a placeholder for the time being.
        if (weapon.DoAttack(character, defender))
        {
            Dictionary<DamageTypes, int> damage = weapon.DoDamage(character, defender);
            foreach (DamageTypes dt in damage.Keys)
            {
                Debug.Log("DAMAGE: " + damage[dt] + " " + dt.ToString());
            }
        }

        

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

