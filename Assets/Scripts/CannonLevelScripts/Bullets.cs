using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullets : MonoBehaviour
{
	Queue<GameObject> bulletsQueue;

	[SerializeField] GameObject bulletPrefab;
	[SerializeField] int bulletCount;

	[Space]
	[SerializeField] float delay = 0.3f;
	[SerializeField] float speed = 0.3f;

	GameObject g;
	float t = 0f;

	#region Singleton class: Bullets

	public static Bullets Instance;

	void Awake ()
	{
		Instance = this;
	}

	#endregion

	void Start ()
	{
		PrepareBullets ();
	}

	void Update ()
	{
		if (Input.GetKey("space"))
        {
            t += Time.deltaTime;
			if (t >= delay) {
				t = 0f;
				g = SpawnBullet(transform.position);
				if (g != null)
					g.GetComponent <Rigidbody> ().velocity = Vector2.up * speed;
			}
        }

	}

	void PrepareBullets()
	{
		bulletsQueue = new Queue<GameObject> ();
		for (int i = 0; i < bulletCount; i++) {
			g = Instantiate (bulletPrefab, transform.position, Quaternion.identity, transform);
			g.SetActive (false);
			bulletsQueue.Enqueue (g);
		}
	}

	public GameObject SpawnBullet(Vector2 position)
	{
		if (bulletsQueue.Count > 0) {
			g = bulletsQueue.Dequeue();
			g.transform.position = position;
			g.SetActive (true);
			return g;
		}

		return null;
	}

	//missile collision with top collider
	void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("bullet")) {
			DestroyBullet(other.gameObject);
		}
	}


	public void DestroyBullet (GameObject bullet)
	{
		bulletsQueue.Enqueue (bullet);
		bullet.SetActive (false);
	}
}
