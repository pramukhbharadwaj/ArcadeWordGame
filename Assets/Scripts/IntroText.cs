using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class IntroText : MonoBehaviour
{
    public TMP_Text initialText,loglineText, instructionText, successMessageText;
    private int WaitTime = 5, successMessageWaitTime = 3;
    public GameObject uiObject, instObject, instructionObject, successMessageObject, nextWordMessageObject;
    public GameObject playButton, reloadButton, pauseButton, homeButton, panel, blade, exitButton;
    public Button buttonToHide, buttonToHide1, buttonToHide2;
    public List<string> WordList = new List<string>();
    [SerializeField] GameObject floatingTextPrefab;
    [SerializeField] GameObject incorrectSlicePrefab;

    public AudioSource triggerSound;
    public GameObject genreCanvas;

    public Transform sparkle;
    private bool toggleSound=false;
    private void Awake()
    {
        Initialize();
        triggerSound = GetComponent<AudioSource>();
    }

    void Initialize()
    {
        // Debug.Log("Awake Invoked");
        
        if (SceneManager.GetActiveScene().name == "SliceItOff" || SceneManager.GetActiveScene().name == "Level3_updated")
        {
            instructionText.text = "Guess the word and slice only the required letters!";
            StartCoroutine("DisplayInstructions");

            WordList = LevelAndGenreManager.GetWordList();
            

        } else if (SceneManager.GetActiveScene().name == "Level2")
        {
            instructionText.text = "Guess the word, Slice colored bubble multiple times to get rewarded!";
            StartCoroutine("DisplayInstructions");

            WordList = LevelAndGenreManager.GetWordList();
            
        } else if (SceneManager.GetActiveScene().name == "Level3-2")
        {
            instructionText.text = "Guess the word, Slice colored bubble multiple times and destroy obstacle!";
            StartCoroutine("DisplayInstructions");

            WordList = LevelAndGenreManager.GetWordList();
            
        }
        else if (SceneManager.GetActiveScene().name == "Level3")
        {
            instructionText.text = "Guess the word, Match the color and slice to score more!";
            StartCoroutine("DisplayInstructions");

            WordList = LevelAndGenreManager.GetWordList();

        }else if (SceneManager.GetActiveScene().name is "Level4" or "Level4-2")
        {
            instructionText.text = "Guess the word, Freeze and slice!";
            StartCoroutine("DisplayInstructions");

            WordList = LevelAndGenreManager.GetWordList();

        }
        else if (SceneManager.GetActiveScene().name is "Level5" or "Level5-2")
        {
            instructionText.text = "Guess the word, Match the color and slice to score more!";
            StartCoroutine("DisplayInstructions");

            WordList = LevelAndGenreManager.GetWordList();

        }
        else if (SceneManager.GetActiveScene().name is "Level6")
        {
            instructionText.text = "Guess the word, Don't miss any of the correct letters!";
            StartCoroutine("DisplayInstructions");

            WordList = LevelAndGenreManager.GetWordList();

        }
        else if (SceneManager.GetActiveScene().name is "Level1-Tutorial" or "Level5-Tutorial" or "Level2-Tutorial" or "Level3-Tutorial" or "Level4-Tutorial" or "Level6-Tutorial" or "CannonLevel1-Tutorial" or "CannonLevel3-Tutorial" or "CannonLevel4-Tutorial")
        {
            WordList.Add("CAT");

        }
        else if (SceneManager.GetActiveScene().name is "CannonLevel1")
        {
            instructionText.text = "Guess the word and shoot only the required letters!!";
            loglineText.text = "Guess & Shoot Correct Bubbles";
            StartCoroutine("DisplayInstructions");
            WordList = LevelAndGenreManager.GetWordList();
        }
        else if (SceneManager.GetActiveScene().name is "CannonLevel4")
        {
            instructionText.text = "Guess the word, Freeze and shoot!";
            loglineText.text = "Guess, Freeze & Shoot Correct Bubbles";
            StartCoroutine("DisplayInstructions");
            WordList = LevelAndGenreManager.GetWordList();
        }
        else if (SceneManager.GetActiveScene().name is "CannonLevel3")
        {
            instructionText.text = "Guess the word, Shoot colored bubble to split it into multiple bubbles!";
            loglineText.text = "Guess & Shoot Correct Bubbles and special bubble";
            StartCoroutine("DisplayInstructions");
            WordList = LevelAndGenreManager.GetWordList();
        }
    
        int index = Random.Range(0, WordList.Count);
        string RandomWord = WordList[index].ToUpper().Trim();
        Debug.Log("Random Word:" + RandomWord);
        initialText.text = RandomWord;
        if(loglineText.text.Equals("")) loglineText.text = "Guess & Slice It Off";
        WordList.RemoveAt(index);

    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine("DisplayLogLine");
        //StartCoroutine("DisplayWord");
    }

    void OnTriggerEnter(Collider player)
    {
        if (SceneManager.GetActiveScene().name.Contains("Cannon") && toggleSound == false)
        {
            toggleSound = true;
        }
        else
        {
            triggerSound.Play();
        }
            
        
        if (player.gameObject.tag == "SpecialBubble"){
            if(FindObjectOfType<NewBehaviourScript>().get_special_bubble_life()==0){
                showScoreIncrement("+50");
            }
            if (SceneManager.GetActiveScene().name == "Level2-Tutorial")
            {
                FindObjectOfType<TutorialSpawner>().HideAnim();
                StartCoroutine(FindObjectOfType<TutorialSpawner>().DisplayLevelComplete());
            }

        }
        if (player.gameObject.tag == "Text")
        {
            string slicedLetter = player.gameObject.name[0].ToString();
            Color playerColor = player.gameObject.GetComponent<Renderer>().material.color;
            
            if(FindObjectOfType<GameManager>().TestLetter(slicedLetter)){
                if ((SceneManager.GetActiveScene().name is "Level5" or "Level5-2" or "Level5-Tutorial") && playerColor != FindObjectOfType<GameManager>().getSparkleColor()){
                    showScoreIncrement("+5");
                    if (SceneManager.GetActiveScene().name == "Level5-Tutorial")
                    {
                        FindObjectOfType<TutorialSpawner>().HideAnimIncorrect();
                        StartCoroutine(FindObjectOfType<TutorialSpawner>().DisplayLevelComplete());
                    }
                }
                else{
                    showScoreIncrement("+10");
                    if (SceneManager.GetActiveScene().name == "Level4-Tutorial")
                    {
                        FindObjectOfType<TutorialSpawner>().HideLevel4Anim();
                        StartCoroutine(FindObjectOfType<TutorialSpawner>().DisplayLevelComplete());
                    }
                    if (SceneManager.GetActiveScene().name is "Level1-Tutorial" or "Level5-Tutorial")
                    {
                        Debug.Log("Calling HideAnim");
                        FindObjectOfType<TutorialSpawner>().HideAnim();
                    }

                    if (SceneManager.GetActiveScene().name is "Level6-Tutorial")
                    {
                        if (FindObjectOfType<TutorialSpawner>().getSpaceKeyPressed())
                        {
                            StartCoroutine("ShowMessageAndSwitchLevel");
                        }
                    }
                    
                }
            }else if (!FindObjectOfType<GameManager>().isLetterInWord(slicedLetter)) {
                showIncorrectSliceDamage(slicedLetter);
                if (SceneManager.GetActiveScene().name is "Level1-Tutorial")
                {
                    // Debug.Log("In L1 Tutorial");
                    FindObjectOfType<TutorialSpawner>().HideAnimIncorrect();
                    StartCoroutine(FindObjectOfType<TutorialSpawner>().DisplayLevelComplete());
                }
            }

            bool isLetterComplete = FindObjectOfType<GameManager>().CheckLetter(slicedLetter, playerColor, 0);
            if(isLetterComplete) {
                Debug.Log("letter complete: " + slicedLetter);
                if (FindObjectOfType<BasicSpawner>())
                    FindObjectOfType<BasicSpawner>().RemoveLetter(slicedLetter);
                if (FindObjectOfType<MultiSliceBubbleSpawner>())
                    FindObjectOfType<MultiSliceBubbleSpawner>().RemoveLetter(slicedLetter);
                if (FindObjectOfType<BurstSpawner>())
                    FindObjectOfType<BurstSpawner>().RemoveLetter(slicedLetter);
                if (FindObjectOfType<ColorSpawner>())
                    FindObjectOfType<ColorSpawner>().RemoveLetter(slicedLetter);
                if (FindObjectOfType<BurstColorSpawner>())
                    FindObjectOfType<BurstColorSpawner>().RemoveLetter(slicedLetter);
                if (FindObjectOfType<LeftSpawner>()){
                    Debug.Log("in intro text, remove letter.");
                    FindObjectOfType<LeftSpawner>().RemoveLetter(slicedLetter);
                }
                if (FindObjectOfType<RightSpawner>()){
                    Debug.Log("in intro text, remove letter.");
                    FindObjectOfType<RightSpawner>().RemoveLetter(slicedLetter);
                }
            }   

            if(FindObjectOfType<GameManager>().isFinished()){
                showScoreIncrement("+50");
                if (WordList.Count == 0)
                {
                    //FindObjectOfType<GameManager>().clearHolder();
                    SceneManager.LoadScene("LevelFinished");
                    //Debug.Log("GAME OVER");
                }
                else
                {
                    //FindObjectOfType<GameManager>().clearHolder();
                    int index = Random.Range(0, WordList.Count);
                    string RandomWord = WordList[index].ToUpper().Trim();
                    initialText.text = RandomWord;
                    WordList.RemoveAt(index);
                    // StartCoroutine("DisplayWord");
                    StartCoroutine("DisplaySuccessMessage");
                    if (FindObjectOfType<BasicSpawner>())
                        FindObjectOfType<BasicSpawner>().UpdateWord(RandomWord);
                    if (FindObjectOfType<ColorSpawner>())
                        FindObjectOfType<ColorSpawner>().UpdateWord(RandomWord);
                    if (FindObjectOfType<BurstSpawner>())
                        FindObjectOfType<BurstSpawner>().UpdateWord(RandomWord);
                    if (FindObjectOfType<MultiSliceBubbleSpawner>())
                        FindObjectOfType<MultiSliceBubbleSpawner>().UpdateWord(RandomWord);
                    if (FindObjectOfType<BurstColorSpawner>())
                        FindObjectOfType<BurstColorSpawner>().UpdateWord(RandomWord);
                    if (SceneManager.GetActiveScene().name is "CannonLevel1" or "CannonLevel4"or "CannonLevel3"){
                        Debug.Log("left & right spawner update word.");
                        FindObjectOfType<LeftSpawner>().UpdateWord(RandomWord);
                        FindObjectOfType<RightSpawner>().UpdateWord(RandomWord);
                    }
                    FindObjectOfType<GameManager>().Initialize();
                }
                
            }
            
            

        }
    }
    void OnTriggerExit(Collider player)
    {
        //uiObject.SetActive(false);
    }

    IEnumerator ShowMessageAndSwitchLevel()
    {
        StartCoroutine(FindObjectOfType<TutorialSpawner>().DisplayLevel6Complete());
        yield return new WaitForSeconds(5f);
        LevelAndGenreManager.levelName = "Level6";
        TimeManager.Clear();
        SceneManager.LoadScene("Genre");
    }

    IEnumerator DisplaySuccessMessage()
    {
        List<string> successMessage = new List<string>();
        successMessage.Add("Good Job!");
        successMessage.Add("Well Done!");
        successMessage.Add("Brilliant!");
        
        successMessageText.text = successMessage[Random.Range(0, successMessage.Count)];
        
        successMessageObject.SetActive(true);
        nextWordMessageObject.SetActive(true);
        yield return new WaitForSeconds(successMessageWaitTime);
        successMessageObject.SetActive(false);
        nextWordMessageObject.SetActive(false);
    }
    
    IEnumerator DisplayWord()
    {
        uiObject.SetActive(true);
        yield return new WaitForSeconds(WaitTime);
        uiObject.SetActive(false);
    }
 
    IEnumerator DisplayLogLine()
    {
        instObject.SetActive(true);
        yield return new WaitForSeconds(WaitTime);
        instObject.SetActive(false);
    }
    
    public void DisplayInstructions()
    {
        instructionObject.SetActive(true);
        buttonToHide.gameObject.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
        if (SceneManager.GetActiveScene().name == "Level2")
            FindObjectOfType<Blade>().DisableBladeTrail();
    }

    public void HideInstructions()
    {
        Time.timeScale = 1;
        pauseButton.SetActive(true);
        instructionObject.SetActive(false);
        buttonToHide.gameObject.SetActive(false);
        StartCoroutine("DisplayLogLine");
        // StartCoroutine("DisplayWord");
        if (SceneManager.GetActiveScene().name == "Level2")
            FindObjectOfType<Blade>().EnableBladeTrail();
    }
    
    public void showScoreIncrement(string text){
        if(floatingTextPrefab)
        {
            GameObject prefab = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TMP_Text>().text = text;
        }
    }

    public void showIncorrectSliceDamage(string text){
        if(floatingTextPrefab)
        {
            GameObject prefab = Instantiate(incorrectSlicePrefab, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TMP_Text>().text = text;
        }
    }

    public void onPauseClick()
    {
        panel.SetActive(true);
        blade.SetActive(false);
        pauseButton.SetActive(false);
        exitButton.SetActive(false);
        Time.timeScale = 0;
    }
    public void onPlayClick()
    {
        Time.timeScale = 1;
        panel.SetActive(false);
        blade.SetActive(true);
        pauseButton.SetActive(true);
        exitButton.SetActive(true);
    }

    public void onHomeClick()
    {
        ScoreManager.Clear();
        UserNumberManager.Clear();
        TimeManager.Clear();
        SpawnManager.Clear();
        LivesManager.Clear();
        blade.SetActive(true);
        exitButton.SetActive(true);
        SceneManager.LoadScene("HomeScene");
    }

    public void onReloadClick()
    {
        ScoreManager.Clear();
        UserNumberManager.Clear();
        TimeManager.Clear();
        SpawnManager.Clear();
        LivesManager.Clear();
        blade.SetActive(true);
        exitButton.SetActive(true);
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);
    }
    public void ChangeWord()
    {
        if (WordList.Count == 0)
        {
            //FindObjectOfType<GameManager>().clearHolder();
            SceneManager.LoadScene("LevelFinished");
            //Debug.Log("GAME OVER");
        }
        else
        {
            //FindObjectOfType<GameManager>().clearHolder();
            int index = Random.Range(0, WordList.Count);
            string RandomWord = WordList[index].ToUpper().Trim();
            initialText.text = RandomWord;
            WordList.RemoveAt(index);
            // StartCoroutine("DisplayWord");
            StartCoroutine("DisplaySuccessMessage");
            // if (FindObjectOfType<BasicSpawner>())
            //     FindObjectOfType<BasicSpawner>().UpdateWord(RandomWord);
            // if (FindObjectOfType<ColorSpawner>())
            //     FindObjectOfType<ColorSpawner>().UpdateWord(RandomWord);
            // if (FindObjectOfType<BurstSpawner>())
            //     FindObjectOfType<BurstSpawner>().UpdateWord(RandomWord);
            // if (FindObjectOfType<MultiSliceBubbleSpawner>())
            //     FindObjectOfType<MultiSliceBubbleSpawner>().UpdateWord(RandomWord);
            // if (FindObjectOfType<BurstColorSpawner>())
            //     FindObjectOfType<BurstColorSpawner>().UpdateWord(RandomWord);
            if (SceneManager.GetActiveScene().name is "CannonLevel1" or "CannonLevel4"or "CannonLevel3"){
                Debug.Log("left & right spawner update word.");
                FindObjectOfType<LeftSpawner>().UpdateWord(RandomWord);
                FindObjectOfType<RightSpawner>().UpdateWord(RandomWord);

            }
            FindObjectOfType<GameManager>().Initialize();
        }
    } 
    public void showSpikeDamage(string text, Vector3 pos){
        if(incorrectSlicePrefab)
        {
            GameObject prefab = Instantiate(incorrectSlicePrefab, pos, Quaternion.identity);
            prefab.GetComponentInChildren<TMP_Text>().text = text;
        }
    }
}