using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void ResetTheGame()
    {
        ScoreManager.Clear();
        UserNumberManager.Clear();
        TimeManager.Clear();
        SpawnManager.Clear();
        LivesManager.Clear();
        SceneManager.LoadScene("HomeScene");
    }
}