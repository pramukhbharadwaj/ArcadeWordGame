using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Math;

public class CannonTutorialSpawner : MonoBehaviour
{
    private Collider spawnArea;
    public GameObject[] bubblePrefabs;
    public Material[] materials;


    public float minForce = 1;
    public float maxForce = 3;

    public float maxLifetime = 150f;

    public string givenWord ="";

    // stores indexes(in the form '<Correct Letter>' - 'A' : indexed(0-25)) of correct letters that are preferred for spawning.
    public List<int> preferredAlphabetIndexes = new List<int>();

// stores indexes(in the form '<InCorrect Letter>' - 'A' : indexed(0-25)) of incorrect letters that are NOT preferred for spawning.
    public List<int> incorrectLetterList = new List<int>();

    public static List<int> incorrectColorList = new List<int>();

    private List<GameObject> existingBubbles = new List<GameObject>(); //Stores existing bubbles and their current velocity

	private bool isFrozen = false;

    [SerializeField] private Image uiFill;

    public int freezeTimerDuration;

    private int remainingFreezeTimerDuration;

    public bool isDestroyed = false, isCannonCollision = false, cannonCollisionHappened = false;

    public GameObject spaceBarInstUIObject, correctInstUIObject, incorrectInstUIObject, tutorialCompleteInstUIObject, freezeButtonInst; 
    // Start is called before the first frame update
    void Awake()
    {
        spawnArea = GetComponent<Collider>();
        // CoroutineSpawn(2.5f, 1f);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void CoroutineSpawn(float initialDelayBasicSpawner, float spawnDelayBasicSpawner)
    {
        Debug.Log("SPAWN CANNON TUT");
        StartCoroutine(Spawn(initialDelayBasicSpawner, spawnDelayBasicSpawner));
        Debug.Log("SPAWN CANNON TUT");
    }

    // set physics aspects for each bubble
    public GameObject InstantiateNewBubble(GameObject prefab)
    {
        Vector3 position = new Vector3();
        position.y =  Random.Range(spawnArea.bounds.max.y/2, spawnArea.bounds.max.y) ;
        position.z = (spawnArea.bounds.min.z + spawnArea.bounds.max.z)/2;
        position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);

        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);

        GameObject bubble = Instantiate(prefab, position, rotation);

        // random size of object.
        bubble.GetComponent<CannonBubble>().designatedLife = 3f;
        
        float currBubbleDesigLife = bubble.GetComponent<CannonBubble>().designatedLife;
        bubble.GetComponent<CannonBubble>().life = currBubbleDesigLife;
        float force = Random.Range(2f, 4f);
        // bubble.gameObject.designatedLife = Random.Range(2,4);

        if(currBubbleDesigLife == 2){
            bubble.transform.localScale = bubble.transform.localScale * 0.8f;
            force = 4f;
        }
        else if(currBubbleDesigLife == 3){
            bubble.transform.localScale = bubble.transform.localScale * 1.2f;
            force = 3f;
        }
        else if(currBubbleDesigLife == 4){
            bubble.transform.localScale *= 1.8f;
            force = 2f;
        }


        Destroy(bubble, maxLifetime);
        // float force = Random.Range(2f, 4f);
        bubble.GetComponent<Rigidbody>().AddForce(Vector3.right * force, ForceMode.Impulse);
        return bubble;
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
    // spawns n correct/required bubbles
    public void SpawnNCorrectBubbles(int n)
    {
        GameObject prefab;
        for(int i=0; i<n; i++)
        {
            prefab = bubblePrefabs[preferredAlphabetIndexes[0]];
            GameObject bubble = InstantiateNewBubble(prefab);
            existingBubbles.Add(bubble); //Add to list of existing bubble
        }
        return;
    }

// spawns n incorrect/wrong bubbles
    public void SpawnNIncorrectBubbles(int n)
    {
        GameObject prefab;
        for(int i=0; i<n; i++)
        {
            prefab = bubblePrefabs[incorrectLetterList[Random.Range(0, incorrectLetterList.Count)]];
            GameObject bubble = InstantiateNewBubble(prefab);
            existingBubbles.Add(bubble); //Add to list of existing bubble
        }
        return;
    }


    public void SpawnBubblesHelper(int numCorrectBubbles, int numIncorrectBubbles)
    {
        SpawnNCorrectBubbles(numCorrectBubbles);
        SpawnNIncorrectBubbles(numIncorrectBubbles);
    }

    public void SpawnBubbles(int numBubblesInOneSpawn, int correctBubbles)
    {
        int numIncorrectBubbles, numCorrectBubbles;
        
            numIncorrectBubbles = numBubblesInOneSpawn - correctBubbles;
            numCorrectBubbles = correctBubbles;
        
        SpawnBubblesHelper(numCorrectBubbles, numIncorrectBubbles);

    }

