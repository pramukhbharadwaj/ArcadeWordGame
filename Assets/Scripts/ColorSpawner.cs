using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Math;

public class ColorSpawner : MonoBehaviour
{
    private Collider spawnArea;
    public GameObject[] bubblePrefabs;
    public Color[] colors;

// stores indexes(in the form '<Correct Letter>' - 'A' : indexed(0-25)) of correct letters that are preferred for spawning.
    public List<int> preferredAlphabetIndexes = new List<int>();

// stores indexes(in the form '<InCorrect Letter>' - 'A' : indexed(0-25)) of incorrect letters that are NOT preferred for spawning.
    public List<int> incorrectLetterList = new List<int>();

    public static List<int> incorrectColorList = new List<int>();

// List to choose the number of bubbles to spawn in one go.
    public List<int> listNumBubblesInOneSpawn = new List<int>{ 6, 7, 8, 9, 10, 11};

    public float givenWordLettersPrefabsProb = 0.40f;

    public float correctBubbleCorrectColorProb = 0.70f;
    
    public float minAngle = -3f;
    public float maxAngle = 3f;
    
    public float minForce = 18;
    public float maxForce = 20;

    public float maxLifetime = 5f;

    public string givenWord ="";
    public string prevWord ="";
    
    private bool isFrozen = false;
    private List<GameObject> existingBubbles = new List<GameObject>(); //Stores existing bubbles and their current velocity
    public int freezeTimerDuration;
    private int remainingFreezeTimerDuration;
    [SerializeField] private Image uiFill;
    

    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void CoroutineSpawn(float initialDelayColorSpawner, float spawnDelayColorSpawner)
    {
        StartCoroutine(Spawn(initialDelayColorSpawner, spawnDelayColorSpawner));
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

        for(int i=0;i<colors.Length;i++)
        {
            incorrectColorList.Add(i);
        }
    }

// randomly choose from {3,4,5} for the number of bubbles to pop together in current spawn
    public int GetNumBubblesToSpawn()
    {
        System.Random rand = new System.Random();
        int index = rand.Next(listNumBubblesInOneSpawn.Count);

        int numBubblesInOneSpawn = listNumBubblesInOneSpawn[index];
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

        //int numColors = GetColors().Length;
        //int randColorIndex = Random.Range(0, numColors);
        bubble.GetComponent<Renderer>().material.color = colors[randColorIndex];
        SpawnManager.numOfBubblesSpawned++;

        Destroy(bubble, maxLifetime);

        float force = Random.Range(minForce, maxForce);
        bubble.GetComponent<Rigidbody>().AddForce(bubble.transform.up * force, ForceMode.Impulse);

        return bubble;
    }

// spawns n correct/required bubbles
    public void SpawnNCorrectBubbles(int n, int randColorIndex)
    {
        GameObject prefab;
        for(int i=0;i<Max(n, preferredAlphabetIndexes.Count);i++)
        {
            if(i==n) break;
            prefab = bubblePrefabs[preferredAlphabetIndexes[i % preferredAlphabetIndexes.Count]];
            GameObject bubble = InstantiateNewBubble(prefab, randColorIndex);
            existingBubbles.Add(bubble); //Add to list of existing bubble
        }
        return;
    }

// spawns n incorrect/wrong bubbles
    public void SpawnNIncorrectBubbles(int n, int randColorIndex)
    {
        GameObject prefab;
        for(int i=0;i<n;i++)
        {
            prefab = bubblePrefabs[incorrectLetterList[Random.Range(0, incorrectLetterList.Count)]];
            GameObject bubble = InstantiateNewBubble(prefab, randColorIndex);
            existingBubbles.Add(bubble); //Add to list of existing bubble
        }
        return;
    }


    public void SpawnBubblesHelper(int numCorrectBubbles, int numIncorrectBubbles, int randColorIndex)
    {
        SpawnNCorrectBubbles(numCorrectBubbles, randColorIndex);
        SpawnNIncorrectBubbles(numIncorrectBubbles, randColorIndex);
    }


    public void SpawnBubbles(int numBubblesInOneSpawn, int randColorIndex)
    {
        System.Random random = new System.Random();

        // if total number of bubbles spawning is 3, all must be correct, else atmost 2 should be incorrect.
        int numIncorrectBubbles = numBubblesInOneSpawn == 3 ? 0 : random.Next(0, 3);
        int numCorrectBubbles = numBubblesInOneSpawn - numIncorrectBubbles;
        
        SpawnBubblesHelper(numCorrectBubbles, numIncorrectBubbles, randColorIndex);

    }


