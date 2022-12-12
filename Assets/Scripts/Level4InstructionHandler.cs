using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Level4InstructionHandler : MonoBehaviour
{
    public TMP_Text instructionText1, instructionText2;
    public Button buttonToHide1, buttonToHide2;
    public GameObject instructionObject1, instructionObject2;

    void Awake()
    {
        instructionText1.text = "Slice green bubbles to fill bucket!               --------->\nSlice red bubbles to remove from bucket!";
        StartCoroutine("DisplayInstructions1");
    }
    
    public void DisplayInstructions1()
    {
        instructionObject1.SetActive(true);
        buttonToHide1.gameObject.SetActive(true);
        instructionObject2.SetActive(false);
        buttonToHide2.gameObject.SetActive(false);
        Time.timeScale = 0;
    }
    
    public void HideInstructions1()
    {
        //Time.timeScale = 1;
        instructionObject1.SetActive(false);
        buttonToHide1.gameObject.SetActive(false);
        instructionText2.text = "Complete the words in the panel        -------->\nusing the letters in the bucket!";
        instructionObject2.SetActive(true);
        buttonToHide2.gameObject.SetActive(true);
        //Time.timeScale = 0;
    }
    
    public void HideInstructions2()
    {
        Time.timeScale = 1;
        instructionObject2.SetActive(false);
        buttonToHide2.gameObject.SetActive(false);
    }
}
