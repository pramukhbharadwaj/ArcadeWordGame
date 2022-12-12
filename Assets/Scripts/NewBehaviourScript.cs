using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class NewBehaviourScript : MonoBehaviour
{
    public int life=3;
    public GameObject whole;
    
    private Rigidbody bubbleRigidBody;
    private Collider bubbleCollider;
    
    // Start is called before the first frame update
    
    
        
     public void OnSliceSpecialBubble(GameObject specialBubble)
     {
         
        specialBubble.GetComponent<NewBehaviourScript>().life--;
        

        // Debug.Log(specialBubble.GetComponent<NewBehaviourScript>().life);
        int lives = specialBubble.GetComponent<NewBehaviourScript>().life;
 
         if (lives == 2)
         {
             
             specialBubble.transform.localScale = specialBubble.transform.localScale * 0.80f;
             
            // var renderer = GetComponent<MeshRenderer>();
            // Material[] materials = renderer.sharedMaterials;
            //  SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            //  renderer.color = new Color(0.5f, 0.5f, 0.5f, 1f);

            //GetComponent<MeshRenderer>().material = materials[0];
             //GetComponent<SpriteRenderer>().color = new Color(1f,0.30196078f, 0.30196078f);
         }
         if (lives == 1)
         {
            //  GetComponent<SpriteRenderer>().color = new Color(0.388235229f, 0.3372549f, 1f);
             specialBubble.transform.localScale = specialBubble.transform.localScale * 0.90f;
         }
         
         if (lives <=0)
         {  
            // Debug.Log("life 0, destroy newbehvmonoscript ");
            //showScoreIncrement("+50");
            //Debug.Log("life 0 color change if : "  +  specialBubble.name);
            FindObjectOfType<GameManager>().IncrementAllOrbsByAmt((float)1.0f, specialBubble.name);
            ScoreManager.IncreaseScore(50);
            FindObjectOfType<GameManager>().scoreText.text = "Score: " + ScoreManager.score.ToString();
            
            Destroy(gameObject);
            SpawnManager.numOfSpecialBubblesSliced++;
         }
         
 
     }


    public int get_special_bubble_life(){
        return life;
    }
     
}