    public IEnumerator Spawn(float initialDelayColorSpawner, float spawnDelayColorSpawner)
    {
        givenWord = FindObjectOfType<GameManager>().initialText.text;
        yield return new WaitForSeconds(initialDelayColorSpawner); 
        int wordLen = givenWord.Length;

        //    Debug.Log("in color spawner spawn");
    // array of correct letters.
        char[] wordAlphabets = givenWord.ToCharArray();

        SetInitialWordArrayLists(wordAlphabets, wordLen);   

        int spawnColorChangeCounter = 0;     
        int randColorIndex, nextRandColorIndex;
        int numColors = GetColors().Length;
        randColorIndex = Random.Range(0, numColors);
        nextRandColorIndex = Random.Range(0, numColors);
        while (nextRandColorIndex == randColorIndex)
            nextRandColorIndex = Random.Range(0, numColors);

        int spawnRandomCounter = Random.Range(4, 6);

        while (enabled)
        {
            while(isFrozen) yield return null;

            // Debug.Log("color change counter " + spawnColorChangeCounter);
            // Debug.Log("Random counter" + spawnRandomCounter);

            if(spawnColorChangeCounter == spawnRandomCounter)
            {
                randColorIndex = Random.Range(0, numColors);
                nextRandColorIndex = Random.Range(0, numColors);
                while (nextRandColorIndex == randColorIndex)
                    nextRandColorIndex = Random.Range(0, numColors);

                spawnColorChangeCounter = 0;
                spawnRandomCounter = Random.Range(4, 6);
            }
            

            int numBubblesInOneSpawn = GetNumBubblesToSpawn();
            SpawnBubbles((int)Mathf.Round(0.75f * numBubblesInOneSpawn), randColorIndex);


            // Debug.Log("col spawn 2");

            yield return new WaitForSeconds(0.5f);
            
            while(isFrozen) yield return null;
            
            SpawnBubbles((int)Mathf.Round(0.25f * numBubblesInOneSpawn), nextRandColorIndex);



            
            //Debug.Log("spawning colors now");
            /*
            Commented out as reference for color level spawning
            */
            // int preferredColorInd = SpawnManager.preferredBubbleSpawnColor;
            // if(SceneManager.GetActiveScene().name is "Level3")
            // {
            //     if(isCurrBubbleCorrect && Random.value < correctBubbleCorrectColorProb)
            //     {
            //         bubble.GetComponent<Renderer>().material.color = colors[preferredColorInd];
            //     }
            //     else{
            //         bubble.GetComponent<Renderer>().material.color = colors[incorrectColorList[Random.Range(0, incorrectColorList.Count)]];
            //     }
            // }

            // yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            // Debug.Log("waiting " + spawnDelayColorSpawner);
            spawnColorChangeCounter++;
            yield return new WaitForSeconds(spawnDelayColorSpawner);

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
         
        //  Debug.Log(existingBubbles.Count);
         for (var i = 0; i < existingBubbles.Count; i++)
         {
             if (existingBubbles[i].gameObject)
             {
                 existingBubbles[i].GetComponent<Rigidbody>().velocity = currentVelocities[i]; //Set it's velocity to previous value
                 existingBubbles[i].GetComponent<Rigidbody>().useGravity = true;
             }
         }
         isFrozen = false;
         BeginFreezeTimer(freezeTimerDuration);
     }

     public void OnFreezeClicked()
     {
         isFrozen = true;
         var freezeButton = GameObject.FindGameObjectWithTag("FreezeButton").GetComponent<Button>();
         freezeButton.interactable = false;

         existingBubbles.RemoveAll(item => item == null); //Remove all destroyed bubbles
         
         List<Vector3> currentVelocities = new List<Vector3>();

        //  Debug.Log(existingBubbles.Count);
         for (var i = 0; i < existingBubbles.Count; i++)
         {
             currentVelocities.Add(existingBubbles[i].GetComponent<Rigidbody>().velocity); //Record it's current velocity
             existingBubbles[i].GetComponent<Rigidbody>().velocity = Vector3.zero; //Set it's velocity to zero
             existingBubbles[i].GetComponent<Rigidbody>().useGravity = false;

         }
         
         StartCoroutine(waitFunction(currentVelocities));
     }

     private void BeginFreezeTimer(int Second)
     {
         remainingFreezeTimerDuration = Second;
         StartCoroutine(UpdateFreezeTimer());
     }

     private IEnumerator UpdateFreezeTimer()
     {
         while (remainingFreezeTimerDuration >= 0)
         {
             uiFill.fillAmount = Mathf.InverseLerp(0, freezeTimerDuration, remainingFreezeTimerDuration);
             remainingFreezeTimerDuration--;
             yield return new WaitForSeconds(1f);
         }
         var freezeButton = GameObject.FindGameObjectWithTag("FreezeButton").GetComponent<Button>();
         freezeButton.interactable = true;
     }
     
     
}
