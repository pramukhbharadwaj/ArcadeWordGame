using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Math;

public class RightSpawner : MonoBehaviour
{
    private Collider spawnArea;
    public GameObject[] bubblePrefabs;
    public Material[] materials;


    public float minForce = 1;
    public float maxForce = 3;

    public float maxLifetime = 15f;

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


    // Start is called before the first frame update
    void Awake()
    {
        spawnArea = GetComponent<Collider>();
        // CoroutineSpawn(2.5f, 5f);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void CoroutineSpawn(float initialDelayBasicSpawner, float spawnDelayBasicSpawner)
    {
        StartCoroutine(Spawn(initialDelayBasicSpawner, spawnDelayBasicSpawner));
    }

    // set physics aspects for each bubble
    public GameObject InstantiateNewBubble(GameObject prefab)
    {
        Vector3 position = new Vector3();
        position.y =  spawnArea.bounds.max.y ;
        position.z = (spawnArea.bounds.min.z + spawnArea.bounds.max.z)/2;
        position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);

        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);

        GameObject bubble = Instantiate(prefab, position, rotation);


        // random size of object.
        bubble.GetComponent<CannonBubble>().designatedLife = Random.Range(2,5);
        
        float currBubbleDesigLife = bubble.GetComponent<CannonBubble>().designatedLife;
        bubble.GetComponent<CannonBubble>().life = currBubbleDesigLife;
        // bubble.gameObject.designatedLife = Random.Range(2,4);
        float force = Random.Range(2f, 4f);


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
        
        bubble.GetComponent<Rigidbody>().AddForce(Vector3.left * force, ForceMode.Impulse);
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
            // if(preferredAlphabetIndexes.Count > 0){
                int randIdx = Random.Range(0,preferredAlphabetIndexes.Count);
                int prefAscii = preferredAlphabetIndexes[randIdx];
                Debug.Log(randIdx + "  right spwnr char ascii: " + (prefAscii) + "pref size : " + preferredAlphabetIndexes.Count);
                prefab = bubblePrefabs[prefAscii];
                GameObject bubble = InstantiateNewBubble(prefab);
                existingBubbles.Add(bubble); //Add to list of existing bubble
            // }
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

   public void SpawnBubbles(int numBubblesInOneSpawn)
    {
        int numIncorrectBubbles, numCorrectBubbles;
        if(Random.value<0.3)
        {
            numIncorrectBubbles = 1;
            numCorrectBubbles = 0;
        }
        else
        {
            numIncorrectBubbles = 0;
            numCorrectBubbles = 1;
        }
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
        
        while (enabled)
        {
            while(isFrozen) yield return null;
            int numBubblesInOneSpawn = 1;
            SpawnBubbles(numBubblesInOneSpawn);
            
            
            yield return new WaitForSeconds(spawnDelayBasicSpawner);

        }
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
