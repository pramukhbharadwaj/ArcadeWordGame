using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


using UnityEngine.SceneManagement;
public static class UserNumberManager {


    // level in which the player died - icorrect orb filled up.
    public static string level_of_game_over { get;  set; } = "";

    // death : incorrect orb fills up.
    public static int time_taken_for_death { get;  set; } = 0;


    public static int user_no_orb_level1_1 { get; private set; } = 0;
    public static int user_no_time_level1_1 { get; private set; } = 0;

    public static int user_no_orb_level2_1 { get; private set; } = 0;
    public static int user_no_time_level2_1 { get; private set; } = 0;

    public static int user_no_orb_level3_1 { get; private set; } = 0;
    public static int user_no_time_level3_1 { get; private set; } = 0;

    public static int user_no_orb_level3_2 { get; private set; } = 0;
    public static int user_no_time_level3_2 { get; private set; } = 0;

    public static int user_no_orb_level4_1 { get; private set; } = 0;
    public static int user_no_time_level4_1 { get; private set; } = 0;

    public static int user_no_orb_level4_2 { get; private set; } = 0;
    public static int user_no_time_level4_2 { get; private set; } = 0;

    public static int user_no_orb_level5_1 { get; private set; } = 0;
    public static int user_no_time_level5_1 { get; private set; } = 0;

    public static int user_no_orb_level5_2 { get; private set; } = 0;
    public static int user_no_time_level5_2 { get; private set; } = 0;

    public static void IncreaseNumUser(string x){
        if (SceneManager.GetActiveScene().name == "SliceItOff" ){
            if(x=="orb"){
                user_no_orb_level1_1 =1;
            }else{
                user_no_time_level1_1 =1;
            }
            
        }else if(SceneManager.GetActiveScene().name == "Level2" ){
            if(x=="orb"){
                user_no_orb_level2_1 =1;
            }else{
                user_no_time_level2_1 =1;
            }
        
        }else if(SceneManager.GetActiveScene().name == "Level3_updated" ){
            if(x=="orb"){
                user_no_orb_level3_1 =1;
            }else{
                user_no_time_level3_1 =1;
            }
        
        }else if(SceneManager.GetActiveScene().name == "Level3-2" ){
            if(x=="orb"){
                user_no_orb_level3_2 =1;
            }else{
                user_no_time_level3_2 =1;
            }
        
        }else if(SceneManager.GetActiveScene().name == "Level4" ){
            if(x=="orb"){
                user_no_orb_level4_1 =1;
            }else{
                user_no_time_level4_1 =1;
            }
        
        }else if(SceneManager.GetActiveScene().name == "Level4-2" ){
            if(x=="orb"){
                user_no_orb_level4_2 =1;
            }else{
                user_no_time_level4_2 =1;
            }
        
        }else if(SceneManager.GetActiveScene().name == "Level5" ){
            if(x=="orb"){
                user_no_orb_level5_1 =1;
            }else{
                user_no_time_level5_1 =1;
            }
        
        }else if(SceneManager.GetActiveScene().name == "Level5-2" ){
            if(x=="orb"){
                user_no_orb_level5_2 =1;
            }else{
                user_no_time_level5_2 =1;
            }
        
        }

    }



    public static void Clear() {
        user_no_orb_level1_1=0;
        user_no_time_level1_1=0;

        user_no_orb_level2_1=0;
        user_no_time_level2_1=0;

        user_no_orb_level3_1=0;
        user_no_time_level3_1=0;

        user_no_orb_level3_2=0;
        user_no_time_level3_2=0;

        user_no_orb_level4_1=0;
        user_no_time_level4_1=0;

        user_no_orb_level4_2=0;
        user_no_time_level4_2=0;

        user_no_orb_level5_1=0;
        user_no_time_level5_1=0;

        user_no_orb_level5_2=0;
        user_no_time_level5_2=0;


    }
}