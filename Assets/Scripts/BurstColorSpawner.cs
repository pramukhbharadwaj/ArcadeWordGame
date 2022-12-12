using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using static System.Math;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class BurstColorSpawner : MonoBehaviour
{

     private Collider spawnArea;
    public GameObject[] bubblePrefabs;
    public Color[] colors;

// stores indexes(in the form '<Correct Letter>' - 'A' : indexed(0-25)) of correct letters that are preferred for spawning.
    public List<int> preferredAlphabetIndexes = new List<int>();

// stores indexes(in the form '<InCorrect Letter>' - 'A' : indexed(0-25)) of incorrect letters that are NOT preferred for spawning.
    public List<int> incorrectLetterList = new List<int>();

// List to choose the number of bubbles to spawn in one go.
    public List<int> listNumBubblesInOneSpawn = new List<int>{ 3, 4, 5};

    // public float spawnDelay;
    
    public float minAngle = 0f;
    public float maxAngle = 0f;
    
    public float minForce = 18;
    public float maxForce = 20;

    public float maxLifetime = 5f; //Increased max life time to accomodate for the freeing

    public string givenWord ="";
    public string prevWord ="";
    
    private List<GameObject> existingBubbles = new List<GameObject>(); //Stores existing bubbles and their current velocity

    private bool isFrozen = false;
    
    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void CoroutineSpawn(float initialDelayBurstColorSpawner, float minSpawnDelayBurstColorSpawner, float maxSpawnDelayBurstColorSpawner)
    {
        StartCoroutine(Spawn(initialDelayBurstColorSpawner, minSpawnDelayBurstColorSpawner, maxSpawnDelayBurstColorSpawner));
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
        int numBubblesInOneSpawn = Random.Range(12, 18);
        return numBubblesInOneSpawn;
    }


// set physics aspects for each bubble
    public GameObject InstantiateNewBubble(GameObject prefab, int randColorIndex)
    {
        Vector3 position = new Vector3();
        position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

        // rotation of the spawner
        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
        
        GameObject bubble = Instantiate(prefab, position, rotation);
        bubble.GetComponent<Renderer>().material.color = colors[randColorIndex];
        SpawnManager.numOfBubblesSpawned++;

        Destroy(bubble, maxLifetime);

        float force = Random.Range(minForce, maxForce);
        bubble.GetComponent<Rigidbody>().AddForce(bubble.transform.up * force, ForceMode.Impulse);

        return bubble;
    }

// spawns n correct/required bubbles
    public void SpawnNCorrectBubbles(int n)
    {
        List<int> colorCount = new List<int>();
        colorCount.Add((int)(n * 0.5f));
        colorCount.Add((int)(n * 0.3f));
        colorCount.Add(n - colorCount[0] - colorCount[1]);
        
        GameObject prefab;
        int colorIndex = 0;
        int numColors = GetColors().Length;
        int randColorIndex = Random.Range(0, numColors);
        
        for(int i=0;i<Max(n, preferredAlphabetIndexes.Count);i++)
        {
            if (colorCount[colorIndex] == 0)
            {
                randColorIndex = Random.Range(0, numColors);
                colorIndex++;
            }
                
            
            if(i==n) break;
            colorCount[colorIndex]--;
            // Debug.Log("i: " + i);
            // Debug.Log(preferredAlphabetIndexes + " " + i%preferredAlphabetIndexes.Count + " " + preferredAlphabetIndexes[i % preferredAlphabetIndexes.Count]);
            prefab = bubblePrefabs[preferredAlphabetIndexes[i % preferredAlphabetIndexes.Count]];
            GameObject bubble = InstantiateNewBubble(prefab, randColorIndex);
            //Debug.Log("Spawning: " + 'A' + preferredAlphabetIndexes[i % preferredAlphabetIndexes.Count]);
            existingBubbles.Add(bubble); //Add to list of existing bubble
        }
        return;
    }

// spawns n incorrect/wrong bubbles
    public void SpawnNIncorrectBubbles(int n)
    {
        GameObject prefab;
        for(int i=0;i<n;i++)
        {
            int numColors = GetColors().Length;
            int randColorIndex = Random.Range(0, numColors);
            prefab = bubblePrefabs[incorrectLetterList[Random.Range(0, incorrectLetterList.Count)]];
            GameObject bubble = InstantiateNewBubble(prefab, randColorIndex);
            existingBubbles.Add(bubble); //Add to list of existing bubble
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


    public IEnumerator Spawn(float initialDelayBurstColorSpawner, float minSpawnDelayBurstColorSpawner, float maxSpawnDelayBurstColorSpawner)
    {
        givenWord = FindObjectOfType<GameManager>().initialText.text;

        yield return new WaitForSeconds(initialDelayBurstColorSpawner); 
        int wordLen = givenWord.Length;

    // array of correct letters.
        char[] wordAlphabets = givenWord.ToCharArray();

        SetInitialWordArrayLists(wordAlphabets, wordLen);        
        
        while (enabled)
        {
            while(isFrozen) yield return null;
            yield return new WaitForSeconds( Random.Range(minSpawnDelayBurstColorSpawner,maxSpawnDelayBurstColorSpawner));
            // Debug.Log("Waited for burst color.");
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

     IEnumerator waitFunction(List<Vector3> currentVelocities)
     {
         yield return new WaitForSeconds(5);
         
         for (var i = 0; i < existingBubbles.Count; i++)
         {
             if (existingBubbles[i].gameObject)
             {
                 existingBubbles[i].GetComponent<Rigidbody>().velocity = currentVelocities[i]; //Set it's velocity to previous value
                 existingBubbles[i].GetComponent<Rigidbody>().useGravity = true;
             }
         }
         isFrozen = false;
     }
     
     IEnumerator WaitForUnscaledSeconds(float dur)
     {
         var cur = 0f;
         while (cur < dur)
         {
             yield return null;
             cur += Time.unscaledDeltaTime;
         }
     }

     public void OnFreezeClicked()
     {
         isFrozen = true;
         
         existingBubbles.RemoveAll(item => item == null); //Remove all destroyed bubbles
         
         List<Vector3> currentVelocities = new List<Vector3>();

         for (var i = 0; i < existingBubbles.Count; i++)
         {
             currentVelocities.Add(existingBubbles[i].GetComponent<Rigidbody>().velocity); //Record it's current velocity
             existingBubbles[i].GetComponent<Rigidbody>().velocity = Vector3.zero; //Set it's velocity to zero
             existingBubbles[i].GetComponent<Rigidbody>().useGravity = false;

         }
         
         StartCoroutine(waitFunction(currentVelocities));
     }




    // private Collider spawnArea;
    // private GameObject obstacle;
    // public Sprite sprite;
    // public Canvas canvas;
    // public float scaleValue = 0f;
    // public void CoroutineSpawn()
    // {
    //     spawnArea = GetComponent<Collider>();
    //     StartCoroutine(Spawn());
    // }

    // public IEnumerator Spawn()
    // {
    //     while (true)
    //     {
    //         CreateBurst();
    //         Destroy(obstacle, 10f);
    //         yield return new WaitForSeconds(5f);
    //     }
    // }

    // public void Start()
    // {
        
    // }

    // private void Update()
    // {
    //     if (!obstacle.IsUnityNull())
    //     {
    //         if (scaleValue <= 1)
    //         {
    //             Vector3 scale = new Vector3(scaleValue, scaleValue, 0f);
    //             obstacle.transform.localScale = scale;
    //             scaleValue += 0.01f;
    //         }
    //         obstacle.transform.Rotate(0, 0, 2, Space.Self);
    //     }
    // }

    // public void CreateBurst()
    // {
    //     Vector3 scale = new Vector3(scaleValue, scaleValue, 0f);
    //     GameObject obstacle = new GameObject("Obstacle");
    //     this.obstacle = obstacle;
    //     obstacle.AddComponent<Image>();
    //     obstacle.GetComponent<Image>().sprite = sprite;
    //     obstacle.tag = "Burst";
    //     //obstacle.AddComponent<Rigidbody>();
    //     //obstacle.GetComponent<Rigidbody>().useGravity = false;
    //     obstacle.transform.SetParent(canvas.transform);
    //     obstacle.transform.localScale = scale;
    //     Vector2 canvasSize = canvas.GetComponent<RectTransform>().rect.size;
    //     float x = Random.Range(-canvasSize.x/2+50, canvasSize.x/2-50);
    //     float y = Random.Range(-canvasSize.y/2+50, canvasSize.y/2-50);
    //     //float x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
    //     //float y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
    //     obstacle.transform.localPosition = new Vector3(x, y, 0f);
        
    //     obstacle.AddComponent<SphereCollider>();
    //     obstacle.GetComponent<SphereCollider>().radius = 50f;
    //     obstacle.GetComponent<SphereCollider>().isTrigger = true;
    // }
}
