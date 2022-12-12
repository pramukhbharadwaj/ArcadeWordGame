using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelChange : MonoBehaviour
{
    
    public void openLevel1_0()
    {
        LevelAndGenreManager.levelName = "Level1-Tutorial";
        SceneManager.LoadScene("Level1-Tutorial");
    }
    
    public void openLevel2_0()
    {
        LevelAndGenreManager.levelName = "Level2-Tutorial";
        SceneManager.LoadScene("Level2-Tutorial");
    }
    
    public void openLevel3_0()
    {
        LevelAndGenreManager.levelName = "Level3-Tutorial";
        SceneManager.LoadScene("Level3-Tutorial");
    }
    public void openLevel4_0()
    {
        LevelAndGenreManager.levelName = "Level4-Tutorial";
        SceneManager.LoadScene("Level4-Tutorial");
    }
    public void openLevel5_0()
    {
        LevelAndGenreManager.levelName = "Level5-Tutorial";
        SceneManager.LoadScene("Level5-Tutorial");
    }
    
    // public void openLevel6_0()
    // {
    //     LevelAndGenreManager.levelName = "Level6";
    //     SceneManager.LoadScene("Genre");
    // }
    public void openLevel1_1()
    {
        LevelAndGenreManager.levelName = "SliceItOff";
        SceneManager.LoadScene("Genre");
    }
    
    public void openLevel2_1()
    {
        LevelAndGenreManager.levelName = "Level2";
        SceneManager.LoadScene("Genre");
    }
    
    public void openLevel3_1()
    {
        LevelAndGenreManager.levelName = "Level3_updated";
        SceneManager.LoadScene("Genre");
    }
    public void openLevel3_2()
    {
        LevelAndGenreManager.levelName = "Level3-2";
        SceneManager.LoadScene("Genre");
    }
    public void openLevel4_1()
    {
        LevelAndGenreManager.levelName = "Level4";
        SceneManager.LoadScene("Genre");
    }
    public void openLevel4_2()
    {
        LevelAndGenreManager.levelName = "Level4-2";
        SceneManager.LoadScene("Genre");
    }
    public void openLevel5_1()
    {
        LevelAndGenreManager.levelName = "Level5";
        SceneManager.LoadScene("Genre");
    }
    public void openLevel5_2()
    {
        LevelAndGenreManager.levelName = "Level5-2";
        SceneManager.LoadScene("Genre");
    }
    public void openLevel6_0()
    {
        LevelAndGenreManager.levelName = "Level6-Tutorial";
        SceneManager.LoadScene("Level6-Tutorial");
    }
    public void openLevel6_1()
    {
        LevelAndGenreManager.levelName = "Level6";
        SceneManager.LoadScene("Genre");
    }
    
    public void openCanonLevel1_0()
    {
        LevelAndGenreManager.levelName = "CannonLevel1-Tutorial";
        SceneManager.LoadScene("CannonLevel1-Tutorial");
    }

    public void openCanonLevel1_1()
    {
        LevelAndGenreManager.levelName = "CannonLevel1";
        SceneManager.LoadScene("Genre");
    }
    
    public void openCanonLevel2_0()
    {
        
    }
    
    public void openCanonLevel2_1()
    {
        
    }
    
    public void openCanonLevel3_0()
    {
        LevelAndGenreManager.levelName = "CannonLevel3-Tutorial";
        SceneManager.LoadScene("CannonLevel3-Tutorial");
    }
    
    public void openCanonLevel3_1()
    {
        LevelAndGenreManager.levelName = "CannonLevel3";
        SceneManager.LoadScene("Genre");
    }
    
    public void openCanonLevel4_0()
    {
        LevelAndGenreManager.levelName = "CannonLevel4-Tutorial";
        SceneManager.LoadScene("CannonLevel4-Tutorial");
    }
    
    public void openCanonLevel4_1()
    {
        LevelAndGenreManager.levelName = "CannonLevel4";
        SceneManager.LoadScene("Genre");
    }

    public void openLeaderboard()
    {
        LevelAndGenreManager.levelName = "Leaderboard";
        SceneManager.LoadScene("Leaderboard");
    }
}