using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ChoiceInfo
{
    private string classpath;
    private int weight;
    private Dictionary<string, TileOption> options;

    /**
     * @return the classpath
     */
    public string getClasspath()
    {
        return classpath;
    }

    /**
     * @param classpath
     *            the classpath to set
     */
    public void setClasspath(string classpath)
    {
        this.classpath = classpath;
    }

    /**
     * @return the weight
     */
    public int getWeight()
    {
        return weight;
    }

    /**
     * @param weight
     *            the weight to set
     */
    public void setWeight(int weight)
    {
        this.weight = weight;
    }

    /**
     * @return the options
     */
    public Dictionary<string, TileOption> getOptions()
    {
        return options;
    }

    /**
     * @param options
     *            the options to set
     */
    public void setOptions(Dictionary<string, TileOption> options)
    {
        this.options = options;
    }

    /**
     * Simple set option for the choice. This method allows the version
     * number to remain at its default.
     * 
     * @param option
     *            The name of the option to set
     * @param value
     *            The integer value of the option
     */
    public void setOption(string option, int value)
    {
        if (options == null)
        {
            options = new Dictionary<string, TileOption>();
        }

        options.Add(option, new TileOption(option, value));
    }

    /**
     * Set an option to the given TileOption. This method is the
     * easiest way to set an option with a version number.
     * 
     * @param option
     *            The name of the option to set
     * @param value
     *            The TileOption object that contains the option data
     */
    public void setOption(string option, TileOption value)
    {
        if (options == null)
        {
            options = new Dictionary<string, TileOption>();
        }
        options[option] = value;
    }

}
