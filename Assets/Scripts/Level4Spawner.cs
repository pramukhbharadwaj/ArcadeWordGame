using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level4Spawner : MonoBehaviour
{
    
    private Collider spawnArea;
    public GameObject[] bubblePrefabs;
    public Color[] colors;

    public List<int> preferredAlphabetIndexes = new List<int>();
    public List<int> incorrectLetterList = new List<int>();
    public float givenWordLettersPrefabsProb = 0.40f;
    

    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 1.25f;
    
    public float minAngle = -15f;
    public float maxAngle = 10f;
    
    public float minForce = 15;
    public float maxForce = 17;

    public float maxLifetime = 5f;
    
    public List<string> givenSetOfWords = new List<string>();

    //public char[] bucketLetters = new char[] {};
    public List<char> bucketLetters = new List<char>();
    
    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        // givenWord = FindObjectOfType<GameManager>().initialText.text;
        givenSetOfWords = FindObjectOfType<Level4GameManager>().GetCurrentWords();
        bucketLetters = FindObjectOfType<Level4GameManager>().GetBucketList();

        //givenSetOfWords.Add("CAT"); //Level4
        //givenSetOfWords.Add("RAT"); //Level4
        //givenSetOfWords.Add("BAT"); //Level4

        //bucketLetters[0] = 'A';
        //bucketLetters[1] = 'B';
        //bucketLetters[2] = 'C';
        //bucketLetters[3] = 'D';
        //bucketLetters[4] = 'E';
        //bucketLetters[5] = 'F';

        StartCoroutine(GreenSpawn());
        StartCoroutine(RedSpawn());

    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    
    public IEnumerator GreenSpawn()
    {
        
        yield return new WaitForSeconds(2f);
        
        int setLen = givenSetOfWords.Count;

        for (int i = 0; i < setLen; i++)
        {
            char[] wordAlphabets = givenSetOfWords[i].ToCharArray();
            int wordLen = givenSetOfWords[i].Length;
            for (int j = 0; j < wordLen; j++)
            {
                if (!preferredAlphabetIndexes.Contains(wordAlphabets[j] - 'A'))
                {
                    preferredAlphabetIndexes.Add(wordAlphabets[j] - 'A');
                }
            }
        }
        
        for(int i = 0; i < 26; i++)
        {
            if(!preferredAlphabetIndexes.Contains(i))
            {
                incorrectLetterList.Add(i);
            }
        }
        
        while (enabled)
        {
            
            givenSetOfWords = FindObjectOfType<Level4GameManager>().GetCurrentWords();
            bucketLetters = FindObjectOfType<Level4GameManager>().GetBucketList();
        
            GameObject prefab;

            if(Random.value < givenWordLettersPrefabsProb && preferredAlphabetIndexes.Count > 0)
            {
                prefab = bubblePrefabs[preferredAlphabetIndexes[Random.Range(0, preferredAlphabetIndexes.Count)]];
                CreateBubble(prefab, 0); //Green
                SpawnManager.numOfCorrectBubblesSpawned++;
            }
            else
            {
                prefab = bubblePrefabs[incorrectLetterList[Random.Range(0, incorrectLetterList.Count)]];
                CreateBubble(prefab, 0); //Green
            }

            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }
    
    public IEnumerator RedSpawn()
    {
        
        yield return new WaitForSeconds(2f);

        while (enabled)
        {
            givenSetOfWords = FindObjectOfType<Level4GameManager>().GetCurrentWords();
            bucketLetters = FindObjectOfType<Level4GameManager>().GetBucketList();
        
            GameObject prefab;

            if (bucketLetters.Count > 3)
            {
                var sequence = Enumerable.Range( 0 , bucketLetters.Count ).OrderBy( n => n * n * ( new System.Random() ).Next() );
                var result = sequence.Distinct().Take( 3 );

                foreach (int num in result)
                {
                    prefab = bubblePrefabs[(int)bucketLetters[num] - (int)('A')];
                    CreateBubble(prefab, 1); //Red
                }
            } else if (bucketLetters.Count > 2)
            {
                var sequence = Enumerable.Range( 0 , bucketLetters.Count ).OrderBy( n => n * n * ( new System.Random() ).Next() );
                var result = sequence.Distinct().Take( 2 );

                foreach (int num in result)
                {
                    prefab = bubblePrefabs[(int)bucketLetters[num] - (int)('A')];
                    CreateBubble(prefab, 1); //Red
                }
            }
            else if (bucketLetters.Count > 0)
            {
                var sequence = Enumerable.Range( 0 , bucketLetters.Count ).OrderBy( n => n * n * ( new System.Random() ).Next() );
                var result = sequence.Distinct().Take( 1 );

                foreach (int num in result)
                {
                    prefab = bubblePrefabs[(int)bucketLetters[num] - (int)('A')];
                    CreateBubble(prefab, 1); //Red
                }
            }
            
            yield return new WaitForSeconds(Random.Range(3f, 4f));
        }
    }

    private void CreateBubble(GameObject prefab, int colorIndex)
    {
        Vector3 position = new Vector3();
        position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
            
        GameObject bubble = Instantiate(prefab, position, rotation);
        SpawnManager.numOfBubblesSpawned++;
        Destroy(bubble, maxLifetime);

        float force = Random.Range(minForce, maxForce);
        bubble.GetComponent<Rigidbody>().AddForce(bubble.transform.up * force, ForceMode.Impulse);
            
        bubble.GetComponent<Renderer>().material.color = colors[colorIndex];
    }
    
    /*// Remove letter from list of high probability spawn letters.
    public void RemoveLetter(string letter){
        for(int i=0; i<preferredAlphabetIndexes.Count;i++){

            if(((char)('A' + preferredAlphabetIndexes[i])).ToString() == letter){
                preferredAlphabetIndexes.RemoveAt(i);
                break;
            }
        }
    }
    
    // Add letter from to of high probability spawn letters.
    public void AddLetter(string letter)
    {
        preferredAlphabetIndexes.Add(char.Parse(letter) - 'A');
    }
    
    public void UpdateSetOfWords(List<string> setOfWords)
    {
        givenSetOfWords = setOfWords;
        int setLen = givenSetOfWords.Count;
        preferredAlphabetIndexes.Clear();

        for (int i = 0; i < setLen; i++)
        {
            char[] wordAlphabets = givenSetOfWords[i].ToCharArray();
            int wordLen = givenSetOfWords[i].Length;
            for (int j = 0; j < wordLen; j++)
            {
                if (!preferredAlphabetIndexes.Contains(wordAlphabets[j] - 'A'))
                {
                    preferredAlphabetIndexes.Add(wordAlphabets[j] - 'A');
                }
            }
        }
         
        for(int i = 0; i < 26; i++)
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
    }*/
}
