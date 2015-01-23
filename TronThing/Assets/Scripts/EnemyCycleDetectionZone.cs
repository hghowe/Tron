using UnityEngine;
using System.Collections;

public class EnemyCycleDetectionZone : MonoBehaviour {

	private bool wallDetected = false;
	private bool playerDetected = false;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if ("Wall"==other.tag || "BoundaryWall"==other.tag)
		{
			wallDetected = true;
//			Debug.Log ("Just detected a wall.");

		}
		if ("Player" == other.tag)
		{
			playerDetected =true;
//			Debug.Log ("Just detected the player.");
		}

	}

	void OnTriggerExit(Collider other)
	{
		if ("Wall"==other.tag || "BoundaryWall"==other.tag)
		{
			wallDetected = false;
//			Debug.Log ("Just undetected a wall.");
			
		}
		if ("Player" == other.tag)
		{
			playerDetected =false;
//			Debug.Log ("Just undetected the player.");
		}
		
	}

	public bool WallDetected { get{return wallDetected;}}

	public bool PlayerDetected { get{return playerDetected;}}



}
