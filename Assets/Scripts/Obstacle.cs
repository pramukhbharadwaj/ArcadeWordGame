using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    public GameObject incorrectSlicePrefab;
    public bool obstacleDestroyed = false;
    public GameObject floatingTextPrefab;
    public TMP_Text scoreText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnManager.numOfSlicesOnObstacle++;
            SpawnManager.totalNumOfSlices++;
            if (gameObject.transform.parent.localScale.x > 0.2)
            {
                gameObject.transform.parent.localScale -= new Vector3(0.075f, 0.075f, 0f);
            }
            else
            {
                obstacleDestroyed = true;
                if (SceneManager.GetActiveScene().name == "Level3-Tutorial")
                {
                    FindObjectOfType<TutorialSpawner>().HideAnimLevel3();
                }
                ScoreManager.IncreaseScore(30);
                showScoreIncrement("+30");
                FindObjectOfType<GameManager>().UpdateScore();
                Destroy(gameObject);
            }
        }

        if (other.gameObject.tag == "Text")
        {
            string slicedLetter = other.gameObject.name[0].ToString();
            if (!FindObjectOfType<GameManager>().TestLetter(slicedLetter))
            {
                showIncorrectSliceDamage(slicedLetter);
            }
        }
    }
    
    public bool isObstacleDestroyed(){
        return obstacleDestroyed;
    }
    public void showIncorrectSliceDamage(string text){
        GameObject prefab = Instantiate(incorrectSlicePrefab, transform.position, Quaternion.identity);
        prefab.GetComponentInChildren<TMP_Text>().text = text;
    }

    public void showScoreIncrement(string text){
        GameObject prefab = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        prefab.GetComponentInChildren<TMP_Text>().text = text;
    }
}