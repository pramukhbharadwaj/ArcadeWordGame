using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Math;

public class TutorialSpawner : MonoBehaviour
{
    private Collider spawnArea;
    public GameObject[] bubblePrefabs;
    public Color[] colors;
    public GameObject uiObject,animObject,orbInstObject,incorrectOrbInstObject,levelCompleteObject,blade;
    public RaycastHit hitInfo;
    private List<GameObject> existingBubbles = new List<GameObject>(); //Stores existing bubbles and their current velocity
    public GameObject[] specialBubblePrefabs;

    private bool isFrozen = false;
    [SerializeField] private Image uiFill;

    public int freezeTimerDuration;

    private int remainingFreezeTimerDuration;    
// stores indexes(in the form '<Correct Letter>' - 'A' : indexed(0-25)) of correct letters that are preferred for spawning.
    public List<int> preferredAlphabetIndexes = new List<int>();

// stores indexes(in the form '<InCorrect Letter>' - 'A' : indexed(0-25)) of incorrect letters that are NOT preferred for spawning.
    public List<int> incorrectLetterList = new List<int>();

    public static List<int> incorrectColorList = new List<int>();

// List to choose the number of bubbles to spawn in one go.
    public List<int> listNumBubblesInOneSpawn = new List<int>{ 3, 4};

    public float minSpawnDelay = 6f;
    public float maxSpawnDelay = 4f;
    
    public float minAngle = -0f;
    public float maxAngle = 0f;
    
    public float minForce = 20;
    public float maxForce = 20;

    public float maxLifetime = 180f;

    public string givenWord ="";
    public string prevWord ="";

