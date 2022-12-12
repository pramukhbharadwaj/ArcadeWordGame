using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Level4GameManager : MonoBehaviour
{
    private int WaitTime = 3;
    public List<string> WordList,currWordList;
    public List<char> bucketChars = new List<char>();
    public HashSet<char> totalChars = new HashSet<char>();
    public List<string> solvedList = new List<string>();
    public List<string> solvedList2 = new List<string>();
    
    public TMP_Text Timertext;

    private bool finished = false;

    public GameObject temp, bucketObject;
    public TMP_Text scoreText;
    
    HeartSystem heartSystem;
    [SerializeField] GameObject player; //Blade object to which the HeartSystem script is attached
    [SerializeField] GameObject floatingTextPrefab;

    public List<List<TMP_Text>> wordHolderList = new List<List<TMP_Text>>();
    public List<TMP_Text> bubbleHolderList = new List<TMP_Text>();
    // public List<TMP_Text> wordHolder2List = new List<TMP_Text>();
    // public List<TMP_Text> wordHolder3List = new List<TMP_Text>();
    // public List<TMP_Text> wordHolder4List = new List<TMP_Text>();
    // public List<TMP_Text> wordHolder5List = new List<TMP_Text>();n
    public GameObject emptyPrefab;
    public List<Transform> wordHolder;
    public Transform bubbleHolder;

    void Awake()
    {
        wordHolderList.Add(new List<TMP_Text>());
        wordHolderList.Add(new List<TMP_Text>());
        wordHolderList.Add(new List<TMP_Text>());
        wordHolderList.Add(new List<TMP_Text>());
        wordHolderList.Add(new List<TMP_Text>());
        heartSystem = player.GetComponent<HeartSystem>(); //Getting the HeartSystem script which is attached to the 'player' game object

        WordList.Add("NEST");
        WordList.Add("STAR");
        WordList.Add("RAIN");
        WordList.Add("TINY");
        WordList.Add("MESH");

        for (int i = 1 ; i <= 5; i++)
        {
            int index = Random.Range(0, WordList.Count);
            string RandomWord = WordList[index].ToUpper().Trim();
            currWordList.Add(RandomWord);
            WordList.RemoveAt(index);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        Renderer(currWordList,bucketChars);
    }


    void Update()
    {   
        if(finished==false && TimeManager.timeRemaining > 0){
            StartCoroutine(TimerTake());
        }
    }

    public void Initialize(){

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                temp = Instantiate(emptyPrefab, wordHolder[i]);
                wordHolderList[i].Add(temp.GetComponent<TMP_Text>());
            }
        }
        InitializeTimerText();
    }

    public void InitializeTimerText() {
        string minutes = (TimeManager.timeRemaining/ 60).ToString();
        string seconds = (TimeManager.timeRemaining % 60).ToString("f0");
        if(seconds=="0")
            seconds = "00";
        Timertext.text = "0"+ minutes + ":"+ seconds;
    }

    IEnumerator TimerTake()
    {
        finished =true;
        yield return new WaitForSeconds(1);
        TimeManager.timeRemaining-=1;

        string minutesString = ((int) TimeManager.timeRemaining/ 60).ToString();
        int seconds = (TimeManager.timeRemaining % 60);
        string secondsString = seconds.ToString("f0");

        if(seconds < 10)
            Timertext.text = "0"+minutesString + ":0"+ seconds;
        else
            Timertext.text = "0"+minutesString + ":"+ seconds;
                
        if(TimeManager.timeRemaining<1){
            Timertext.text = "";
            SceneManager.LoadScene("LevelFinished");
        }

        finished = false;
    }

    void Renderer(List<string> currWordList, List<char> bucketChars)
    {
        int i = 0;
        foreach(string word in currWordList)
        {
            if (IsWordCompleted(word, bucketChars))
            {
                int size = wordHolderList[i].Count;
                while (size > 0)
                {
                    Destroy(wordHolderList[i][size-1].gameObject);
                    size--;
                }
                wordHolderList.RemoveAt(i);
                currWordList.RemoveAt(i);
                // int index = Random.Range(0, WordList.Count);
                // string RandomWord = WordList[index].ToUpper().Trim();
                // currWordList[i] = RandomWord;
                // WordList.RemoveAt(index);
                ScoreManager.IncreaseScore(50);
                scoreText.text = "Score: " + ScoreManager.score.ToString();
                showScoreIncrement("+50");
                Renderer(currWordList, bucketChars);
                return;
            }
            int j=0;
            // Debug.Log(word);
            string s = "";
            int highlightCount = 0;
            foreach (char c in word)
            {
                if(bucketChars.Contains(c))
                {
                    wordHolderList[i][j].text = "<mark=#00FF0080>" + c.ToString() + "</mark>";
                }
                else
                {
                    // Debug.Log("i:" + i);
                    // Debug.Log("j:" + j);
                    wordHolderList[i][j].text=c.ToString();   
                }

                j++;
            }
            i++;
            // Debug.Log(l);
        }
    }

    Boolean IsWordCompleted(String word, List<char> bucketChars)
    {
        foreach (char c in word)
        {
            if (!bucketChars.Contains(c))
            {
                return false;
            }
        }
        return true;
    }
    void OnTriggerEnter(Collider player)
    {
        // Debug.Log("GameObject TAG:" + player.gameObject.tag);
        if (player.gameObject.tag == "Text")
        {
            totalChars.Clear();
            foreach (string word in currWordList)
            {
                foreach (char c in word)
                {
                    totalChars.Add(c);
                }
            }

            string slicedLetter = player.gameObject.name[0].ToString();
            Color playerColor = player.gameObject.GetComponent<Renderer>().material.color;
            Color redColor = new Color(1, 0, 0, 1);
            Color greenColor = new Color(0, 1, 0, 1);
            if (playerColor == greenColor)
            {
                if (bucketChars.Count <= 5)
                {
                        bucketChars.Add(char.Parse(slicedLetter));
                        int count = bubbleHolderList.Count;
                        temp = Instantiate(emptyPrefab, bubbleHolder);
                        bubbleHolderList.Add(temp.GetComponent<TMP_Text>());
                        bubbleHolderList[count].text = slicedLetter;
                        bubbleHolderList[count].alignment = TextAlignmentOptions.Center;
                        Renderer(currWordList,bucketChars);
                }
                else
                {
                    StartCoroutine("DisplayBucketFull");

                }
            }

            if (playerColor == redColor)
            {
                if (bucketChars.Count != 0)
                {
                    int i = 0;
                    if (bucketChars.Contains(char.Parse(slicedLetter)))
                    {
                        i = bucketChars.IndexOf(char.Parse(slicedLetter));
                        Destroy(bubbleHolderList[i].gameObject);
                        bubbleHolderList.RemoveAt(i);
                        bucketChars.RemoveAt(i);
                        Renderer(currWordList, bucketChars);
                    }
                }
            }
        }
    }

    public List<char> GetBucketList()
    {
        return bucketChars;
    }

    public List<String> GetCurrentWords()
    {
        return currWordList;
    }

    public void showScoreIncrement(string text){
        if(floatingTextPrefab)
        {
            GameObject prefab = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TMP_Text>().text = text;
        }
    }
    
    IEnumerator DisplayBucketFull()
    {
        bucketObject.SetActive(true);
        yield return new WaitForSeconds(WaitTime);
        bucketObject.SetActive(false);
    }

    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(WaitTime);
    }
}
