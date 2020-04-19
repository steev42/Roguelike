using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum SenseEnum
{
    BLIND,
    VISION,
    SCENT,
    SOUND,
    HEAT
}

public static class Sense
{
    public static SenseEnum GetSense(ISenseProperty prop)
    {
        if (prop is IlluminatedObject)
            return SenseEnum.VISION;

        else
            return SenseEnum.BLIND;
    }
}

