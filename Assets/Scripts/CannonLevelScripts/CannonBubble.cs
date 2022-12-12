using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CannonBubble : MonoBehaviour
{
    public GameObject whole;
    private Rigidbody bubbleRigidBody;
    private Collider bubbleCollider;


    public CannonBubble cb;
    public GameObject[] TotalBubbles;
    public LeftSpawner ls;
    //public cannon_special_bubble csb;



    private float jumpForce = 12.5f;
    public float designatedLife;
    public float life;
    [SerializeField] GameObject floatingTextPrefab;
    [SerializeField] GameObject incorrectSlicePrefab;
    private void Awake()
    {
        bubbleRigidBody = GetComponent<Rigidbody>();
        bubbleCollider = GetComponent<Collider>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("floor"))
        {
            if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
            {
                if (FindObjectOfType<CannonTutorialSpawner>().isCannonCollision)
                {
                    FindObjectOfType<CannonTutorialSpawner>().isCannonCollision = false;
                }
            }
            if(gameObject.GetComponent<CannonBubble>().designatedLife == 4)
            {
                jumpForce = 10f;
            }
            else if(gameObject.GetComponent<CannonBubble>().designatedLife == 3)
            {
                jumpForce = 11f;
            }
            else if(gameObject.GetComponent<CannonBubble>().designatedLife == 2)
            {
                jumpForce = 12.5f;
            }
            
            bubbleRigidBody.velocity = new Vector2 (bubbleRigidBody.velocity.x, jumpForce);
			bubbleRigidBody.AddTorque (-bubbleRigidBody.angularVelocity * 4f);
        }

        if(other.CompareTag("bullet"))
        {
            gameObject.transform.localScale = gameObject.transform.localScale * 0.80f;
            // Debug.Log("life : " + this.life);
            gameObject.GetComponent<CannonBubble>().life--;
            int score = 0;



            if(gameObject.GetComponent<CannonBubble>().life <= 0)
            {
                string scoreIncrementText ="";
                string slicedLetter = gameObject.name[0].ToString();
                if(FindObjectOfType<GameManager>().TestLetter(slicedLetter)){
                    switch (gameObject.GetComponent<CannonBubble>().designatedLife)
                    {
                        case 2:
                            scoreIncrementText = "+5";
                            showScoreIncrement("+5");
                            score = 5;
                            break;
                        case 3:
                            scoreIncrementText = "+10";
                            showScoreIncrement("+10");
                            score = 10;
                            break;
                        case 4:
                            scoreIncrementText = "+15";
                            showScoreIncrement("+15");
                            score = 15;
                            break;
                        default:
                            Debug.Log("Invalid word type");
                            scoreIncrementText = "";
                            break;
                    }
                }
                if(gameObject.GetComponent<CannonBubble>().life == 0 & this.CompareTag("cannon_special_bubble"))
                {
                     bool correct = true;
                    if(Random.value<0.1)
                    {
                        correct = false;
                    }

                    int prefIdx;
                    if (SceneManager.GetActiveScene().name == "CannonLevel3-Tutorial")
                    {
                        prefIdx = FindObjectOfType<CannonTutorialSpawner>().preferredAlphabetIndexes[0];   
                    }
                    else
                    {
                        prefIdx = FindObjectOfType<LeftSpawner>()
                            .preferredAlphabetIndexes[
                                Random.Range(0, FindObjectOfType<LeftSpawner>().preferredAlphabetIndexes.Count)];
                    }

                    
                    int idx = correct == true ? prefIdx : Random.Range (0, TotalBubbles.Length);
                    GameObject nextBall = TotalBubbles[idx];
                    gameObject.transform.localScale = gameObject.transform.localScale * 0.00f;
                    
                    GameObject ball1 = Instantiate(nextBall, bubbleRigidBody.position + Vector3.right/ 4f, Quaternion.identity);
                    GameObject ball2 = Instantiate(nextBall, bubbleRigidBody.position + Vector3.left/ 4f, Quaternion.identity);
                    ball1.GetComponent<CannonBubble>().designatedLife = 4;
                    ball2.GetComponent<CannonBubble>().designatedLife = 4;
                    ball1.GetComponent<CannonBubble>().life = 4;
                    ball2.GetComponent<CannonBubble>().life = 4;
                    ball1.GetComponent<CannonBubble>().designatedLife = 4;
                    ball2.GetComponent<CannonBubble>().designatedLife = 4;
                    //float currBubbleDesigLife = ball1.GetComponent<CannonBubble>().designatedLife;
                    // ball1.GetComponent<CannonBubble>().life = currBubbleDesigLife;
                    // float currBubbleDesigLife = ball2.GetComponent<CannonBubble>().designatedLife;
                    // ball2.GetComponent<CannonBubble>().life = currBubbleDesigLife;

                    //float force = Random.Range(2f, 4f);
                    // bubble.gameObject.designatedLife = Random.Range(2,4);

                    
                    ball1.transform.localScale = ball1.transform.localScale * 1.2f;
                    ball2.transform.localScale = ball2.transform.localScale * 1.2f;
                    
                    float force = Random.Range(2f, 3f);
                    ball1.GetComponent<Rigidbody>().AddForce(Vector3.right * force, ForceMode.Impulse);
                    ball2.GetComponent<Rigidbody>().AddForce(-Vector3.right * force, ForceMode.Impulse);
                    
                    if (SceneManager.GetActiveScene().name is "CannonLevel3-Tutorial")
                    {
                        FindObjectOfType<CannonTutorialSpawner>().isDestroyed = true;
                    }
                    
                }
                Destroy(gameObject);
                if (SceneManager.GetActiveScene().name is "CannonLevel1-Tutorial" or "CannonLevel3-Tutorial" or "CannonLevel4-Tutorial")
                {
                    // Debug.Log("Cannon Bubble isDestroyed Value:"+isDestroyed);
                    FindObjectOfType<CannonTutorialSpawner>().isDestroyed = true;
                    // Debug.Log("Cannon Bubble isDestroyed Value:"+isDestroyed);
                }
                Color playerColor = other.gameObject.GetComponent<Renderer>().material.color;
                
                // if(FindObjectOfType<GameManager>().TestLetter(slicedLetter)){
	            //     showScoreIncrement("+10");
                // }
                if (!FindObjectOfType<GameManager>().isLetterInWord(slicedLetter)) {
                    showIncorrectSliceDamage(slicedLetter);
	            }

                bool isLetterComplete = FindObjectOfType<GameManager>().CheckLetter(slicedLetter, playerColor, score);
                if (isLetterComplete)
                {
                    // logic to remove letter from spawner
                    FindObjectOfType<LeftSpawner>().RemoveLetter(slicedLetter);
                    FindObjectOfType<RightSpawner>().RemoveLetter(slicedLetter);
                }

                if(FindObjectOfType<GameManager>().isFinished()){
                    showScoreIncrement("+50");
                    FindObjectOfType<IntroText>().ChangeWord();
                }
            }
        }

        if(other.CompareTag("cannon"))
        {
            Destroy(gameObject);
            string slicedLetter = gameObject.name[0].ToString();
            showIncorrectSliceDamage("Damage: "+slicedLetter, true);
            FindObjectOfType<GameManager>().FillIncorrectOrb();
            if (SceneManager.GetActiveScene().name is "CannonLevel1-Tutorial" or "CannonLevel3-Tutorial" or "CannonLevel4-Tutorial")
            {
                // Debug.Log("Cannon Bubble isDestroyed Value:"+isDestroyed);
                FindObjectOfType<CannonTutorialSpawner>().isCannonCollision = true;
                FindObjectOfType<CannonTutorialSpawner>().cannonCollisionHappened = true;
                // Debug.Log("Cannon Bubble isDestroyed Value:"+isDestroyed);
            }
        }
    }
    public void showScoreIncrement(string text, bool isColored=false){
        if(floatingTextPrefab)
        {
            GameObject prefab = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TMP_Text>().text = text;
            if (isColored)
            {
                prefab.GetComponentInChildren<TMP_Text>().color = Color.red;
            }
        }
    }

    public void showIncorrectSliceDamage(string text, bool isColored=false){
        if(incorrectSlicePrefab)
        {
            GameObject prefab = Instantiate(incorrectSlicePrefab, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TMP_Text>().text = text;
            if (isColored)
            {
                prefab.GetComponentInChildren<TMP_Text>().color = Color.red;
            }
        }
    }

    
}