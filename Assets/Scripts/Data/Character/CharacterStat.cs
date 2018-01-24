using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterStat
{
    public string statName;
    private float baseValue;
    private Dictionary<string, CharacterStat> adjustments;

    public CharacterStat (string name, float bv)
    {
        statName = name;
        baseValue = bv;
        adjustments = new Dictionary<string, CharacterStat>();
    }

    public float GetCurrentValue()
    {
        float currentValue = baseValue;
        foreach (CharacterStat s in adjustments.Values)
        {
            currentValue += s.GetCurrentValue();
        }

        return currentValue;
    }

    public void SetAttribute(float v)
    {
        baseValue = v;
    }

    public void AddAdjustment(string s, float v)
    {
        if (adjustments == null)
        {
            adjustments = new Dictionary<string, CharacterStat>();
        }

        adjustments[s] = new CharacterStat(s, v);
    }

    public void RemoveAdjustment(string s)
    {
        if (adjustments == null)
            return;
        if (adjustments.ContainsKey(s))
        {
            adjustments.Remove(s);
        }
    }
    
    public void AlterAdjustment(string s, float newValue)
    {
        if (adjustments == null)
        {
            adjustments = new Dictionary<string, CharacterStat>();
        }

        adjustments[s] = new CharacterStat(s, newValue);
    }
}
