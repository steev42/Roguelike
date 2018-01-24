using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDefinition
{


    private string name;
    private Dictionary<string, float> attackerMods = new Dictionary<string, float>();
    private Dictionary<string, float> defenderMods = new Dictionary<string, float>();
    private Dictionary<DamageTypes, RollData> baseDamage = new Dictionary<DamageTypes, RollData>();
    private Dictionary<DamageTypes, Dictionary<string, float>> damageOptimums = new Dictionary<DamageTypes, Dictionary<string, float>>();
    
    // TODO Weapon Classes as opposed to specific weapons,
    // to allow for sufficient randomization

    // TODO XML loading of Weapon Classes

    // TODO Ranged Weapons?

    public WeaponDefinition()
    {
        // Let's default to a normal longsword.
        name = "Longsword";
        // TODO Image?
        attackerMods[CharacterAttributes.STR] = 0.5f;
        attackerMods[CharacterAttributes.DEX] = 0.4f;
        attackerMods[CharacterAttributes.INT] = 0.1f;
        
        defenderMods[CharacterAttributes.STR] = 0.2f;
        defenderMods[CharacterAttributes.DEX] = 0.4f;
        defenderMods[CharacterAttributes.INT] = 0.4f;
        baseDamage[DamageTypes.SLASHING] = new RollData(1, 1, 8);
        Dictionary<string, float> slashingOptimum = new Dictionary<string, float>();
        slashingOptimum[CharacterAttributes.STR] = 10.0f;
        damageOptimums[DamageTypes.SLASHING] = slashingOptimum;        
    }

    public float GetAttackerAdjustedStats(IAttackableObject attacker) // TODO Should we make a different interface for things that CAN attack?
    {
        float total = 0f;
        foreach (string attr in attackerMods.Keys)
        {
            total += (attacker.GetAttribute(attr) * attackerMods[attr]);
        }
        return total;
    }

    public float GetDefenderAdjustedStats(IAttackableObject defender)
    {
        float total = 0f;
        foreach (string attr in defenderMods.Keys)
        {
            total += (defender.GetAttribute(attr, true) * defenderMods[attr]);
        }
        return total;
    }

    public bool DoAttack(IAttackableObject attacker, IAttackableObject defender)
    {
        // Figure out calculations for hit chance
        float attackerBase = GetAttackerAdjustedStats(attacker);
        float defenderBase = GetDefenderAdjustedStats(defender);
        float hitModRatio = attackerBase / defenderBase;

        float hitModLog = Mathf.Log(hitModRatio, GameData.HIT_DIMINISHMENT_FACTOR);
        float hitChance;

        if (hitModLog >= 0)
        {
            hitChance = 1 - ((1 - GameData.BASE_HIT_CHANCE) / (1 + hitModLog));
            Debug.Log("log > 0; hit chance = " + hitChance);
        }
        else
        {
            hitChance = (GameData.BASE_HIT_CHANCE) / (1 - hitModLog);
            Debug.Log("log < 0; hit chance = " + hitChance);
        }


        // Check for hit
        float roll = UnityEngine.Random.Range(0.0f, 1.0f); // TODO Central roll location to allow for seeding?
        Debug.Log("Roll: " + roll);
        if (roll <= hitChance)
        {
            Debug.Log("It's a HIT!");
            return true;
        }
        return false;
    }

    public Dictionary<DamageTypes, int> DoDamage(IAttackableObject attacker, IAttackableObject defender)
    {
        Dictionary<DamageTypes, int> damage = new Dictionary<DamageTypes, int>();
        foreach (DamageTypes dt in baseDamage.Keys)
        {
            float damageStatTotal = 0.0f;
            float damageMultiplier = 1.0f;
            float damageDivisor = 1.0f;

            if (damageOptimums.ContainsKey(dt))
            {
                foreach (string stat in damageOptimums[dt].Keys)
                {
                    float statOptimum = damageOptimums[dt][stat];
                    float characterStat = attacker.GetAttribute(stat);
                    //TODO The assumption here is that attributes will always be positive.
                    float ratio = characterStat / statOptimum;
                    damageStatTotal += ratio;
                }
                damageStatTotal /= damageOptimums[dt].Keys.Count; // Get the average ratio.
                Debug.Log("Damage Stat Ratio = " + damageStatTotal);
                if (damageStatTotal >= 1)
                {
                    damageMultiplier = 1 + Mathf.Log(damageStatTotal, GameData.DAMAGE_DIMINISHMENT_FACTOR);
                    Debug.Log("Damage Multiplier: " + damageMultiplier);
                }
                else
                {
                    damageDivisor = 1 + Mathf.Log(1 / damageStatTotal, GameData.DAMAGE_DIMINISHMENT_FACTOR);
                    Debug.Log("Damage Divisor: " + damageDivisor);
                }
            }

            damage[dt] = Mathf.FloorToInt(baseDamage[dt].Roll() * damageMultiplier / damageDivisor);

        }
        return damage;

        //TODO Apply damage to defender
    }

}