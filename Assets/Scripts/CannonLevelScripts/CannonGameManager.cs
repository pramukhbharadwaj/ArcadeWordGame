using UnityEngine;

public class CannonGameManager : MonoBehaviour
{
	#region Singleton class: Game

	public static CannonGameManager Instance;

	void Awake ()
	{
		Instance = this;
		screenWidth = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, 0f, 0f)).x;
	}

	#endregion

	[HideInInspector]public float screenWidth;
}

