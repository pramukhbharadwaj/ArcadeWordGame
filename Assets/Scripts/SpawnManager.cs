using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class SpawnManager
{
    public static int numOfBubblesSpawned { get; set;} = 0;

    public static int numOfCorrectBubblesSpawned { get; set;} = 0;

    public static int numOfCorrectBubblesSliced { get; set;} = 0;

    public static int preferredBubbleSpawnColor { get; set;} = 0;

    public static int numOfSpecialBubblesSpawned { get; set;} = 0;

    public static int numOfSpecialBubblesSliced { get; set;} = 0;

    public static int numOfCorrectColoredCorrectCharacterBubblesSliced { get; set;} = 0;

    public static int numOfIncorrecColoredCorrectCharacterBubblesSliced { get; set;} = 0;
    
    public static int numOfSlicesOnObstacle { get; set;} = 0;
    
    public static int totalNumOfSlices { get; set;} = 0;

    public static List<int> incorrectColorList = new List<int>();



    /*
    *   TO MAKE BELOW SPAWN DELAYS LEVEL SPECIFIC.
    *   Go to GameManager.cs, search for initializeAndSpawn() call in the level you want the change in
    *   Set SpawnManager.<min/max>SpawnDelay<type of spawner> = <Value>
    */

    // spawn delays basic spawner
    public static float initialDelayBasicSpawner = 0.5F, spawnDelayBasicSpawner = 2F;

    // spawn delays multislice spawner
    public static float initialDelayMultiSliceSpawner = 1F, 
            minSpawnDelayMultiSliceSpawner = 3F, 
            maxSpawnDelayMultiSliceSpawner = 5F;

    // spawn delays burst spawner
    public static float initialDelayBurstSpawner = 1F, 
            minSpawnDelayBurstSpawner = 15F, 
            maxSpawnDelayBurstSpawner = 20F;

    // spawn delays color spawner
    public static float initialDelayColorSpawner = 1F, 
            spawnDelayColorSpawner = 2F;

    // spawn delays obstacle spawner
    public static float initialDelayObstacleSpawner = 10F, 
            minSpawnDelayObstacleSpawner, 
            maxSpawnDelayObstacleSpawner;
        
    // spawn delays burstcolor spawner
    public static float initialDelayBurstColorSpawner = 2F, 
            minSpawnDelayBurstColorSpawner = 15F, 
            maxSpawnDelayBurstColorSpawner = 20F;
    



    public static void SetSpawnColorPreferences(int currColorInd)
    {
        preferredBubbleSpawnColor = currColorInd;
        Color[] colors = GameObject.FindObjectOfType<BasicSpawner>().GetColors();
        for(int i=0;i<colors.Length;i++)
        {
            if(i!=preferredBubbleSpawnColor)
                incorrectColorList.Add(i);
        }
        BasicSpawner.incorrectColorList = incorrectColorList;
        return;
    }

    public static void Clear()
    {
        numOfBubblesSpawned = 0;
        numOfCorrectBubblesSpawned = 0;
        numOfCorrectBubblesSliced = 0;
        numOfSpecialBubblesSpawned = 0;
        numOfSpecialBubblesSliced = 0;
        numOfCorrectColoredCorrectCharacterBubblesSliced = 0;
        numOfIncorrecColoredCorrectCharacterBubblesSliced = 0;
        numOfSlicesOnObstacle = 0;
        totalNumOfSlices = 0;
    }

    public static void initializeAndSpawn(bool activateBasicSpawner,
                                        bool activateMultiSliceBubbleSpawner,
                                        bool activateBurstSpawner,
                                        bool activateColorSpawner,
                                        bool activateTutorialSpawner, 
                                        bool activateObstacleSpawner, 
                                        bool activateBurstColorSpawner
                                        )
    {
        if(activateBasicSpawner)
        {
            GameObject.FindObjectOfType<BasicSpawner>().CoroutineSpawn(initialDelayBasicSpawner, spawnDelayBasicSpawner);
        }
        if(activateMultiSliceBubbleSpawner)
        {
            GameObject.FindObjectOfType<MultiSliceBubbleSpawner>().CoroutineSpawn(initialDelayMultiSliceSpawner, minSpawnDelayMultiSliceSpawner, maxSpawnDelayMultiSliceSpawner);
        }
        if(activateBurstSpawner)
        {
            GameObject.FindObjectOfType<BurstSpawner>().CoroutineSpawn(initialDelayBurstSpawner, minSpawnDelayBurstSpawner, maxSpawnDelayBurstSpawner);
        }
        if(activateColorSpawner)
        {
            GameObject.FindObjectOfType<ColorSpawner>().CoroutineSpawn(initialDelayColorSpawner, spawnDelayColorSpawner);
        }
        if(activateTutorialSpawner)
        {
            GameObject.FindObjectOfType<TutorialSpawner>().CoroutineSpawn();
        }
        if (activateObstacleSpawner)
        {
            GameObject.FindObjectOfType<ObstacleSpawner>().CoroutineSpawn(initialDelayObstacleSpawner, minSpawnDelayObstacleSpawner, maxSpawnDelayObstacleSpawner);
        }
        if(activateBurstColorSpawner)
        {
            GameObject.FindObjectOfType<BurstColorSpawner>().CoroutineSpawn(initialDelayBurstColorSpawner, minSpawnDelayBurstColorSpawner, maxSpawnDelayBurstColorSpawner);
        }
    }

    public static void initializeAndSpawnCannonLevel()
    {
        GameObject.FindObjectOfType<LeftSpawner>().CoroutineSpawn(1.5f, 2.5f);
        GameObject.FindObjectOfType<RightSpawner>().CoroutineSpawn(2.2f, 3.5f);
        
    }
    public static void initializeAndSpawnCannonTutorialLevel()
    {
        GameObject.FindObjectOfType<CannonTutorialSpawner>().CoroutineSpawn(2.5f, 4f);

    }
}
