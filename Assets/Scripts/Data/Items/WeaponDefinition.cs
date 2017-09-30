using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDefinition {

    private string name;
    private CharacterAttributes attackerRelevance;
    private CharacterAttributes defenderRelevance;
    private Dictionary<DamageTypes, RollData> baseDamage;
    private CharacterAttributes attackerOptimumAttributes;

    // TODO Weapon Classes as opposed to specific weapons,
    // to allow for sufficient randomization

    // TODO XML loading of Weapon Classes

    // TODO Ranged Weapons?

    public WeaponDefinition()
    {
        // Let's default to a normal longsword.
        name = "Longsword";
        // TODO Image?
        Dictionary<string, float> attackerMods = new Dictionary<string, float>();
        attackerMods[CharacterAttributes.STR] = 0.5f;
        attackerMods[CharacterAttributes.DEX] = 0.4f;
        attackerMods[CharacterAttributes.INT] = 0.1f;
        attackerRelevance = CharacterAttributes.ItemData(attackerMods);

        Dictionary<string, float> defenderMods = new Dictionary<string, float>();
        defenderMods[CharacterAttributes.STR] = 0.2f;
        defenderMods[CharacterAttributes.DEX] = 0.4f;
        defenderMods[CharacterAttributes.INT] = 0.4f;
        defenderRelevance = CharacterAttributes.ItemData(defenderMods);

        baseDamage = new Dictionary<DamageTypes, RollData>();
        baseDamage[DamageTypes.SLASHING] = new RollData(1, 1, 8);

        Dictionary<string, float> damageOptimums = new Dictionary<string, float>();
        damageOptimums[CharacterAttributes.STR] = 10;
        attackerOptimumAttributes = CharacterAttributes.ItemData(damageOptimums);
    }

}
