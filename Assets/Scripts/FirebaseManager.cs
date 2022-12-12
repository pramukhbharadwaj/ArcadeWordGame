using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FirebaseManager : MonoBehaviour
{
    public TMP_Text[] rank;
    public TMP_Text[] name;
    public TMP_Text[] score;

    public TMP_Dropdown levelDropdown;
    public TMP_Dropdown modeDropdown;


    string levelName = LevelAndGenreManager.levelName;

    public void Start(){
        Debug.Log(SceneManager.GetActiveScene().name);
        if(SceneManager.GetActiveScene().name is "LevelFinished" or "GameOver"){
            SubmitScore();
        }
    }

    private void SubmitScore()
    {
        User currentUser = new User(UserManager.name, ScoreManager.score);
        DatabaseHandler.PostUser(currentUser, levelName, UserManager.name, ()=>{
            Debug.Log("Score Submitted");
            LoadLeaderboard();
        });
    }

    public void HandleGameModeDropDown(int val){
        Debug.Log(modeDropdown.value);
        levelDropdown.ClearOptions();
        var option = new TMP_Dropdown.OptionData("Level");
        levelDropdown.options.Add(option);
        option = new TMP_Dropdown.OptionData("Level 1-1");
        levelDropdown.options.Add(option);
        option = new TMP_Dropdown.OptionData("Level 2-1");
        levelDropdown.options.Add(option);
        option = new TMP_Dropdown.OptionData("Level 3-1");
        levelDropdown.options.Add(option);
        if(modeDropdown.value == 1){
            option = new TMP_Dropdown.OptionData("Level 3-2");
            levelDropdown.options.Add(option);
            option = new TMP_Dropdown.OptionData("Level 4-1");
            levelDropdown.options.Add(option);
            option = new TMP_Dropdown.OptionData("Level 4-2");
            levelDropdown.options.Add(option);
            option = new TMP_Dropdown.OptionData("Level 5-1");
            levelDropdown.options.Add(option);
            option = new TMP_Dropdown.OptionData("Level 5-2");
            levelDropdown.options.Add(option);
            option = new TMP_Dropdown.OptionData("Level 6-1");
            levelDropdown.options.Add(option);
        }
        levelDropdown.RefreshShownValue();
    }

    public void HandleLevelDropDown(int val){
        Debug.Log(levelDropdown.value);
        if(modeDropdown.value == 1){
            if(levelDropdown.value == 1){
                levelName = "SliceItOff";
            }else if(levelDropdown.value == 2){
                levelName = "Level2";
            }else if(levelDropdown.value == 3){
                levelName = "Level3_updated";
            }else if(levelDropdown.value == 4){
                levelName = "Level3-2";
            }else if(levelDropdown.value == 5){
                levelName = "Level4";
            }else if(levelDropdown.value == 6){
                levelName = "Level4-2";
            }else if(levelDropdown.value == 7){
                levelName = "Level5";
            }else if(levelDropdown.value == 8){
                levelName = "Level5-2";
            }else if(levelDropdown.value == 9){
                levelName = "Level6";
            }
        }else if(modeDropdown.value == 2){
            if(levelDropdown.value == 1){
                levelName = "CannonLevel1";
            }else if(levelDropdown.value == 2){
                levelName = "Level2";
            }else if(levelDropdown.value == 3){
                levelName = "CannonLevel3";
            }
        }
        LoadLeaderboard();
    }

    static int Compare(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
    {
        return b.Value.CompareTo(a.Value);
    }

    public void LoadLeaderboard(){
        ResetLeaderBoard();
        var userList = new List<KeyValuePair<string, int>>();
        DatabaseHandler.GetUsers(levelName, users =>
        {
            foreach (var user in users)
            {
                userList.Add(new KeyValuePair<string, int>(user.Value.username,user.Value.score));
                Debug.Log($"{user.Value.username} {user.Value.score}");
            }

            userList.Sort(Compare);

            int i=0;
            int userRankAttained = -1;

            foreach(var u in userList){
                string username = u.Key;
                string highScore = u.Value.ToString();

                if(i<=5){
                    rank[i].text = (i+1).ToString();
                    name[i].text = username;
                    score[i].text = highScore;
                }

                if(UserManager.name == username){
                    userRankAttained = i;
                    if(i>5){
                        break;
                    }else{
                        rank[i].color = new Color32(196,5,5,255);
                        name[i].color = new Color32(196,5,5,255);
                        score[i].color = new Color32(196,5,5,255);
                    }
                }
                i +=1;
                Debug.Log(username+":"+highScore);
            }

            if(userRankAttained>5){
                int j=5;
                i=0;
                foreach (var u in userList)
                {
                    string username = u.Key;
                    string highScore = u.Value.ToString();

                    if(i== userRankAttained){
                        rank[j].text = (i+1).ToString();
                        name[j].text = username;
                        score[j].text = highScore;
                        rank[j].color = new Color32(196,5,5,255);
                        name[j].color = new Color32(196,5,5,255);
                        score[j].color = new Color32(196,5,5,255);

                    }
                    i +=1;
                }
            }
        });
    }

    public void ResetLeaderBoard(){
        for(int i=0;i<6;i++){
            rank[i].text = "";
            name[i].text = "";
            score[i].text = "";
            rank[i].color = new Color32(225,255,255,255);
            name[i].color = new Color32(255,255,255,255);
            score[i].color = new Color32(255,255,255,255);
        }
    }
}
