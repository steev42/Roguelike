using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTileFactory
{
    private string className;
    private static Logger logger;
    private static MapTileFactory instance;

    private MapTileFactory()
    {
        className = this.GetType().ToString();
    }

    public static MapTileFactory getInstance()
    {
        if (instance == null)
        {
            instance = new MapTileFactory();
        }

        return instance;
    }

    public AbstractMapTile getTile(DungeonPoint dp)
    {
        // TODO
        return null;
    }

    /*
    public AbstractMapTile getTile(DungeonPoint dp)
    {
        AbstractMapTile at = null;

        if (setup != null)
        {
            ChoiceInfo choice = setup.getRandomSelection();
            if (choice != null)
            {
                string classpath = choice.getClasspath();
                Class <?> c = null;
                try
                {
                    c = Class.forName(classpath);
                }
                catch (ClassNotFoundException e)
                {
                    logger.log(Level.WARN, "Unable to find class " + classpath
                            + " - " + e);
                }

                logger.log(Level.DEBUG, "Found class...");
                if (c != null && AbstractMapTile.class.isAssignableFrom(c))
                {
                    logger.log(Level.DEBUG, "class is assignable");
                    Constructor<?>[] ctors = c.getDeclaredConstructors();
    Constructor<?> ctor = null;
                    for (int i = 0; i<ctors.length; i++)
                    {
                        ctor = ctors[i];

                        if (ctor.getGenericParameterTypes().length == 1
                                && ctor.getGenericParameterTypes()[0]
                                        .equals(DungeonPoint.class))
                        // Have found a constructor with a single
                        // DungeonPoint parameter.
                        {
                            logger.log(Level.TRACE, "Found a good constructor");
                            break;
                        }
                    }

                    try
                    {
                        logger.log(Level.TRACE, "Trying to get new instance");
                        at = (AbstractMapTile) ctor.newInstance(dp);
                    }
                    catch (InstantiationException | IllegalAccessException
                            | IllegalArgumentException
                            | InvocationTargetException e)
                    {
                        logger.log(Level.WARN,
                                "Unable to get a new instance of " + classpath
                                        + " - " + e);
                    }
                }
            }
        }
        return at;
    }*/
}