    public IEnumerator Spawn(float initialDelayBasicSpawner, float spawnDelayBasicSpawner)
    {
        
        yield return new WaitForSeconds(initialDelayBasicSpawner); 
        givenWord = FindObjectOfType<GameManager>().initialText.text;
        int wordLen = givenWord.Length;

    // array of correct letters.
        char[] wordAlphabets = givenWord.ToCharArray();

        SetInitialWordArrayLists(wordAlphabets, wordLen);   
            while(isFrozen) yield return null;

        
        if(SceneManager.GetActiveScene().name is "CannonLevel1-Tutorial"){
            int numBubblesInOneSpawn = 1;
            SpawnBubbles(numBubblesInOneSpawn, 1);
            yield return new WaitForSeconds(2.0f);
            while (isCannonCollision)
            {
                SpawnBubbles(numBubblesInOneSpawn, 1);
                yield return new WaitForSeconds(2.0f);
            }

            if (cannonCollisionHappened)
            {
                existingBubbles.RemoveAll(item => item == null);
                cannonCollisionHappened = false;
                // SpawnBubbles(numBubblesInOneSpawn, 1);
                yield return new WaitForSeconds(0.5f);
            }
            for (var i = 0; i < existingBubbles.Count; i++)
            {
                
                existingBubbles[i].GetComponent<Rigidbody>().velocity = Vector3.zero; //Set it's velocity to zero
                existingBubbles[i].GetComponent<Rigidbody>().useGravity = false;

            }
            spaceBarInstUIObject.SetActive(true);

            while (!isDestroyed)
            {
                Debug.Log(isDestroyed);
                yield return new WaitForSeconds(1.0f);
            }

            isDestroyed = false;
            Debug.Log("Outside while:"+isDestroyed);
            spaceBarInstUIObject.SetActive(false);
            correctInstUIObject.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            correctInstUIObject.SetActive(false);
            
            SpawnBubbles(numBubblesInOneSpawn, 0);
            existingBubbles.RemoveAll(item => item == null);
            yield return new WaitForSeconds(2.5f);
            while (isCannonCollision)
            {
                SpawnBubbles(numBubblesInOneSpawn, 0);
                yield return new WaitForSeconds(2.0f);
            }

            if (cannonCollisionHappened)
            {
                existingBubbles.RemoveAll(item => item == null);
                cannonCollisionHappened = false;
                // SpawnBubbles(numBubblesInOneSpawn, 0);
                yield return new WaitForSeconds(0.5f);
            }
            for (var i = 0; i < existingBubbles.Count; i++)
            {
                existingBubbles[i].GetComponent<Rigidbody>().velocity = Vector3.zero; //Set it's velocity to zero
                existingBubbles[i].GetComponent<Rigidbody>().useGravity = false;
            }    
            spaceBarInstUIObject.SetActive(true);
            while (!isDestroyed)
            {
                // Debug.Log(isDestroyed);
                yield return new WaitForSeconds(1.0f);
            }

            isDestroyed = false;
            spaceBarInstUIObject.SetActive(false);
            incorrectInstUIObject.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            incorrectInstUIObject.SetActive(false);
            tutorialCompleteInstUIObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            LevelAndGenreManager.levelName = "CannonLevel1";
            SceneManager.LoadScene("Genre");

        }else if(SceneManager.GetActiveScene().name is "CannonLevel3-Tutorial"){
            SpawnNSpecialBubbles(1);
            yield return new WaitForSeconds(2.0f);
            while (isCannonCollision)
            {
                SpawnNSpecialBubbles(1);
                yield return new WaitForSeconds(2.0f);
            }

            if (cannonCollisionHappened)
            {
                existingBubbles.RemoveAll(item => item == null);
                cannonCollisionHappened = false;
                // SpawnBubbles(numBubblesInOneSpawn, 1);
                yield return new WaitForSeconds(0.5f);
            }
            correctInstUIObject.SetActive(true);
            for (var i = 0; i < existingBubbles.Count; i++)
            {
                
                existingBubbles[i].GetComponent<Rigidbody>().velocity = Vector3.zero; //Set it's velocity to zero
                existingBubbles[i].GetComponent<Rigidbody>().useGravity = false;

            }
            while (!isDestroyed)
            {
                Debug.Log(isDestroyed);
                yield return new WaitForSeconds(1.0f);
            }
            correctInstUIObject.SetActive(false);
            isDestroyed = false;
            tutorialCompleteInstUIObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            LevelAndGenreManager.levelName = "CannonLevel3";
            SceneManager.LoadScene("Genre");
        }else if(SceneManager.GetActiveScene().name is "CannonLevel4-Tutorial"){
            while (!isFrozen)
            {
                // if(isDestroyed==true) isDestroyed=false;
                SpawnBubbles(1, 1);
                yield return new WaitForSeconds(7.0f);
            }
            if (isFrozen)
            {
                while(!isDestroyed)
                {
                    yield return null;
                    
                }
                
                yield return new WaitForSeconds(2.0f);
            }
            yield return new WaitForSeconds(1.0f);

            isDestroyed = false;
            freezeButtonInst.SetActive(false);
            tutorialCompleteInstUIObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            LevelAndGenreManager.levelName = "CannonLevel4";
            SceneManager.LoadScene("Genre");
        }
        
        // .......wrong bub

    }

    public void SpawnNSpecialBubbles(int n)
    {
        GameObject prefab;

        prefab = bubblePrefabs[26];
        GameObject bubble_s = InstantiateNewBubble(prefab);
        existingBubbles.Add(bubble_s);
        
        return;
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
         
         //StartCoroutine(waitFunction(currentVelocities));
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
        Debug.Log("updating word: "+ newWord);
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
}
