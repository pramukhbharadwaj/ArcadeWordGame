using UnityEngine;

public class Cannon : MonoBehaviour
{
	public Camera MainCamera; 
    private Vector2 screenBounds;
	private float objectWidth = 1;

	void Start () {
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
		SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
		for(int i = 0; i < sprites.Length; i++){
			//Debug.Log(sprites[i].sprite.name);
 			if(sprites[i].sprite.name == "cannon")
				objectWidth = sprites[i].bounds.size.x;

 		}
	}

	void Update () {
		
		if (Input.GetKey(KeyCode.LeftArrow))
		{
				Vector3 position = this.transform.position;
				position.x = Mathf.Clamp(position.x - 0.1f, screenBounds.x * -1 + objectWidth +1, screenBounds.x - objectWidth -1);
				this.transform.position = position;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
				Vector3 position = this.transform.position;
				position.x = Mathf.Clamp(position.x + 0.1f, screenBounds.x * -1 + objectWidth+1, screenBounds.x - objectWidth -1);
				this.transform.position = position;
		}
	}
}
