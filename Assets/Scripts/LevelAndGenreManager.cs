using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelAndGenreManager
{
    public static string levelName { get; set; } = null;
    public static string wordType { get; set; } = null;

    public static List<string> fruits { get; private set; } = new List<string>();
    public static List<string> space { get; private set; } = new List<string>();
    public static List<string> finance { get; private set; } = new List<string>();
    public static List<string> countries { get; private set; } = new List<string>();
    public static List<string> carBrands { get; private set; } = new List<string>();
    public static List<string> veggies { get; private set; } = new List<string>();

    static LevelAndGenreManager()
    {
        
        
        fruits.Add("BANANA");
        fruits.Add("GRAPES");
        fruits.Add("APPLE");
        fruits.Add("KIWI");
        fruits.Add("PLUM");
        fruits.Add("PINEAPPLE");
        
        space.Add("COMET");
        space.Add("PLANET");
        space.Add("GALAXY");
        space.Add("ASTEROID");
        space.Add("METEOR");
        space.Add("STAR");
        
        finance.Add("DEBIT");
        finance.Add("CREDIT");
        finance.Add("INTEREST");
        finance.Add("STOCK");
        finance.Add("ASSET");
        finance.Add("LOAN");

        countries.Add("INDIA");
        countries.Add("CHINA");
        countries.Add("ITALY");
        countries.Add("CANADA");
        countries.Add("EGYPT");
        countries.Add("FRANCE");
        
        carBrands.Add("TESLA");
        carBrands.Add("BMW");
        carBrands.Add("TOYOTA");
        carBrands.Add("FORD");
        carBrands.Add("PORSCHE");
        carBrands.Add("HONDA");

        veggies.Add("CABBAGE");
        veggies.Add("POTATO");
        veggies.Add("ONION");
        veggies.Add("PEAS");
        veggies.Add("OLIVE");
        veggies.Add("JALAPENO");
    }

    public static List<string> GetWordList()
    {
        switch (wordType)
        {
            case "Fruits":
                return new List<string>(fruits);
            case "Space":
                return new List<string>(space);
            case "Finance":
                return new List<string>(finance);
            case "Countries":
                return new List<string>(countries);
            case "Car Brands":
                return new List<string>(carBrands);
            case "Veggies":
                return new List<string>(veggies);
            default:
                Debug.Log("Invalid word type");
                return null;
        }
    }


}
