using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyCycleScript : MonoBehaviour {

	public float speed = 5.0f;
	public GameObject wallModel;
	public GameObject explosion;

	private GameObject currentWall;
	private List<GameObject> myWalls;
	private float offsetToWallStart = 0.0f;
	private float turnAngle = 90; // final value should be 90°
	private float actualSpeed;
	private bool gameIsOver = true;
	private float myDirection = 0;
	private float timeSinceLastTurn = 0.0f;

	const float minTimeToTurn = 0.05f;
	const int FORWARD = 0;
	const int LEFT = 1;
	const int RIGHT = 2;
	// Use this for initialization
	void Start () {
		myWalls = new List<GameObject>();
		spawnWall();
		BeginGame();
	}
	public void BeginGame()
	{
		actualSpeed = speed;
		gameIsOver = false;
	}
	// Update is called once per frame
	void Update () {

		if (gameIsOver)
			return;
		timeSinceLastTurn += Time.deltaTime;
		int choice = gameObject.GetComponentInParent<EnemyDecisionMaker>().getChoice();

		if (LEFT == choice )
			TurnLeft();
		if (RIGHT == choice)
			TurnRight ();

		transform.parent.Translate(Vector3.forward*actualSpeed*Time.deltaTime);
		currentWall.transform.localScale = new Vector3(currentWall.transform.localScale.x,
		                                               currentWall.transform.localScale.y,
		                                               currentWall.transform.localScale.z+actualSpeed*Time.deltaTime*10);
	
			

	}

	void TurnLeft()
	{
		if (timeSinceLastTurn>minTimeToTurn)
		{	transform.parent.Rotate(Vector3.up,-turnAngle);
			spawnWall();
			myDirection -=90;
			timeSinceLastTurn = 0.0f;
		}
	}

	void TurnRight()
	{
		if (timeSinceLastTurn>minTimeToTurn)
		{
			transform.parent.Rotate (Vector3.up,turnAngle);
			spawnWall ();
			myDirection +=90;
			timeSinceLastTurn = 0.0f;
		}
	}

	void spawnWall()
	{
		currentWall = Instantiate(wallModel,transform.parent.position+Vector3.up*-0.4f+(Vector3.forward*-offsetToWallStart),transform.rotation) as GameObject;
		currentWall.transform.Rotate(Vector3.up,180);
		currentWall.transform.localScale = new Vector3(currentWall.transform.localScale.x,
		                                               currentWall.transform.localScale.y,
		                                               0.01f);
		myWalls.Add(currentWall);
	}

	public void collapseAllWalls()
	{
		foreach(GameObject GO in myWalls)
		{
			GO.GetComponent <wallCollapseScript>().Collapse();
		}
		
	}

	public void destructor()
	{
		deleteAllWalls();
		Destroy(gameObject);
	}

	public void deleteAllWalls()
	{
//		Debug.Log ("Deleting my walls.");
		foreach(GameObject GO in myWalls)
		{
			Destroy(GO);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Wall" || other.tag == "BoundaryWall" || other.tag == "Player")
		{

//			if (currentWall == other)
//				Debug.Log ("Enemy just hit his own wall. Velocity was: "+rigidbody.velocity);
//			if (other.tag == "Wall")
//				Debug.Log ("Enemy hit a wall. Direction was: "+myDirection);

			actualSpeed = 0;
			GameObject explodeGO = Instantiate (explosion,transform.position,transform.rotation) as GameObject;
			explodeGO.transform.Rotate(Vector3.right,-90);
			Destroy(explodeGO,2);
			collapseAllWalls();
			Destroy (gameObject);
			GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerScript>().enemyDied(gameObject);
		}
	}


}
