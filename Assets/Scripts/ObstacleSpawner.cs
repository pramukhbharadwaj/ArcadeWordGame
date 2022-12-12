using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    private Collider spawnArea;
    private List<GameObject> obstacles = new List<GameObject>();
    public GameObject obstaclePrefab;

    public void CoroutineSpawn(float initialDelayObstacleSpawner, float minSpawnDelayObstacleSpawner, float maxSpawnDelayObstacleSpawner)
    {
        spawnArea = GetComponent<Collider>();
        StartCoroutine(UpdateObstacle());
        StartCoroutine(Spawn(initialDelayObstacleSpawner, minSpawnDelayObstacleSpawner, maxSpawnDelayObstacleSpawner));
        
    }

    public void TutorialSpawn()
    {
        spawnArea = GetComponent<Collider>(); 
        StartCoroutine(UpdateObstacle());
        CreateObstacle(true); 
    }

    public IEnumerator Spawn(float initialDelayObstacleSpawner, float minSpawnDelayObstacleSpawner, float maxSpawnDelayObstacleSpawner)
    {
        yield return new WaitForSeconds(initialDelayObstacleSpawner); 
        while (enabled)
        {
            int num = Random.Range(1, 3);
            for (int i = 0; i < num; i++)
            {
                if(SceneManager.GetActiveScene().name=="Level4-2" && (FindObjectOfType<BasicSpawner>().getBasicFrozen() == false && FindObjectOfType<BasicSpawner>().getBasicFrozen() == false))
                {
                    CreateObstacle();
                }
                else
                {
                    CreateObstacle();
                }
            }
            yield return new WaitForSeconds(Random.Range(minSpawnDelayObstacleSpawner, maxSpawnDelayObstacleSpawner));
        }
    }

    private IEnumerator UpdateObstacle()
    {
        yield return new WaitForSeconds(0.01f);
        while (enabled)
        {
            List<GameObject> updatedObstacles = new List<GameObject>();
            foreach (GameObject obstacle in obstacles)
            {
                if (!obstacle.IsUnityNull())
                {
                    if (obstacle.transform.childCount > 0)
                    {
                        updatedObstacles.Add(obstacle);
                        float scaleValue = obstacle.transform.localScale.x;
                        if (SceneManager.GetActiveScene().name == "Level4-2" &&
                            (FindObjectOfType<BasicSpawner>().getBasicFrozen() == true ||
                             FindObjectOfType<BasicSpawner>().getBasicFrozen() == true))
                        {
                        }
                        else
                        {
                            scaleValue += 0.001f;
                            if (scaleValue <= 1)
                            {
                                Vector3 scale = new Vector3(scaleValue, scaleValue, 0f);
                                obstacle.transform.localScale = scale;
                            }    
                            obstacle.transform.Rotate(0, 0, 2, Space.Self);
                        }

                    }
                    else
                    {
                        Destroy(obstacle);
                    }
                }
            }

            obstacles = updatedObstacles;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void CreateObstacle(bool isTutorial=false)
    {
        if (obstacles.Count < 2)
        {
            float x = Random.Range(spawnArea.bounds.min.x + 3, spawnArea.bounds.max.x - 3);
            float y = Random.Range(spawnArea.bounds.min.y + 3, spawnArea.bounds.max.y - 3);
            if (isTutorial)
            {
                x = 0;
                y = 0;
            }
            if (obstacles.Count == 1)
            {
                if (IsObstacleOverlapping(x, y))
                {
                    CreateObstacle(isTutorial);
                    Task.Delay(TimeSpan.FromSeconds(1));
                    return;
                }
            }
            GameObject obstacle = Instantiate(obstaclePrefab, new Vector3(x, y, 0f), Quaternion.identity);
            obstacles.Add(obstacle);
            obstacle.transform.localPosition = new Vector3(x, y, 0f);
        }
    }

    public bool IsObstacleOverlapping(float x, float y)
    {
        Vector3 pos = obstacles[0].transform.position;
        if(Math.Sqrt(Math.Pow(Math.Abs(pos.x - x), 2) + Math.Pow(Math.Abs(pos.y - y), 2)) < 5)
        {
            return true;
        }
        return false;
    }
    // public bool checkObstacleDestroyed(){
    //     return FindObjectOfType<Obstacle>().obstacleDestroyed;
    // }
}
