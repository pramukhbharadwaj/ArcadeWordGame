using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    public TMP_Text[] rank;
    public TMP_Text[] name;
    public TMP_Text[] score;

  public GameObject finishedUI;
    public GameObject leaderboardUI;

    string levelName = LevelAndGenreManager.levelName;

    public void SubmitScoreButton()
    {
    //  SubmitScore();
    }

    // private void SubmitScore()
    // {
    //     User currentUser = new User(UserManager.name, ScoreManager.score);
    //     DatabaseHandler.PostUser(currentUser, UserManager.name, ()=>{
    //         Debug.Log("Score Submitted");
    //         // LoadLeaderboard();
    //     });
    // }

    static int Compare(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
    {
        return b.Value.CompareTo(a.Value);
    }

    // public void LoadLeaderboard(){
    //     finishedUI.SetActive(false);
    //     leaderboardUI.SetActive(true);

    //     var userList = new List<KeyValuePair<string, int>>();
    //     DatabaseHandler.GetUsers(users =>
    //     {
    //         foreach (var user in users)
    //         {
    //             userList.Add(new KeyValuePair<string, int>(user.Value.username,user.Value.score));
    //             Debug.Log($"{user.Value.username} {user.Value.score}");
    //         }

    //         userList.Sort(Compare);

    //         int i=0;
    //         int userRankAttained = -1;

    //         foreach(var u in userList){
    //             string username = u.Key;
    //             string highScore = u.Value.ToString();

    //             if(i is 0 or 1 or 2){
    //                 rank[i].text = (i+1).ToString();
    //                 name[i].text = username;
    //                 score[i].text = highScore;
    //             }

    //             if(UserManager.name == username){
    //                 userRankAttained = i;
    //                 if(i>2){
    //                     break;
    //                 }else{
    //                     rank[i].color = new Color32(196,5,5,255);
    //                     name[i].color = new Color32(196,5,5,255);
    //                     score[i].color = new Color32(196,5,5,255);
    //                 }
    //             }
    //             i +=1;
    //             Debug.Log(username+":"+highScore);
    //         }

    //         if(userRankAttained>2){
    //             int j=3;
    //             i=0;
    //             foreach (var u in userList)
    //             {
    //                 string username = u.Key;
    //                 string highScore = u.Value.ToString();

    //                 if(i == userRankAttained-1 || i== userRankAttained || i== userRankAttained+1 && userRankAttained-1 != 2){
    //                     rank[j].text = (i+1).ToString();
    //                     name[j].text = username;
    //                     score[j].text = highScore;
    //                     if(i == userRankAttained){
    //                         rank[j].color = new Color32(196,5,5,255);
    //                         name[j].color = new Color32(196,5,5,255);
    //                         score[j].color = new Color32(196,5,5,255);
    //                     }

    //                     j += 1;
    //                 }
    //                 i +=1;
    //             }
    //         }
    //     });
    // }
  
}
