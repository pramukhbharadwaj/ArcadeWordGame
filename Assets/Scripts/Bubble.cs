using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public GameObject whole;
    

    private Rigidbody bubbleRigidBody;
    private Collider bubbleCollider;

    private void Awake()
    {
        bubbleRigidBody = GetComponent<Rigidbody>();
        bubbleCollider = GetComponent<Collider>();
    }

    private void Slice(Vector3 direction, Vector3 position, float force)
    {
        string slicedLetter = gameObject.name[0].ToString();
        
        if(gameObject.tag == "SpecialBubble")
        {
            GameObject specialBubble =  gameObject;
            // Debug.Log(specialBubble.GetComponent<NewBehaviourScript>().life);
            //Debug.Log("In here");
            FindObjectOfType<NewBehaviourScript>().OnSliceSpecialBubble(specialBubble);
        }
        // FindObjectOfType<GameManager>().CheckLetter(slicedLetter)

        // Remove sliced letter from high probability spawning
        else{
            whole.SetActive(false);
            bubbleCollider.enabled = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnManager.totalNumOfSlices++;
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
        }
        if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("Spikes"))
        {
            bool isPattern = false;
            String name = gameObject.name.Split('(')[0];
            if (FindObjectOfType<GameManager>().isLetterInWord(name))
            {
                if (gameObject.transform.parent)
                {
                    if (gameObject.transform.parent.transform.parent)
                    {
                        if (gameObject.transform.parent.transform.parent.name.Contains("Pattern"))
                        {
                            isPattern = true;
                            if (gameObject.transform.parent.transform.parent.GetComponent<Rigidbody>().velocity.y < 0)
                            {
                                Destroy(gameObject);
                                bool isScoreReduced = FindObjectOfType<GameManager>().ReduceCorrectOrb(name);
                                if(isScoreReduced) FindObjectOfType<IntroText>().showSpikeDamage("-10", transform.position);
                            }
                        }
                    }
                }
                if(!isPattern)
                {
                    if (gameObject.GetComponent<Rigidbody>().velocity.y < 0)
                    {
                        Destroy(gameObject);
                        bool isScoreReduced = FindObjectOfType<GameManager>().ReduceCorrectOrb(name);
                        if(isScoreReduced) FindObjectOfType<IntroText>().showSpikeDamage("-10", transform.position);
                    }
                }
            }
        }
    }
    
}