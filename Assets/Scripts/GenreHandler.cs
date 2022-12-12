using UnityEngine;
using UnityEngine.SceneManagement;

public class GenreHandler : MonoBehaviour
{
    public void IntializeFruitsAndStart()
    {
        LevelAndGenreManager.wordType = "Fruits";
        SceneManager.LoadScene(LevelAndGenreManager.levelName);
    }
    
    public void IntializeSpaceAndStart()
    {
        LevelAndGenreManager.wordType = "Space";
        SceneManager.LoadScene(LevelAndGenreManager.levelName);
    }
    
    public void IntializeFinanceAndStart()
    {
        LevelAndGenreManager.wordType = "Finance";
        SceneManager.LoadScene(LevelAndGenreManager.levelName);
    }
    
    public void IntializeCountriesAndStart()
    {
        LevelAndGenreManager.wordType = "Countries";
        SceneManager.LoadScene(LevelAndGenreManager.levelName);
    }
    
    public void IntializeCarBrandsAndStart()
    {
        LevelAndGenreManager.wordType = "Car Brands";
        SceneManager.LoadScene(LevelAndGenreManager.levelName);
    }
    
    public void IntializeVeggiesAndStart()
    {
        LevelAndGenreManager.wordType = "Veggies";
        SceneManager.LoadScene(LevelAndGenreManager.levelName);
    }
}
