using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Analytics: MonoBehaviour
{
    [SerializeField] private string LEVEL2_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfQTiBgNACmXcsu_P16t7NTxib5PifOFwHrJHPelkGxANo5gw/formResponse";
    [SerializeField] private string LEVEL3_URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLScgtQhmO015haF3gH-2xS7AaXChzcz8qvNjrwqBDsAGVhMVCg/formResponse";
    [SerializeField] private string LEVEL4_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSeFIzGyhF6ecIpb-_W3ZjjiIhoZvLkJ7_Bn2An-q9rMGkAY-A/formResponse";
    [SerializeField] private string LEVEL5_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSf5Wvu30lXaUY3kx7HQXOFg5WVYVtEZQSOzjEMDM5VVEUrMcg/formResponse";
    [SerializeField] private string GENERAL_TIME_ANALYTIC_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSd5c06Qgfv52dFqAh6w0ljioxjtyyfVPZjxTHgxURnS5y1K1g/formResponse";

    [SerializeField] private string ANALYTICS1_URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSf-dwAXxX96RQvaktjJE23eqaTnrkQmkW6-qdl6Uv7AwantaQ/formResponse";
    public void SendSpecialBubbleAnalyticWrapper()
    {
        StartCoroutine(SendSpecialBubbleAnalytic());
    }
    
    public void SendFreezeCardAnalyticWrapper()
    {
        StartCoroutine(SendFreezeCardAnalytic());
    }

    public void SendColoredBubbleSliceAnalyticWrapper()
    {
        StartCoroutine(SendColoredBubbleSliceAnalytic());
    }
    public void SendUserAnalyticWrapper()
    {
        StartCoroutine(SendUserAnalytic());
    }

    public void SendTimeTakenInDeathAnalyticWrapper()
    {
        StartCoroutine(SendTimeTakenInDeathAnalytic());
    }

    public void SendObstacleSliceAnalyticWrapper()
    {
        StartCoroutine(SendObstacleSliceAnalytic());
    }

    private IEnumerator SendColoredBubbleSliceAnalytic()
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.770810724", SpawnManager.numOfCorrectColoredCorrectCharacterBubblesSliced.ToString());
        form.AddField("entry.1256396698", SpawnManager.numOfIncorrecColoredCorrectCharacterBubblesSliced.ToString());
        using (UnityWebRequest www = UnityWebRequest.Post(LEVEL5_URL, form))
        {
            yield return www.SendWebRequest();
            Debug.Log(www.result != UnityWebRequest.Result.Success ? www.error : "Form upload complete...");
        }
    }

    private IEnumerator SendObstacleSliceAnalytic()
    {
        float analyticValue = (float) SpawnManager.numOfSlicesOnObstacle / (float) SpawnManager.totalNumOfSlices;
        WWWForm form = new WWWForm();
        if (analyticValue < 0.2f)
            form.AddField("entry.566017682", "0.0-0.2");
        else if (analyticValue >= 0.2f && analyticValue < 0.4f)
            form.AddField("entry.566017682", "0.2-0.4");
        else if (analyticValue >= 0.4f && analyticValue < 0.6f)
            form.AddField("entry.566017682", "0.4-0.6");
        else if (analyticValue >= 0.6f && analyticValue < 0.8f)
            form.AddField("entry.566017682", "0.6-0.8");
        else if (analyticValue >= 0.8f)
            form.AddField("entry.566017682", "0.8-1.0");

        using (UnityWebRequest www = UnityWebRequest.Post(LEVEL3_URL, form))
        {
            yield return www.SendWebRequest();
            Debug.Log("Sending SendObstacleSliceAnalytic data. Value " + (SpawnManager.numOfSlicesOnObstacle / SpawnManager.totalNumOfSlices).ToString());
            Debug.Log(www.result != UnityWebRequest.Result.Success ? www.error : "Form upload complete...");
        }
    }
    
    private IEnumerator SendFreezeCardAnalytic()
    {
        WWWForm form = new WWWForm();
        if (BasicSpawner.scoreDiffBeforeAndAfterFreeze >= 0 && BasicSpawner.scoreDiffBeforeAndAfterFreeze < 21)
        {
            form.AddField("entry.1651039141", "0-20");
        } else if (BasicSpawner.scoreDiffBeforeAndAfterFreeze > 20 && BasicSpawner.scoreDiffBeforeAndAfterFreeze < 41)
        {
            form.AddField("entry.1651039141", "21-40");
        } else if (BasicSpawner.scoreDiffBeforeAndAfterFreeze > 40 && BasicSpawner.scoreDiffBeforeAndAfterFreeze < 61)
        {
            form.AddField("entry.1651039141", "41-60");
        } else if (BasicSpawner.scoreDiffBeforeAndAfterFreeze > 60 && BasicSpawner.scoreDiffBeforeAndAfterFreeze < 81)
        {
            form.AddField("entry.1651039141", "61-80");
        } else if (BasicSpawner.scoreDiffBeforeAndAfterFreeze > 80 && BasicSpawner.scoreDiffBeforeAndAfterFreeze < 101)
        {
            form.AddField("entry.1651039141", "81-100");
        } else if (BasicSpawner.scoreDiffBeforeAndAfterFreeze > 100 && BasicSpawner.scoreDiffBeforeAndAfterFreeze < 121)
        {
            form.AddField("entry.1651039141", "101-120");
        } else if (BasicSpawner.scoreDiffBeforeAndAfterFreeze > 120 && BasicSpawner.scoreDiffBeforeAndAfterFreeze < 141)
        {
            form.AddField("entry.1651039141", "121-140");
        }
        else
        {
            form.AddField("entry.1651039141", ">140");
        }
        
        using (UnityWebRequest www = UnityWebRequest.Post(LEVEL4_URL, form))
        {
            yield return www.SendWebRequest();
            Debug.Log(www.result != UnityWebRequest.Result.Success ? www.error : "Form upload complete...");
        }
    }

    private IEnumerator SendSpecialBubbleAnalytic()
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.243084854", SpawnManager.numOfSpecialBubblesSliced.ToString());
        form.AddField("entry.1200357974", SpawnManager.numOfSpecialBubblesSpawned.ToString());
        using (UnityWebRequest www = UnityWebRequest.Post(LEVEL2_URL, form))
        {
            yield return www.SendWebRequest();
            Debug.Log(www.result != UnityWebRequest.Result.Success ? www.error : "Form upload complete...");
        }
    }

    private IEnumerator SendTimeTakenInDeathAnalytic()
    {
        //Debug.Log("called time death analytic : " + UserNumberManager.level_of_game_over + " " + UserNumberManager.time_taken_for_death.ToString());
        WWWForm form = new WWWForm();
        form.AddField("entry.334369683", UserNumberManager.level_of_game_over);
        form.AddField("entry.1102616409", UserNumberManager.time_taken_for_death.ToString());
        using (UnityWebRequest www = UnityWebRequest.Post(GENERAL_TIME_ANALYTIC_URL, form))
        {
            yield return www.SendWebRequest();
            Debug.Log(www.result != UnityWebRequest.Result.Success ? www.error : "Time for death recorded...");
        }
    }
    private IEnumerator SendUserAnalytic()
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.344710661", UserNumberManager.user_no_orb_level1_1);
        form.AddField("entry.520998493", UserNumberManager.user_no_time_level1_1);

        form.AddField("entry.1954795411", UserNumberManager.user_no_orb_level2_1);
        form.AddField("entry.467348718", UserNumberManager.user_no_time_level2_1);

        form.AddField("entry.1560882412", UserNumberManager.user_no_orb_level3_1);
        form.AddField("entry.980857594", UserNumberManager.user_no_time_level3_1);

        form.AddField("entry.478736432", UserNumberManager.user_no_orb_level3_2);
        form.AddField("entry.299727108", UserNumberManager.user_no_time_level3_2);

        form.AddField("entry.1858953075", UserNumberManager.user_no_orb_level4_1);
        form.AddField("entry.1892654440", UserNumberManager.user_no_time_level4_1);

        form.AddField("entry.1309206572", UserNumberManager.user_no_orb_level4_2);
        form.AddField("entry.91432891", UserNumberManager.user_no_time_level4_2);

        form.AddField("entry.1474806251", UserNumberManager.user_no_orb_level5_1);
        form.AddField("entry.1659802926", UserNumberManager.user_no_time_level5_1);

        form.AddField("entry.1964189175", UserNumberManager.user_no_orb_level5_2);
        form.AddField("entry.1017052371", UserNumberManager.user_no_time_level5_2);


        using (UnityWebRequest www = UnityWebRequest.Post(ANALYTICS1_URL, form))
        {
            yield return www.SendWebRequest();
            Debug.Log(www.result != UnityWebRequest.Result.Success ? www.error : "Form upload complete...");
        }
    }
}
