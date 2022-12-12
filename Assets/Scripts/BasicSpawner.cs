using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;
using static System.Math;
using System.Threading.Tasks;

public class BasicSpawner : MonoBehaviour
{
    private Collider spawnArea;
    public GameObject[] bubblePrefabs;
    public Color[] colors;

    public Material[] materials;

    GameObject[] patternArray;


// stores indexes(in the form '<Correct Letter>' - 'A' : indexed(0-25)) of correct letters that are preferred for spawning.
    public List<int> preferredAlphabetIndexes = new List<int>();

// stores indexes(in the form '<InCorrect Letter>' - 'A' : indexed(0-25)) of incorrect letters that are NOT preferred for spawning.
    public List<int> incorrectLetterList = new List<int>();

    public static List<int> incorrectColorList = new List<int>();

// List to choose the number of bubbles to spawn in one go.
    public List<int> listNumBubblesInOneSpawn = new List<int>{ 3, 4};

    public float givenWordLettersPrefabsProb = 0.40f;

    public float correctBubbleCorrectColorProb = 0.70f;
    
    public float minAngle = -3f;
    public float maxAngle = 3f;
    
    public float minForce = 18;
    public float maxForce = 20;

    public float maxLifetime = 5f; //Increased max life time to accomodate for the freeing

    public string givenWord ="";
    public string prevWord ="";

    private int patternNum = 1;
    
    private List<GameObject> existingBubbles = new List<GameObject>(); //Stores existing bubbles and their current velocity

    private bool isFrozen = false;
    [SerializeField] private Image uiFill;

    public int freezeTimerDuration;

