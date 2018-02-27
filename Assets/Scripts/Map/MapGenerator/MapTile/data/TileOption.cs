using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TileOption
{

    private string optionName;
    private int optionValue;

    /**
     * @return the optionName
     */
    public string getOptionName()
    {
        return optionName;
    }

    /**
     * @param optionName
     *            the optionName to set
     */
    public void setOptionName(string optionName)
    {
        this.optionName = optionName;
    }

    /**
     * @return the optionValue
     */
    public int getOptionValue()
    {
        return optionValue;
    }

    /**
     * @param optionValue
     *            the optionValue to set
     */
    public void setOptionValue(int optionValue)
    {
        this.optionValue = optionValue;
    }

    public TileOption(string optionName, int optionValue)
    {
        this.optionName = optionName;
        this.optionValue = optionValue;
    }

}
