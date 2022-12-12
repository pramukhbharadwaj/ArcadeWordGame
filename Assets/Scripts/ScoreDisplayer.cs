using UnityEngine;
using TMPro;

public class ScoreDisplayer : MonoBehaviour{
    public TMP_Text scoreText;

    void Awake(){
      scoreText.text = "Score: " + ScoreManager.score.ToString();
    }
}