    private int remainingFreezeTimerDuration;
    public static int scoreBeforeFreeze {get; set;} = 0; //For Level 4Analytics
    public static int scoreAfterFreeze {get; set;} = 0; //For Level 4 Analytics
    public static int scoreDiffBeforeAndAfterFreeze {get; set;} = 0; //For Level 4 Analytics
    private bool spaceKeyPressed;
    public float upwardVelocity = 1;
    
    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
        scoreDiffBeforeAndAfterFreeze = 0;
    }
    
    private void Update()
    {
        if (SceneManager.GetActiveScene().name is "Level6")
        {
            if (spaceKeyPressed == false && Input.GetKeyDown("space"))
            {
                spaceKeyPressed = true;
                for (var i = 0; i < existingBubbles.Count; i++)
                {
                    if (existingBubbles[i].gameObject && existingBubbles[i].GetComponent<Rigidbody>().velocity.y <=0)
                    {
                        existingBubbles[i].GetComponent<Rigidbody>().velocity = Vector3.up * upwardVelocity;
                    }
                }
            }

            if (spaceKeyPressed == true && Input.GetKeyUp("space"))
            {
                spaceKeyPressed = false;
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void CoroutineSpawn(float initialDelayBasicSpawner, float spawnDelayBasicSpawner)
    {
        StartCoroutine(Spawn(initialDelayBasicSpawner, spawnDelayBasicSpawner));
    }

    
    public void SetInitialWordArrayLists(char[] wordAlphabets, int wordLen)
    {
        Debug.Log("Inside SetInitialWordArrayLists");
         for(int i=0;i<wordLen;i++)
        {
            Debug.Log(wordAlphabets[i]);
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

    public GameObject[] GetPatternArray()
    {
        List<GameObject> patternList = new List<GameObject>();

        for(int i=0; i<bubblePrefabs.Length; i++)
        {
            if(bubblePrefabs[i].name.Contains("Pattern"))
            {
                patternList.Add(bubblePrefabs[i]);
            }
        }
        return patternList.ToArray();
    }
    
    // spawns n correct/required bubbles
    public void SpawnNCorrectBubbles(int n)
    {
        GameObject prefab;
        Vector3 position = new Vector3();
        // position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

        // rotation of the spawner
        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));

         if(Random.value<0.5 || patternArray.Length==0)
         {
            for(int i=0;i<Max(n, preferredAlphabetIndexes.Count);i++)
            {
                if(i==n) break;
                prefab = bubblePrefabs[preferredAlphabetIndexes[i % preferredAlphabetIndexes.Count]];
                position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);

                GameObject bubble = Instantiate(prefab, position, rotation);
                SpawnManager.numOfBubblesSpawned++;
                Destroy(bubble, maxLifetime);
                float force = Random.Range(minForce, maxForce);
                bubble.GetComponent<Rigidbody>().AddForce(bubble.transform.up * force, ForceMode.Impulse);
                existingBubbles.Add(bubble); //Add to list of existing bubble
            }
        }
        else
        {
            Debug.Log(preferredAlphabetIndexes[Random.Range(0, preferredAlphabetIndexes.Count)]);
            prefab = bubblePrefabs[preferredAlphabetIndexes[Random.Range(0, preferredAlphabetIndexes.Count)]];
            position.x = patternNum % 2==0 ? spawnArea.bounds.min.x : spawnArea.bounds.max.x;
            patternNum = (patternNum + 1)%2;

            GameObject pattern = Instantiate(patternArray[Random.Range(0, patternArray.Length)]);
            Destroy(pattern, maxLifetime);

            for (int i = 0; i < pattern.transform.childCount; i++)
            {
                pattern.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.name = prefab.name;
                Material[] patternMaterial = new Material[1];
                patternMaterial[0] = materials[prefab.name[0] - 65];
                pattern.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().materials = patternMaterial;
                SpawnManager.numOfBubblesSpawned++;
            }

            GameObject patternClone = Instantiate(pattern, position, rotation);

            for (int i = 0; i < patternClone.transform.childCount; i++)
            {
                Destroy(patternClone.transform.GetChild(i).gameObject, maxLifetime);
            }
            Destroy(patternClone, maxLifetime);
            float force = Random.Range(17, 19);
            patternClone.GetComponent<Rigidbody>().AddForce(patternClone.transform.up * force, ForceMode.Impulse);
            existingBubbles.Add(patternClone); //Add to list of existing bubble
         }


        return;
    }

// spawns n incorrect/wrong bubbles
    public void SpawnNIncorrectBubbles(int n)
    {
        GameObject prefab;
        Vector3 position = new Vector3();
        // position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

        // rotation of the spawner
        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
        if(Random.value<0.7 || patternArray.Length==0)
        {
            for(int i=0;i<n;i++)
            {
                prefab = bubblePrefabs[incorrectLetterList[Random.Range(0, incorrectLetterList.Count)]];
                
                position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
                GameObject bubble = Instantiate(prefab, position, rotation);
                SpawnManager.numOfBubblesSpawned++;
                Destroy(bubble, maxLifetime);
                float force = Random.Range(17, 19);
                bubble.GetComponent<Rigidbody>().AddForce(bubble.transform.up * force, ForceMode.Impulse);
                existingBubbles.Add(bubble); //Add to list of existing bubble
            }
        }
        else
        {
            prefab = bubblePrefabs[incorrectLetterList[Random.Range(0, incorrectLetterList.Count)]];
            position.x = patternNum % 2 == 0 ? spawnArea.bounds.min.x : spawnArea.bounds.max.x;
            patternNum = (patternNum + 1)%2;

            GameObject pattern = Instantiate(patternArray[Random.Range(0, patternArray.Length)]);
            Destroy(pattern, maxLifetime);

            for (int i = 0; i < pattern.transform.childCount; i++)
            {
                pattern.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.name = prefab.name;
                Material[] patternMaterial = new Material[1];
                patternMaterial[0] = materials[prefab.name[0] - 65];
                pattern.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().materials = patternMaterial;
                SpawnManager.numOfBubblesSpawned++;
            }

            GameObject patternClone = Instantiate(pattern, position, rotation);

            for (int i = 0; i < patternClone.transform.childCount; i++)
            {
                Destroy(patternClone.transform.GetChild(i).gameObject, maxLifetime);
            }
            Destroy(patternClone, maxLifetime);
            float force = Random.Range(17, 19);
            patternClone.GetComponent<Rigidbody>().AddForce(patternClone.transform.up * force, ForceMode.Impulse);
            existingBubbles.Add(patternClone);  
            
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
        System.Random random = new System.Random();

        // if total number of bubbles spawning is 3, all must be correct, else atmost 2 should be incorrect.
        int numIncorrectBubbles = numBubblesInOneSpawn == 3 ? 0 : random.Next(0, 3);
        int numCorrectBubbles = numBubblesInOneSpawn - numIncorrectBubbles;
        
        SpawnBubblesHelper(numCorrectBubbles, numIncorrectBubbles);

    }


    public IEnumerator Spawn(float initialDelayBasicSpawner, float spawnDelayBasicSpawner)
    {
        givenWord = FindObjectOfType<GameManager>().initialText.text;
        Debug.Log(givenWord);
        patternArray = GetPatternArray();
        
        yield return new WaitForSeconds(initialDelayBasicSpawner); 
        int wordLen = givenWord.Length;

    // array of correct letters.
        char[] wordAlphabets = givenWord.ToCharArray();

        SetInitialWordArrayLists(wordAlphabets, wordLen);        
        
        while (enabled)
        {
            while(isFrozen) yield return null;
            int numBubblesInOneSpawn = GetNumBubblesToSpawn();
            SpawnBubbles(numBubblesInOneSpawn);
            
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
            
            yield return new WaitForSeconds(spawnDelayBasicSpawner);

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


// Remove letter from list of high probability spawn letters.
     public void AddLetterToPreferredList(char letter){
        int asciiOfLetterToBeAdded = (letter - 'A');

        // add only if doesn't exist, will have duplicates otherwise.
        if(!preferredAlphabetIndexes.Contains(asciiOfLetterToBeAdded))
        {
            preferredAlphabetIndexes.Add(asciiOfLetterToBeAdded);
        }
        return;
        
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
         
         scoreAfterFreeze = ScoreManager.score;
         scoreDiffBeforeAndAfterFreeze = scoreAfterFreeze - scoreBeforeFreeze;
         FindObjectOfType<GameManager>().SendAnalytic();
         
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
         scoreBeforeFreeze = ScoreManager.score;
         
         isFrozen = true;
         var freezeButton = GameObject.FindGameObjectWithTag("FreezeButton").GetComponent<Button>();
         freezeButton.interactable = false;

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

     public bool getBasicFrozen()
     {
         return isFrozen;
     }
}