    public bool isPaused=false;
    private bool spaceKeyPressed;
    public float upwardVelocity = 1;
    public GameObject spaceBarUiObject, spikesUiObject;
    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
        if(SceneManager.GetActiveScene().name!="Level6-Tutorial")
            blade.SetActive(false);
        
    }
    
    private void Update()
    {
        if (SceneManager.GetActiveScene().name is "Level6-Tutorial")
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

    public void CoroutineSpawn()
    {
        StartCoroutine(Spawn());
    }

    public int GetNumBubblesToSpawn()
    {
        System.Random rand = new System.Random();
        int index = rand.Next(listNumBubblesInOneSpawn.Count);
        int numBubblesInOneSpawn = listNumBubblesInOneSpawn[index];
        return numBubblesInOneSpawn;
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


// set physics aspects for each bubble
    public GameObject InstantiateNewBubble(GameObject prefab)
    {
        Vector3 position = new Vector3();
        int mid = (int)(spawnArea.bounds.min.x + spawnArea.bounds.max.x) / 2;
        position.x = Random.Range(mid,mid);;
        position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

        // rotation of the spawner
        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
        
        GameObject bubble = Instantiate(prefab, position, rotation);

        // int numColors = GetColors().Length;
        // int randColorIndex = Random.Range(0, numColors);
        // bubble.GetComponent<Renderer>().material.color = colors[randColorIndex];
        // SpawnManager.numOfxBubblesSpawned++;
        Destroy(bubble, maxLifetime);

        float force = Random.Range(minForce, maxForce);
        bubble.GetComponent<Rigidbody>().AddForce(bubble.transform.up * force, ForceMode.Impulse);

        return bubble;
    }
    public GameObject InstantiateNewColorBubble(GameObject prefab, int colorIndex)
    {
        Vector3 position = new Vector3();
        int mid = (int)(spawnArea.bounds.min.x + spawnArea.bounds.max.x) / 2;
        position.x = Random.Range(mid,mid);;
        position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

        // rotation of the spawner
        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
        
        GameObject bubble = Instantiate(prefab, position, rotation);

        int numColors = 3;
        int randColorIndex = colorIndex;
        bubble.GetComponent<Renderer>().material.color = colors[randColorIndex];
        SpawnManager.numOfBubblesSpawned++;
        Destroy(bubble, maxLifetime);

        float force = Random.Range(minForce, maxForce);
        bubble.GetComponent<Rigidbody>().AddForce(bubble.transform.up * force, ForceMode.Impulse);

        return bubble;
    }
    public GameObject InstantiateLevel4Bubble(GameObject prefab, int mid)
    {
        Vector3 position = new Vector3();
        // int mid = (int)(spawnArea.bounds.min.x + spawnArea.bounds.max.x) / 2;
        position.x = Random.Range(mid,mid);
        position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

        // rotation of the spawner
        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
        
        GameObject bubble = Instantiate(prefab, position, rotation);

        // int numColors = GetColors().Length;
        // int randColorIndex = Random.Range(0, numColors);
        // bubble.GetComponent<Renderer>().material.color = colors[randColorIndex];
        // SpawnManager.numOfBubblesSpawned++;
        Destroy(bubble, maxLifetime);

        float force = Random.Range(minForce, maxForce);
        bubble.GetComponent<Rigidbody>().AddForce(bubble.transform.up * force, ForceMode.Impulse);

        return bubble;
    }
    
    
    
    public void SpawnNCorrectBubbles(int n)
    {
        GameObject prefab;
        Vector3 position = new Vector3();
        // position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x
        position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);

        position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

        // rotation of the spawner
        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));


        for(int i=0;i<Max(n, preferredAlphabetIndexes.Count);i++)
        {
            if(i==n) break;
            prefab = bubblePrefabs[preferredAlphabetIndexes[i % preferredAlphabetIndexes.Count]];

            GameObject bubble = Instantiate(prefab, position, rotation);
            SpawnManager.numOfBubblesSpawned++;
            Destroy(bubble, maxLifetime);
            float force = Random.Range(minForce, maxForce);
            bubble.GetComponent<Rigidbody>().AddForce(bubble.transform.up * force, ForceMode.Impulse);
            existingBubbles.Add(bubble); //Add to list of existing bubble
        }
        
        

        return;
    }

    
       public void SpawnNIncorrectBubbles(int n)
    {
        GameObject prefab;
        Vector3 position = new Vector3();
        // position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

        // rotation of the spawner
        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
 
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
    public IEnumerator SpawnLevel1Tutorial()
    {
        
        GameObject prefabCorrect, prefabIncorrect;
        int correctCharacterToSpawnFirst = 2;                  // Set prefab to Character C
        prefabCorrect = bubblePrefabs[correctCharacterToSpawnFirst];   
        GameObject bubble = InstantiateNewBubble(prefabCorrect);
        yield return new WaitForSeconds(2.8f);
        StartCoroutine(DisplayAnim(bubble));
        yield return new WaitForSeconds(5f);
        while(isPaused == true){
            Debug.Log("In While");
            yield return null;
        }
        int incorrectCharacterToSpawnFirst = 3;
        prefabIncorrect = bubblePrefabs[incorrectCharacterToSpawnFirst];
        if(isPaused is false)    
        {
            GameObject incorrectBubble = InstantiateNewBubble(prefabIncorrect);
            yield return new WaitForSeconds(2.8f);
            StartCoroutine(DisplayAnim(incorrectBubble));
            yield return new WaitForSeconds(2f);
        }

        
    }
    public IEnumerator SpawnLevel2Tutorial()
    {
        GameObject prefab;
        int indexOfSpecialBubble = 26;
        // prefab = specialBubblePrefabs[indexOfSpecialBubble];

        prefab = specialBubblePrefabs[2];
        string letterName = specialBubblePrefabs[2].name;
        // Debug.Log(letterIndex + " " +letterName)

        GameObject bubble = InstantiateNewBubble(prefab);
        bubble.GetComponent<Renderer>().material.color = colors[0];
        yield return new WaitForSeconds(2.8f);
        StartCoroutine(DisplayAnim(bubble));
        yield return new WaitForSeconds(5f);
        while(isPaused == true){
            Debug.Log("In While");
            yield return null;
        }
        
    }
    public IEnumerator SpawnLevel3Tutorial()
    {
        FindObjectOfType<ObstacleSpawner>().TutorialSpawn();
        yield return new WaitForSeconds(2.8f);
        StartCoroutine(DisplayAnim());
        yield return new WaitForSeconds(5f);
        while(isPaused == true){
            Debug.Log("In While");
            yield return null;
        }
    }
    public IEnumerator SpawnLevel5Tutorial()
    {
        
        GameObject prefabCorrect, prefabIncorrect;
        int correctCharacterToSpawnFirst = 2;                  // Set prefab to Character C
        prefabCorrect = bubblePrefabs[correctCharacterToSpawnFirst];   
        GameObject bubble = InstantiateNewColorBubble(prefabCorrect,2);
        yield return new WaitForSeconds(2.8f);
        StartCoroutine(DisplayAnim(bubble));
        yield return new WaitForSeconds(5f);
        while(isPaused == true){
            Debug.Log("In While");
            yield return null;
        }
        int incorrectCharacterToSpawnFirst = 2;
        prefabIncorrect = bubblePrefabs[incorrectCharacterToSpawnFirst];
        if(isPaused is false)    
        {
            GameObject incorrectBubble = InstantiateNewColorBubble(prefabIncorrect,1);
            uiObject.GetComponentsInChildren<TMP_Text>()[0].text = "Slice this bubble";
            uiObject.GetComponentsInChildren<TMP_Text>()[1].enabled = false;
            uiObject.GetComponentInChildren<Image>().enabled = false;
            yield return new WaitForSeconds(2.8f);
            StartCoroutine(DisplayAnim(incorrectBubble));
            yield return new WaitForSeconds(2f);
        }

        
    }
    public IEnumerator SpawnLevel4Tutorial()
    {
        int mid = (int)(spawnArea.bounds.min.x + spawnArea.bounds.max.x) / 2;
        GameObject prefab1;
        int correctCharacterToSpawnFirst = 2;                  // Set prefab to Character C
        prefab1 = bubblePrefabs[correctCharacterToSpawnFirst];
        GameObject bubble1 = InstantiateLevel4Bubble(prefab1,mid-2);
        GameObject bubble2 = InstantiateLevel4Bubble(prefab1,mid);
        GameObject bubble3 = InstantiateLevel4Bubble(prefab1,mid+2);
        existingBubbles.Add(bubble1);
        existingBubbles.Add(bubble2);
        existingBubbles.Add(bubble3);
        StartCoroutine(DisplayLevel4Anim());
        while(isFrozen) yield return null;
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < 100; i++)
        {
            GameObject bubble4 = InstantiateLevel4Bubble(prefab1,mid-2);
            GameObject bubble5 = InstantiateLevel4Bubble(prefab1,mid);
            GameObject bubble6 = InstantiateLevel4Bubble(prefab1,mid+2);
            existingBubbles.Add(bubble4);
            existingBubbles.Add(bubble5);
            existingBubbles.Add(bubble6);
            yield return new WaitForSeconds(5f);
        }
        // while(isPaused == true){
        //     Debug.Log("In While");
        //     yield return null;
        // }
        // int incorrectCharacterToSpawnFirst = 3;
        // prefabIncorrect = bubblePrefabs[incorrectCharacterToSpawnFirst];
        // if(isPaused is false)    
        // {
        //     GameObject incorrectBubble = InstantiateNewBubble(prefabIncorrect);
        //     yield return new WaitForSeconds(2.8f);
        //     StartCoroutine(DisplayAnim(incorrectBubble));
        //     yield return new WaitForSeconds(2f);
        // }


    }
    public IEnumerator SpawnLevel6Tutorial()
    {
        spaceBarUiObject.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        spaceBarUiObject.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        spikesUiObject.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        spikesUiObject.SetActive(false);
        givenWord = FindObjectOfType<GameManager>().initialText.text;
        Debug.Log(givenWord);
        // patternArray = GetPatternArray();
        
        yield return new WaitForSeconds(1.0f); 
        int wordLen = givenWord.Length;

        // array of correct letters.
        char[] wordAlphabets = givenWord.ToCharArray();

        SetInitialWordArrayLists(wordAlphabets, wordLen);        
        
        while (enabled)
        {
            Debug.Log("Inside tutorial level 6 while enabled");
            while(isFrozen) yield return null;
            int numBubblesInOneSpawn = GetNumBubblesToSpawn();
            blade.SetActive(true);
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
            
            yield return new WaitForSeconds(3.0f);

        }
        
        
        // yield return new WaitForSeconds(5f);
        // while (enabled)
        // {
        //     int numBubblesInOneSpawn = GetNumBubblesToSpawn();
        //     SpawnBubbles(numBubblesInOneSpawn);
        // }
        
    
    }
    public IEnumerator Spawn()
    {
        givenWord = FindObjectOfType<GameManager>().initialText.text;
        yield return new WaitForSeconds(3f); 
        
        int wordLen = givenWord.Length;

        // array of correct letters.
        char[] wordAlphabets = givenWord.ToCharArray();
        SetInitialWordArrayLists(wordAlphabets, wordLen);        

        if(SceneManager.GetActiveScene().name == "Level1-Tutorial")
        {
            Debug.Log("In the tutorial spawn call");

            StartCoroutine(SpawnLevel1Tutorial());
        }
        if(SceneManager.GetActiveScene().name == "Level2-Tutorial")
        {
            Debug.Log("In the tutorial spawn call");

            StartCoroutine(SpawnLevel2Tutorial());
        }
        if(SceneManager.GetActiveScene().name == "Level3-Tutorial")
        {
            Debug.Log("In the tutorial spawn call");

            StartCoroutine(SpawnLevel3Tutorial());
        }
        if(SceneManager.GetActiveScene().name == "Level4-Tutorial")
        {
            Debug.Log("In the tutorial level 4 spawn call");
            StartCoroutine(SpawnLevel4Tutorial());
        }                
        if(SceneManager.GetActiveScene().name == "Level5-Tutorial")
        {
            Debug.Log("In the tutorial spawn call");
            StartCoroutine(SpawnLevel5Tutorial());
        }   
        if(SceneManager.GetActiveScene().name == "Level6-Tutorial")
        {
            Debug.Log("In the tutorial spawn call");
            StartCoroutine(SpawnLevel6Tutorial());
        }  
        yield return new WaitForSeconds(maxSpawnDelay);
    }


     public Color[] GetColors()
     {
         return colors;
     }
     IEnumerator DisplayAnim(GameObject bubble)
     {
         Time.timeScale = 0f;
         isPaused = true;
         uiObject.SetActive(true);
         animObject.SetActive(true);
         blade.SetActive(true);
         Debug.Log("......");
         Debug.Log(Time.timeScale); 
         yield return WaitForUnscaledSeconds(0f);
         Time.timeScale = 1f;
         Rigidbody bubbleBody = bubble.GetComponent<Rigidbody>();
         bubbleBody.useGravity = false;
         bubbleBody.velocity = Vector3.zero;
         
         // Thread.Sleep(2);
         // Time.timeScale = 0.5f;
         // Time.timeScale = 1f;
         
         Debug.Log(Time.timeScale);
               
     }
     
     IEnumerator DisplayAnim()
     {
         Time.timeScale = 0f;
         isPaused = true;
         uiObject.SetActive(true);
         animObject.SetActive(true);
         blade.SetActive(true);
         Debug.Log("......");
         Debug.Log(Time.timeScale); 
         yield return WaitForUnscaledSeconds(0f);
         Time.timeScale = 1f;
         
         // Thread.Sleep(2);
         // Time.timeScale = 0.5f;
         // Time.timeScale = 1f;
         
         Debug.Log(Time.timeScale);
               
     }
     public void HideAnim()
     {
         Debug.Log("Inside HideAnim");
         isPaused = false;
         animObject.SetActive(false);
         uiObject.SetActive(false);
        StartCoroutine(DisplayOrbInst());
         // Time.timeScale = 0.5f;
         // Time.timeScale = 1f;
     }

     public void HideAnimLevel3()
     {
        isPaused = false;
        animObject.SetActive(false);
        uiObject.SetActive(false);
        StartCoroutine(DisplayLevelComplete());
     }

     public void HideAnimIncorrect()
     {
         isPaused = false;
         animObject.SetActive(false);
         uiObject.SetActive(false);
         StartCoroutine(DisplayIncorrectOrbInst());
         // Time.timeScale = 0.5f;
         // Time.timeScale = 1f;


     }
     IEnumerator DisplayOrbInst()
     {
         orbInstObject.SetActive(true);
         yield return new WaitForSeconds(4f);
         orbInstObject.SetActive(false);   
     }
     IEnumerator DisplayIncorrectOrbInst()
     {
         incorrectOrbInstObject.SetActive(true);
         yield return new WaitForSeconds(4f);
         incorrectOrbInstObject.SetActive(false);   
     }

     public IEnumerator DisplayLevelComplete()
     {
         yield return WaitForUnscaledSeconds(5f);
         levelCompleteObject.SetActive(true);
         yield return WaitForUnscaledSeconds(5f);
         levelCompleteObject.SetActive(false);
         if (SceneManager.GetActiveScene().name == "Level1-Tutorial")
         {
             // SceneManager.LoadScene("SliceItOff");
             LevelAndGenreManager.levelName = "SliceItOff";
             TimeManager.Clear();
             SceneManager.LoadScene("Genre");
         }
            
         else if (SceneManager.GetActiveScene().name == "Level2-Tutorial")
         {
             // SceneManager.LoadScene("Level2");
             LevelAndGenreManager.levelName = "Level2";
             TimeManager.Clear();
             SceneManager.LoadScene("Genre");
         }
         else if (SceneManager.GetActiveScene().name == "Level3-Tutorial")
         {
             // SceneManager.LoadScene("Level3_updated");
             LevelAndGenreManager.levelName = "Level3_updated";
             TimeManager.Clear();
             SceneManager.LoadScene("Genre");
         }
         else if (SceneManager.GetActiveScene().name == "Level4-Tutorial")
         {
             // SceneManager.LoadScene("Level4");
             LevelAndGenreManager.levelName = "Level4";
             TimeManager.Clear();
             SceneManager.LoadScene("Genre");
         }
         else if (SceneManager.GetActiveScene().name == "Level5-Tutorial")
         {
             // SceneManager.LoadScene("Level5");
             LevelAndGenreManager.levelName = "Level5";
             TimeManager.Clear();
             SceneManager.LoadScene("Genre");
         }
             
         
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

     IEnumerator DisplayLevel4Anim()
     {  
         blade.SetActive(true);
         uiObject.SetActive(true);
         animObject.SetActive(true);
         yield return new WaitForSeconds(3f);
     }

     public void HideLevel4Anim()
     {
         uiObject.SetActive(false);
         animObject.SetActive(false);
         
     }

     public IEnumerator DisplayLevel6Complete()
     {
         // yield return new WaitForSeconds(10.0f);
         levelCompleteObject.SetActive(true);
         yield return null;
         // levelCompleteObject.SetActive(false);
         // yield return new WaitForSeconds(1.0f);

     }
     IEnumerator waitFunction(List<Vector3> currentVelocities)
     {
         yield return new WaitForSeconds(5);
         incorrectOrbInstObject.SetActive(false);
         // Debug.Log("Current Velocities:"+currentVelocities.Count);
         // Debug.Log("Existing Bubbles:"+existingBubbles.Count);
         for (var i = 0; i < existingBubbles.Count; i++)
         {
             if (existingBubbles[i].gameObject && i<currentVelocities.Count)
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
         incorrectOrbInstObject.SetActive(true);
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
     public void AddLetterToPreferredList(char letter){
         int asciiOfLetterToBeAdded = (letter - 'A');

         // add only if doesn't exist, will have duplicates otherwise.
         if(!preferredAlphabetIndexes.Contains(asciiOfLetterToBeAdded))
         {
             preferredAlphabetIndexes.Add(asciiOfLetterToBeAdded);
         }
         return;
        
     }

     public bool getSpaceKeyPressed()
     {
         return spaceKeyPressed;
     }
}

