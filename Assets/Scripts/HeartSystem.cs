using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartSystem : MonoBehaviour
{

    public GameObject[] hearts; //[0] [1] [2]

    [SerializeField] private float fallVelocity;

    private bool dead;

    void Awake()
    {
        
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true)
        {
            //SET DEAD CODE: Display the Game Over scene
            SceneManager.LoadScene("GameOver");
        }
    }

    public void TakeDamage()
    {
        LivesManager.lives -= 1; //3-1=2
        //Destroy(hearts[LivesManager.lives].gameObject); //[2]
        HeartFall();
        if (LivesManager.lives < 1)
        {
            dead = true;
        }
    }

    public int GetLivesEnded()
    {
        return 3 - LivesManager.lives;
    }

    public void HeartFall()
    {
        hearts[LivesManager.lives].gameObject.GetComponent<Rigidbody>().useGravity = true;
        hearts[LivesManager.lives].gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, -fallVelocity, 0.0f);;
    }

    private void OnBecameInvisible()
    {
        Destroy(hearts[LivesManager.lives].gameObject); //[2]
    }
}