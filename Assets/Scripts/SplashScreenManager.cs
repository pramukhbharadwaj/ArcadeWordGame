using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenManager : MonoBehaviour
{
    public TMP_InputField userName;
    public GameObject requiredText;    

    public void setUserName(){
        if(userName.text != ""){
            UserManager.name = userName.text;
            SceneManager.LoadScene("HomeScene");
        }else{
            requiredText.SetActive(true);
        }
    }
}
