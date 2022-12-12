using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
// using UnityEditor.Experimental.GraphView;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public List<string> solvedList = new List<string>();
    // Use - don't fill incorrect orb if the correct letter is sliced whose orb is filled
    public List<string> characterList = new List<string>();
    public TMP_Text initialText;

    public GameObject ui;
    int old_index = -1;
    private int colorIndex = -1;

    public TMP_Text Timertext;
    
    private bool finished = false;
    private int unsolvedCount;

    public GameObject temp;
    
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Transform sparkle;
    public int maxColorTimerValue = 10;
    public int colorChangeSecondsLeft;
    public GameObject healthBar;
    public TrailRenderer bladeTrail;

    public TMP_Text scoreText;

    public Transform orbHolder;
	public GameObject orb;
    public List<GameObject> orbList = new List<GameObject>();
    public GameObject incorrectOrb;
    public GameObject orbClone;

    public List<Image> colorBorders;

    private bool initializedOnce=false;
    
    HeartSystem heartSystem;
    [SerializeField] GameObject player; //Blade object to which the HeartSystem script is attached    
    
    void Awake(){
        heartSystem = player.GetComponent<HeartSystem>(); //Getting the HeartSystem script which is attached to the 'player' game object
        UserNumberManager.time_taken_for_death = 60;
        UserNumberManager.level_of_game_over = "";
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        initializedOnce = true;
    }

    void Update()
    {   
        if(finished==false && TimeManager.timeRemaining > 0){
            StartCoroutine(TimerTake());
        }
        //Debug.Log("Inside update");
        if ((SceneManager.GetActiveScene().name is "Level5" or "Level5-2" or "Level5-Tutorial")  && Input.GetKeyDown("tab"))
        {
            Color[] colors = FindObjectOfType<ColorSpawner>().GetColors();
            colorBorders[colorIndex].gameObject.SetActive(false);
            if (colorIndex == colors.Length - 1)
                colorIndex = 0;
            else
                colorIndex++;
            Color32 newColor = colors[colorIndex];
            sparkle.GetComponent<ParticleSystem>().startColor = newColor;
            bladeTrail.startColor = newColor;
            bladeTrail.endColor = newColor;
            fill.color = newColor;
            colorBorders[colorIndex].gameObject.SetActive(true);
        }
		
    }

    public void Initialize(){
        string tempWord = initialText.text.Trim();
        unsolvedCount = tempWord.Length;
        foreach(char l in tempWord){
            solvedList.Add(l.ToString());
            characterList.Add(l.ToString());
            // Debug.Log(l);
        }
        
        RectTransform rt = (RectTransform)orbHolder.transform;

        for(int i=0; i<solvedList.Count; i++){
            //temp = Instantiate(letterPrefab, letterHolder);
            //letterHolderList.Add(temp.GetComponent<TMP_Text>());
            
            //orbClone = Instantiate(orb, new Vector3((i+1.5f) * rt.rect.width, rt.rect.height, 0f), Quaternion.identity, orbHolder);
            //Debug.Log(i);
            //Debug.Log(Screen.width);

            if (SceneManager.GetActiveScene().name is "Level6" or "Level6-Tutorial")
            {
                orbClone = Instantiate(orb, new Vector3( (i+1) * Screen.width / 20, Screen.height / 7.5f, 0f), Quaternion.identity, orbHolder);
            }
            else
            {
                orbClone = Instantiate(orb, new Vector3( (i+1) * Screen.width / 20, Screen.height / 12, 0f), Quaternion.identity, orbHolder);
            }
            orbClone.SetActive(true);
            
            orbClone.GetComponentsInChildren<Image>()[0].sprite = Resources.Load<Sprite>("Sprites/" + solvedList[i] + "_Border");
            orbClone.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("Sprites/" + solvedList[i] + "_Fill");
            orbClone.GetComponentsInChildren<Image>()[2].sprite = Resources.Load<Sprite>("Sprites/Blank");
            orbList.Add(orbClone);

        }

        if (LevelAndGenreManager.levelName.EndsWith("Tutorial"))
        {
            List<int> indexes = new List<int>();
            indexes.Add(0);

            foreach (var index in indexes)
            {
                orbList[index].GetComponentsInChildren<Image>()[0].fillAmount = 0f;
                orbList[index].GetComponentsInChildren<Image>()[2].fillAmount = 1f;
            }
        }
        else
        {
            int orbListCount = orbList.Count;
            int visibleOrbCount = (int)Math.Round(0.6f * orbListCount, 0);
            List<int> visibleOrbIndexes = new List<int>();
            while (visibleOrbIndexes.Count < visibleOrbCount)
            {
                int index = Random.Range(0, orbListCount - 1);
                while (visibleOrbIndexes.Contains(index))
                    index = Random.Range(0, orbListCount - 1);
                orbList[index].GetComponentsInChildren<Image>()[0].fillAmount = 0f;
                orbList[index].GetComponentsInChildren<Image>()[2].fillAmount = 1f;
                visibleOrbIndexes.Add(index);
            }
        }

        InitializeTimerText();


        if (SceneManager.GetActiveScene().name is "Level1-Tutorial" )
        {
            if(!initializedOnce)
            SpawnManager.initializeAndSpawn(
                false, 
                false, 
                false, 
                false, 
                true,           // tutorial spawner
                false, 
                false);        
        }
        else if(SceneManager.GetActiveScene().name is "SliceItOff")
        {
            SpawnManager.spawnDelayBasicSpawner = 2.5F;

            if(!initializedOnce)
            SpawnManager.initializeAndSpawn(
                true,           // basic spawner
                false, 
                false, 
                false, 
                false, 
                false, 
                false);
        }
        else if(SceneManager.GetActiveScene().name is "Level3_updated")
        {
            SpawnManager.spawnDelayBasicSpawner = 2.5F;
            SpawnManager.minSpawnDelayObstacleSpawner = 15F;
            SpawnManager.maxSpawnDelayObstacleSpawner = 25F;

            if(!initializedOnce)
            SpawnManager.initializeAndSpawn(
                true,             // basic spawner
                false, 
                false, 
                false, 
                false, 
                true,           // obstacle spawner
                false);
        }
        else if (SceneManager.GetActiveScene().name == "Level3-Tutorial")
        {
            if(!initializedOnce)
            SpawnManager.initializeAndSpawn(
                false,
                false, 
                false, 
                false, 
                true,               // Tutorial Spawner
                false,
                false);        
        }
        else if(SceneManager.GetActiveScene().name == "Level2")
        {
            SpawnManager.spawnDelayBasicSpawner = 2.5F;
            SpawnManager.minSpawnDelayMultiSliceSpawner = 10F;
            SpawnManager.maxSpawnDelayMultiSliceSpawner = 15F;

            //change here as per needed
            if(!initializedOnce)
            SpawnManager.initializeAndSpawn(
                true,           // basic spawner
                true,          // multislice spawner
                false, 
                false, 
                false, 
                false, 
                false);
        }
        else if(SceneManager.GetActiveScene().name == "Level3-2")
        {
            SpawnManager.spawnDelayBasicSpawner = 2.5F;
            SpawnManager.minSpawnDelayMultiSliceSpawner = 15F;
            SpawnManager.maxSpawnDelayMultiSliceSpawner = 25F;
            SpawnManager.minSpawnDelayObstacleSpawner = 15F;
            SpawnManager.maxSpawnDelayObstacleSpawner = 25F;
            //change here as per needed
            if(!initializedOnce)
            SpawnManager.initializeAndSpawn(
                true,       // basic spawner
                true,       // multislice spawner
                false, 
                false, 
                false, 
                true,       // obstacle spawner
                false);
        }
        else if (SceneManager.GetActiveScene().name == "Level2-Tutorial")
        {
            if(!initializedOnce)
            SpawnManager.initializeAndSpawn(
                false, 
                false, 
                false, 
                false, 
                true,         // Tutorial Spawner
                false, 
                false);                
        }
        else if(SceneManager.GetActiveScene().name == "Level4")
        {
            SpawnManager.spawnDelayBasicSpawner = 2.5F;
            if(!initializedOnce)
            SpawnManager.initializeAndSpawn(
                true,           // basic spawner
                false, 
                true,           // burst spawner
                false,
                false, 
                false, 
                false);
        }
        else if(SceneManager.GetActiveScene().name == "Level4-2")
        {
            SpawnManager.spawnDelayBasicSpawner = 2.5F;
            SpawnManager.minSpawnDelayObstacleSpawner = 15F;
            SpawnManager.maxSpawnDelayObstacleSpawner = 25F;

            if(!initializedOnce)
            SpawnManager.initializeAndSpawn(
            true,           // basic
            false,
            true,           // burst
            false, 
            false, 
            true,           // obstacle spawner
            false);
        }        
        else if (SceneManager.GetActiveScene().name is "Level5")
        {
            // Debug.Log("Inside Initialize");
            // Debug.Log("colorIndex");
            if (colorIndex == -1)
            {
                Color[] colors = FindObjectOfType<ColorSpawner>().GetColors();
                colorIndex = Random.Range(0, colors.Length-1);
                Color32 newColor = colors[colorIndex];
                sparkle.GetComponent<ParticleSystem>().startColor = newColor;
                bladeTrail.startColor = newColor;
                bladeTrail.endColor = newColor;
                fill.color = newColor;
                colorBorders[colorIndex].gameObject.SetActive(true);
            }

            SpawnManager.spawnDelayColorSpawner = 4F;

            if(!initializedOnce)
            SpawnManager.initializeAndSpawn(
                false,
                false, 
                false,      
                true,           // Color Spawner
                false, 
                false, 
                false);
        }
        else if (SceneManager.GetActiveScene().name is "Level5-2")
        {
            // Debug.Log("Inside Initialize");
            // Debug.Log("colorIndex");
            if (colorIndex == -1)
            {
                Color[] colors = FindObjectOfType<ColorSpawner>().GetColors();
                colorIndex = Random.Range(0, colors.Length-1);
                Color32 newColor = colors[colorIndex];
                sparkle.GetComponent<ParticleSystem>().startColor = newColor;
                bladeTrail.startColor = newColor;
                bladeTrail.endColor = newColor;
                fill.color = newColor;
                colorBorders[colorIndex].gameObject.SetActive(true);
            }
            

            SpawnManager.spawnDelayColorSpawner = 4F;


            if(!initializedOnce)
            SpawnManager.initializeAndSpawn(
                false,
                false, 
                false,
                true,           // Color Spawner
                false, 
                false, 
                true);          // burst color spawner);
        }
        else if(SceneManager.GetActiveScene().name is "Level6")
        {
            SpawnManager.spawnDelayBasicSpawner = 2.5F;

            if(!initializedOnce)
                SpawnManager.initializeAndSpawn(
                    true,           // basic spawner
                    false, 
                    false, 
                    false, 
                    false, 
                    false, 
                    false);
        }
        else if (SceneManager.GetActiveScene().name == "Level5-Tutorial")
        {
            if (colorIndex == -1)
            {
                Color[] colors = FindObjectOfType<ColorSpawner>().GetColors();
                colorIndex = 1;
                Color32 newColor = colors[colorIndex];
                sparkle.GetComponent<ParticleSystem>().startColor = newColor;
                bladeTrail.startColor = newColor;
                bladeTrail.endColor = newColor;
                fill.color = newColor;
                colorBorders[colorIndex].gameObject.SetActive(true);
            }
            
            if(!initializedOnce)
            SpawnManager.initializeAndSpawn(
                false,
                false,
                false, 
                false, 
                true,           // tutorial spawner
                false,
                false);                
        }
        else if (SceneManager.GetActiveScene().name == "Level4-Tutorial")
        {
            if(!initializedOnce)
            SpawnManager.initializeAndSpawn(
                false,
                false,
                false, 
                false,
                true,           // tutorial spawner
                false,
                false );               
        }
        else if (SceneManager.GetActiveScene().name == "Level6-Tutorial")
        {
            if(!initializedOnce)
            SpawnManager.initializeAndSpawn(
                false,
                false,
                false, 
                false,
                true,           // tutorial spawner
                false,
                false );
        }
        else if(SceneManager.GetActiveScene().name is "CannonLevel1"
                                                    or "CannonLevel4"or "CannonLevel3")
        {
            if(!initializedOnce)
            SpawnManager.initializeAndSpawnCannonLevel();
        }
        
        else if (SceneManager.GetActiveScene().name is "CannonLevel1-Tutorial" or "CannonLevel3-Tutorial" or "CannonLevel4-Tutorial")
        {
            Debug.Log("SPAWN CANNON TUT - GAMEMANGE");
            if(!initializedOnce)
                SpawnManager.initializeAndSpawnCannonTutorialLevel();
        }
        Debug.Log(SceneManager.GetActiveScene().name);
    }

    public void InitializeTimerText() {
        if (SceneManager.GetActiveScene().name is not "CannonLevel3-Tutorial" and not "CannonLevel1-Tutorial" and not "CannonLevel4-Tutorial")
        {
            string minutes = (TimeManager.timeRemaining/ 60).ToString();
            string seconds = (TimeManager.timeRemaining % 60).ToString("f0");
            if(seconds=="0")
                seconds = "00";
            Timertext.text = "0"+ minutes + ":"+ seconds;
        }
    }

    public bool TestLetter(string letter){
        for(int i=0; i<solvedList.Count;i++){
            if(solvedList[i] == letter && orbList[i].GetComponentsInChildren<Image>()[1].fillAmount < 1.0f)
            {
                return true;
            }
        }
        return false;
    }

    /*
     Function created to check if letter existed in word. Being used to avoid showing incorrect orb animation.
     */
    public bool isLetterInWord(string letter)
    {
        for (int i = 0; i < solvedList.Count; i++)
        {
            if (characterList[i] == letter)
                return true;
        }
        return false;
    }

    public bool CheckLetter(string letter, Color playerColor, int score){
        for(int i=0; i<solvedList.Count;i++){

            if(solvedList[i] == letter && orbList[i].GetComponentsInChildren<Image>()[1].fillAmount < 1.0f)
            {
                if (SceneManager.GetActiveScene().name is "Level5" or "Level5-Tutorial" or "Level5-2")
                {
                    if (playerColor != sparkle.GetComponent<ParticleSystem>().startColor)
                    {
                        ScoreManager.IncreaseScore(5);
                        SpawnManager.numOfIncorrecColoredCorrectCharacterBubblesSliced++;
                    }
                    else
                    {
                        ScoreManager.IncreaseScore(10);
                        SpawnManager.numOfCorrectColoredCorrectCharacterBubblesSliced++;
                    }    
                }
                else if(SceneManager.GetActiveScene().name is "CannonLevel1" 
                                                            or "CannonLevel4"or "CannonLevel3"){
                        ScoreManager.IncreaseScore(score);
                }
                else
                {
                    ScoreManager.IncreaseScore(10);
                }
                
                if (TimeManager.timeRemaining == 0)
                {   
                    UserNumberManager.IncreaseNumUser("time");
                    SendAnalytic();

                    SceneManager.LoadScene("LevelFinished");
                }

                //letterHolderList[i].text = letter;
                
                // Make the boundary of letter orb visible
                orbList[i].GetComponentsInChildren<Image>()[0].fillAmount = 1f;
                // Make the underscore invisible
                orbList[i].GetComponentsInChildren<Image>()[2].fillAmount = 0f;

                // Fill the letter orb depending on the level
                if (SceneManager.GetActiveScene().name.StartsWith("Cannon"))
                {
                    orbList[i].GetComponentsInChildren<Image>()[1].fillAmount += 0.5f;
                }
                else
                {
                    orbList[i].GetComponentsInChildren<Image>()[1].fillAmount += 0.2f;
                }

                SpawnManager.numOfCorrectBubblesSliced++;
                
                scoreText.text = "Score: " + ScoreManager.score.ToString();
                
                if (SceneManager.GetActiveScene().name is "Level5" or "Level5-2" && playerColor != getSparkleColor()){
                    FindObjectOfType<IntroText>().showScoreIncrement("+5");
                }
                else if(SceneManager.GetActiveScene().name is not ("CannonLevel1" or "CannonLevel4"or "CannonLevel3" or "CannonLevel1-Tutorial" or "CannonLevel3-Tutorial" or "CannonLevel4-Tutorial")){
                    FindObjectOfType<IntroText>().showScoreIncrement("+10");
                }
                
                
                if (orbList[i].GetComponentsInChildren<Image>()[1].fillAmount == 1.0f)
                {
                    solvedList[i] = "";
                    unsolvedCount--;
                    return true;
                }
                return false;
            }
        }

        for (int i = 0; i < solvedList.Count; i++)
        {
            if (characterList[i] == letter)
                return false;
        }

        // Extra damage for CannonLevel 1
        if(SceneManager.GetActiveScene().name is "CannonLevel1" or "CannonLevel4"or "CannonLevel3")
        {
            incorrectOrb.GetComponentsInChildren<Image>()[1].fillAmount += 0.05f;
        }
        incorrectOrb.GetComponentsInChildren<Image>()[1].fillAmount += 0.05f;
        if (incorrectOrb.GetComponentsInChildren<Image>()[1].fillAmount == 1.0f)
        {  
            //Debug.Log("Filled wrong bar.");
            UserNumberManager.level_of_game_over = SceneManager.GetActiveScene().name;
            UserNumberManager.time_taken_for_death = 60 - TimeManager.timeRemaining;
            //Debug.Log(UserNumberManager.level_of_game_over + " " + UserNumberManager.time_taken_for_death);
            UserNumberManager.IncreaseNumUser("orb");
            SendAnalytic();
            SceneManager.LoadScene("GameOver");
        }
        // heartSystem.TakeDamage();
        return false;
    }

    public bool isFinished(){
        if(unsolvedCount == 0)
        {
            for (int i = 0; i < orbList.Count; i++)
            {
                Destroy(orbList[i].gameObject);
            }
            ScoreManager.IncreaseScore(50);
            scoreText.text = "Score: " + ScoreManager.score.ToString();
            orbList = new List<GameObject>();
            solvedList = new List<string>();
            characterList = new List<string>();
            return true;   
        }
        return false;
    }

    IEnumerator TimerTake()
    {
        if (SceneManager.GetActiveScene().name is not "CannonLevel3-Tutorial" and not "CannonLevel1-Tutorial" and not "CannonLevel4-Tutorial")
        {
            Debug.Log(SceneManager.GetActiveScene().name);
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
                UserNumberManager.IncreaseNumUser("time");
                SendAnalytic();
                SceneManager.LoadScene("LevelFinished");
            }
        
        
            if (SceneManager.GetActiveScene().name == "Level3")
            {   
            
                if (colorChangeSecondsLeft == 0)
                {
                    colorChangeSecondsLeft = maxColorTimerValue;
                    // Color32 newColor = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
                    Color[] colors = FindObjectOfType<ColorSpawner>().GetColors();

                    int randColorIndex = Random.Range(0, colors.Length-1);

                    while(old_index==randColorIndex){
                        randColorIndex = Random.Range(0, colors.Length-1);
                    }
                    old_index=randColorIndex;

                    Color32 newColor = colors[randColorIndex];
                    sparkle.GetComponent<ParticleSystem>().startColor = newColor;
                    // SpawnManager.SetSpawnColorPreferences(randColorIndex);
                    bladeTrail.startColor = newColor;
                    bladeTrail.endColor = newColor;
                    fill.color = newColor;
                }

                else
                    colorChangeSecondsLeft -= 1;
                SetColorTimer(colorChangeSecondsLeft);
			
            }
        
            finished = false;
        }

        
    }
    
    public void SetColorTimer(int health)
    {
        slider.value = health;
    }


    public Color getSparkleColor(){
        return sparkle.GetComponent<ParticleSystem>().startColor;
    }

    public void IncrementAllOrbsByAmt(float amt, string letter)
    {
        for (int i = 0; i < orbList.Count; i++)
        {
            
            if(solvedList[i] == letter[0].ToString()){

                if(orbList[i].GetComponentsInChildren<Image>()[1].fillAmount < 1.0f){
                    orbList[i].GetComponentsInChildren<Image>()[1].fillAmount += amt;
                    orbList[i].GetComponentsInChildren<Image>()[2].fillAmount = 0f;
                    orbList[i].GetComponentsInChildren<Image>()[0].fillAmount = 1f;

                    if (orbList[i].GetComponentsInChildren<Image>()[1].fillAmount == 1.0f)
                    {
                        solvedList[i] = "";
                        unsolvedCount--;
                    }
                    return;
                }
            }
        }
        return;
    }

    public void UpdateScore() {
        scoreText.text = "Score: " + ScoreManager.score.ToString();
    }

    public void SendAnalytic(){
        if(SceneManager.GetActiveScene().name is "Level2" or "Level3-2"){
            FindObjectOfType<Analytics>().SendSpecialBubbleAnalyticWrapper();
        }
        if(SceneManager.GetActiveScene().name is "Level3_updated" or "Level4-2" or "Level3-2"){
            //Debug.Log("Inside send analytics of obstacle");
            FindObjectOfType<Analytics>().SendObstacleSliceAnalyticWrapper();
        }
        if(SceneManager.GetActiveScene().name == "Level4")
        {
            FindObjectOfType<Analytics>().SendFreezeCardAnalyticWrapper();
        }
        if(SceneManager.GetActiveScene().name == "Level5" || SceneManager.GetActiveScene().name == "Level5-2"){
            FindObjectOfType<Analytics>().SendColoredBubbleSliceAnalyticWrapper();
        }
        FindObjectOfType<Analytics>().SendTimeTakenInDeathAnalyticWrapper();
         //FindObjectOfType<Analytics>().SendUserAnalyticWrapper();
        FindObjectOfType<Analytics>().SendUserAnalyticWrapper();
    }

    public void FillIncorrectOrb()
    {
        incorrectOrb.GetComponentsInChildren<Image>()[1].fillAmount += 0.10f;
        if (incorrectOrb.GetComponentsInChildren<Image>()[1].fillAmount >= 1.0f)
        {  
            UserNumberManager.level_of_game_over = SceneManager.GetActiveScene().name;
            UserNumberManager.time_taken_for_death = 60 - TimeManager.timeRemaining;
            UserNumberManager.IncreaseNumUser("orb");
            SendAnalytic();
            SceneManager.LoadScene("GameOver");
        }
    }

    public bool ReduceCorrectOrb(String letter)
    {
        for (int i = 0; i < solvedList.Count; i++)
        {

            if (characterList[i] == letter)
            {
                FindObjectOfType<BasicSpawner>().AddLetterToPreferredList(char.Parse(letter));
                if (orbList[i].GetComponentsInChildren<Image>()[1].fillAmount > 0.0f)
                {
                    if (orbList[i].GetComponentsInChildren<Image>()[1].fillAmount == 1.0f)
                    {
                        solvedList[i] = letter;
                        unsolvedCount++;
                    }
                    if (orbList[i].GetComponentsInChildren<Image>()[1].fillAmount <= 0.2f)
                    {
                        orbList[i].GetComponentsInChildren<Image>()[1].fillAmount = 0f;
                    }
                    else
                    {
                        orbList[i].GetComponentsInChildren<Image>()[1].fillAmount -= 0.2f;
                    }
                    orbList[i].GetComponentsInChildren<Image>()[0].fillAmount = 1f;
                    orbList[i].GetComponentsInChildren<Image>()[2].fillAmount = 0f;
                    if (ScoreManager.score > 0)
                    {
                        ScoreManager.DecreaseScore();
                        scoreText.text = "Score: " + ScoreManager.score.ToString();
                        return true;
                    }
                }
                break;
            }
        }
        return false;
    }
}
