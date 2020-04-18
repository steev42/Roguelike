using System;

public class LevelFactory
{
    private static LevelFactory instance = null;
    public enum LevelType { TEST_BLANK, TEST_SMALL, ENTRY, CAVERN }; //TODO This is NOT the way to do this, but for now, will suffice.
    private LevelFactory()
    {
        
    }

    public static LevelFactory getInstance()
    {
        if (instance == null)
        {
            instance = new LevelFactory();
        }

        return instance;
    }

    public AbstractMapLevel getLevel()
    {
        //Current Options - 
        //  Empty Square            [MapLoaderImage]
        //  Test Entry Location     [MapLoaderImage]
        //  Small Test Map          [MapLoaderImage]
        //  Random Cavernous System [JavaImport]


        return null;
    }

    public AbstractMapLevel getLevel(LevelType lt)
    {
        AbstractMapLevel theLevel;
        switch (lt)
        {
            case LevelType.TEST_BLANK:
                theLevel = new BitImageMapLevel();
                BitImageMapLevel.BitImageParameters blankParams = new BitImageMapLevel.BitImageParameters();
                blankParams.resourceName = "emptysquare";
                theLevel.SetParameters(blankParams);
                theLevel.GenerateLevel();
                break;
            case LevelType.TEST_SMALL:
                theLevel = new BitImageMapLevel();
                BitImageMapLevel.BitImageParameters testParams = new BitImageMapLevel.BitImageParameters();
                testParams.resourceName = "testmap";
                theLevel.SetParameters(testParams);
                theLevel.GenerateLevel();
                break;
            case LevelType.ENTRY:
                theLevel = new BitImageMapLevel();
                BitImageMapLevel.BitImageParameters entryParams = new BitImageMapLevel.BitImageParameters();
                entryParams.resourceName = "entry";
                theLevel.SetParameters(entryParams);
                theLevel.GenerateLevel();
                break;
            case LevelType.CAVERN:
                theLevel = null;
                break;
            default:
                throw new NotImplementedException("Unknown level type");
        }
        return theLevel;
    }
}