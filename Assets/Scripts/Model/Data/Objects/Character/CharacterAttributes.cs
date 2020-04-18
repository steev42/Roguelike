using UnityEngine;
using System.Collections.Generic;

public class CharacterAttributes
{
    public const string MIN_LIGHT_FOR_SIGHT = "Minimum Light to See";

    public const string STR = "Strength";
    public const string DEX = "Dexterity";
    public const string CON = "Constitution";
    public const string INT = "Intelligence";
    public const string WIS = "Wisdom";
    public const string CHA = "Charisma";

    public const string HP = "Hit Points";
    public const string DAMAGE = "Current Damage";

    private string[] attributeArray = { STR, DEX, CON, INT, WIS, CHA };

    private Dictionary<string, CharacterStat> stats;

    //TODO Have some sort of XML/Modding method of establishing the baseline of stats?

    public static CharacterAttributes PCAttributes()
    {
        return new CharacterAttributes(true);
    }

    public static CharacterAttributes ItemData(Dictionary<string, float> values)
    {
        CharacterAttributes ca = new CharacterAttributes();
        foreach (string s in values.Keys)
        {
            ca.SetAttribute(s, values[s]);
        }

        return ca;

    }
    private CharacterAttributes()
    {
        stats = new Dictionary<string, CharacterStat>();
    }

    private CharacterAttributes(bool pc)
    {

        stats = new Dictionary<string, CharacterStat>();
        if (pc)
        {
            stats[MIN_LIGHT_FOR_SIGHT] = new CharacterStat(MIN_LIGHT_FOR_SIGHT, 1.0f);

            foreach (string s in attributeArray)
            {
                stats[s] = RandomizeAttribute(s);
            }

            stats[HP] = new CharacterStat(HP, 30); // TODO Randomize?
            stats[DAMAGE] = new CharacterStat(DAMAGE, 0);
        }
    }

    public void SetAttribute(string s, float v)
    {
        if (stats == null)
        {
            stats = new Dictionary<string, CharacterStat>();
        }
        if (stats.ContainsKey(s) == false)
            stats[s] = new CharacterStat(s, v);
        else
            stats[s].SetAttribute(v);
    }

    private CharacterStat RandomizeAttribute(string s)
    {
        CharacterStat stat = new CharacterStat(s, 10); // TODO Actually randomize, don't just set to 10.
        return stat;
    }

    public float GetAttribute(string s)
    {
        if (stats != null && stats.ContainsKey(s))
        {
            return stats[s].GetCurrentValue();
        }
        else
        {
            return -1; // TODO is the proper value?  Are we sure all attributes will be positive?
            // In practice, should probably instead throw an exception.
        }
    }

    public int GetAttributeInteger(string s)
    {
        float attr = GetAttribute(s);
        if (Mathf.RoundToInt(attr) != attr)
            Debug.LogWarning("Returning an Float-Precision Attribute (" + s + ") as an Integer");
        return (int)(GetAttribute(s));
    }

}
