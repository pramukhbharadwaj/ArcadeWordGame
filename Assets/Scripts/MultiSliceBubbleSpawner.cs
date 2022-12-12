using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using static System.Math;

public class MultiSliceBubbleSpawner : MonoBehaviour
{
    public Material specialBubbleMaterial;
    private Collider spawnArea;
    public GameObject[] bubblePrefabs;
    public GameObject[] specialBubblePrefabs;
    public Color[] colors;

// stores indexes(in the form '<Correct Letter>' - 'A' : indexed(0-25)) of correct letters that are preferred for spawning.
    public List<int> preferredAlphabetIndexes = new List<int>();

// stores indexes(in the form '<InCorrect Letter>' - 'A' : indexed(0-25)) of incorrect letters that are NOT preferred for spawning.
    public List<int> incorrectLetterList = new List<int>();

    public float spawnDelay;
    
    public float minAngle = -3f;
    public float maxAngle = 3f;
    
    public float minForce = 10;
    public float maxForce = 20;

    public float maxLifetime = 5f; //Increased max life time to accomodate for the freeing

    public string givenWord ="";
    public string prevWord ="";
    
    public bool isFrozen = false;
    
    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void CoroutineSpawn(float initialDelayMultiSliceSpawner, float minSpawnDelayMultiSliceSpawner, float maxSpawnDelayMultiSliceSpawner)
    {
        StartCoroutine(Spawn(initialDelayMultiSliceSpawner, minSpawnDelayMultiSliceSpawner, maxSpawnDelayMultiSliceSpawner));
    }

    
    public void SetInitialWordArrayLists(char[] wordAlphabets, int wordLen)
    {

         for(int i=0;i<wordLen;i++)
        {
            preferredAlphabetIndexes.Add(wordAlphabets[i] - 'A');
        }

        for(int i=0;i<26;i++)
        {
            if(!preferredAlphabetIndexes.Contains(i))
            {
                incorrectLetterList.Add(i);
            }
        }
    }

// randomly choose from [18,24] for the number of bubbles to pop together in current spawn
    public int GetNumBubblesToSpawn()
    {
        int numBubblesInOneSpawn = 1;
        return numBubblesInOneSpawn;
    }


// set physics aspects for each bubble
    public GameObject InstantiateNewBubble(GameObject prefab)
    {
        Vector3 position = new Vector3();
         int mid = (int)(spawnArea.bounds.min.x + spawnArea.bounds.max.x) / 2;
        position.x = Random.Range(mid,mid);
        position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

        // rotation of the spawner
        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
        
        GameObject bubble = Instantiate(prefab, position, rotation);
        
        Destroy(bubble, maxLifetime);

        // float force = Random.Range(minForce, maxForce);
        bubble.GetComponent<Rigidbody>().AddForce(bubble.transform.up * minForce, ForceMode.Impulse);

        return bubble;
    }

// spawns n correct/required bubbles
    public void SpawnNCorrectBubbles(int n)
    {
        GameObject prefab;
        int indexOfSpecialBubble = 26;
        // prefab = specialBubblePrefabs[indexOfSpecialBubble];

        int len = preferredAlphabetIndexes.Count;
        int letterIndex = Random.Range(0, len);
        prefab = specialBubblePrefabs[preferredAlphabetIndexes[letterIndex]];
        string letterName = specialBubblePrefabs[preferredAlphabetIndexes[letterIndex]].name;
        //Debug.Log(letterIndex + " " +letterName);

        GameObject bubble = InstantiateNewBubble(prefab);
        bubble.GetComponent<Renderer>().material.color = colors[0];

        SpawnManager.numOfSpecialBubblesSpawned++;

        return;
    }

// spawns n incorrect/wrong bubbles
    public void SpawnNIncorrectBubbles(int n)
    {
        GameObject prefab;
        for(int i=0;i<n;i++)
        {
            prefab = bubblePrefabs[incorrectLetterList[Random.Range(0, incorrectLetterList.Count)]];
            GameObject bubble = InstantiateNewBubble(prefab);
        }
        return;
    }


    public void SpawnBubblesHelper(int numCorrectBubbles, int numIncorrectBubbles)
    {
        SpawnNCorrectBubbles(numCorrectBubbles);
        SpawnNIncorrectBubbles(numIncorrectBubbles);
    }


    public void SpawnBubbles(int numBubblesInOneSpawn)
    {
        float incorrectBubblesPercentage = 0f;

        // if total number of bubbles spawning is 3, all must be correct, else atmost 2 should be incorrect.
        int numIncorrectBubbles = (int)(numBubblesInOneSpawn * incorrectBubblesPercentage);
        int numCorrectBubbles = numBubblesInOneSpawn - numIncorrectBubbles;
        
        SpawnBubblesHelper(numCorrectBubbles, numIncorrectBubbles);

    }


    public IEnumerator Spawn(float initialDelayMultiSliceSpawner, float minSpawnDelayMultiSliceSpawner, float maxSpawnDelayMultiSliceSpawner)
    {
        givenWord = FindObjectOfType<GameManager>().initialText.text;

        yield return new WaitForSeconds(initialDelayMultiSliceSpawner); 
        int wordLen = givenWord.Length;

    // array of correct letters.
        char[] wordAlphabets = givenWord.ToCharArray();

        SetInitialWordArrayLists(wordAlphabets, wordLen);        
        
        while (enabled)
        {
            float spawnDelay = Random.Range(minSpawnDelayMultiSliceSpawner, maxSpawnDelayMultiSliceSpawner);
            yield return new WaitForSeconds(spawnDelay);
            int numBubblesInOneSpawn = GetNumBubblesToSpawn();
            SpawnBubbles(numBubblesInOneSpawn);
        }
    }


// Remove letter from list of high probability spawn letters.
     public void RemoveLetter(string letter){
        for(int i=0; i<preferredAlphabetIndexes.Count;i++){

            if(((char)('A' + preferredAlphabetIndexes[i])).ToString() == letter){
                preferredAlphabetIndexes.RemoveAt(i);
                break;
            }
        }
    }
     

     public void UpdateWord(string newWord){
         
         givenWord = newWord;
         int wordLen = givenWord.Length;
         preferredAlphabetIndexes.Clear();
         char[] wordAlphabets = givenWord.ToCharArray();
         for(int i=0;i<wordLen;i++)
         {
            preferredAlphabetIndexes.Add(wordAlphabets[i] - 'A');
         }
         for(int i=0;i<26;i++)
         {
            if(!preferredAlphabetIndexes.Contains(i))
            {
                incorrectLetterList.Add(i);
            }
         }
         return;
         
     }


     public Color[] GetColors()
     {
         return colors;
     }

}
