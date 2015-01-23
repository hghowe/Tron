using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CycleScript : MonoBehaviour {

	public float speed = 5.0f;
	public GameObject cameraTrailer;
	public GameObject wallModel;
	public float cameraAngularRestoreVelocity = 20.0f;
	public GameObject explosion;
	public GameObject gameManager;
	public AudioClip turnSound;
	public AudioClip explodeSound;

	private AudioSource aSource;
	private GameObject currentWall;
	private List<GameObject> myWalls;
	private float offsetToWallStart = 0.0f;
	private float trailerRelativeAngle;
	private float turnAngle = 90; // final value should be 90°
	private float actualSpeed;
	private bool gameIsOver = true;
	// Use this for initialization

	void Awake()
	{
		aSource = GetComponent<AudioSource>();
	}
	void Start () {
		myWalls = new List<GameObject>();
		trailerRelativeAngle = 0;
	}

	public void reset()
	{
		transform.parent.position = Vector3.zero;
		SnapCameraBehindCycle();
		actualSpeed = 0;
		destroyAllWalls();
		spawnWall();
		gameObject.SetActive(true);
	}


	public void BeginGame()
	{
		actualSpeed = speed;
		gameIsOver = false;
		aSource.Play();
	}

	// Update is called once per frame
	void Update () {
	
		if (!gameIsOver)
		{

	//		if (Input.GetKey(KeyCode.W)) // cycle moves forward....
	//		{
				transform.parent.Translate(Vector3.forward*actualSpeed*Time.deltaTime);
				currentWall.transform.localScale = new Vector3(currentWall.transform.localScale.x,
				                                               currentWall.transform.localScale.y,
				                                               currentWall.transform.localScale.z+actualSpeed*Time.deltaTime*10);
	//		}
			if (Input.GetKeyDown(KeyCode.A)) // cycle turns left; camera does not yet turn.
			{
				TurnLeft ();
			}
			if (Input.GetKeyDown(KeyCode.D)) // cycle turns right; camera does not yet turn.
			{
				TurnRight();
			}
		}
		SwingCameraBehindCycle();
	}

	void SnapCameraBehindCycle()
	{
		cameraTrailer.transform.RotateAround(transform.position,Vector3.up,-trailerRelativeAngle);
		trailerRelativeAngle = 0;
	}

	void SwingCameraBehindCycle()
	{
		normalizeRelativeAngle ();
		if (trailerRelativeAngle>0)
		{
			float turnAmount = Mathf.Min(cameraAngularRestoreVelocity*Time.deltaTime,trailerRelativeAngle);
			cameraTrailer.transform.RotateAround(transform.position,Vector3.up,-turnAmount);
			trailerRelativeAngle -= turnAmount;
		}
		if (trailerRelativeAngle<0)
		{
			float turnAmount = Mathf.Min(cameraAngularRestoreVelocity*Time.deltaTime,-trailerRelativeAngle);
			cameraTrailer.transform.RotateAround(transform.position,Vector3.up,turnAmount);
			trailerRelativeAngle += turnAmount;
		}
	}

	void TurnLeft()
	{
		aSource.PlayOneShot(turnSound,1.0f);
		transform.parent.Rotate(Vector3.up,-turnAngle);
		cameraTrailer.transform.RotateAround(transform.position,Vector3.up,turnAngle);
		trailerRelativeAngle += turnAngle;
		spawnWall();
	}

	void TurnRight()
	{
		aSource.PlayOneShot(turnSound,1.0f);
		transform.parent.Rotate (Vector3.up,turnAngle);
		cameraTrailer.transform.RotateAround(transform.position,Vector3.up,-turnAngle);
		trailerRelativeAngle -= turnAngle;
		spawnWall ();
	}

	void normalizeRelativeAngle ()
	{
		while(trailerRelativeAngle>180)
			trailerRelativeAngle -= 360;
		while(trailerRelativeAngle<=-180)
			trailerRelativeAngle += 360;
	}

	public void spawnWall()
	{
		currentWall = Instantiate(wallModel,transform.parent.position+Vector3.up*-0.2f+(Vector3.forward*-offsetToWallStart),transform.rotation) as GameObject;
		currentWall.transform.Rotate(Vector3.up,180);
		currentWall.transform.localScale = new Vector3(currentWall.transform.localScale.x,
		                                         currentWall.transform.localScale.y,
		                                         0.065f);
		myWalls.Add(currentWall);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Wall" || other.tag == "BoundaryWall" || other.tag == "EnemyCycle")
		{
			//Debug.Log("Wall!");
			aSource.Stop();
			//aSource.PlayOneShot(explodeSound,1.0f);
			actualSpeed = 0;
			GameObject explodeGO = Instantiate (explosion,transform.position,transform.rotation) as GameObject;
			explodeGO.transform.Rotate(Vector3.right,-90);
			Destroy(explodeGO,2);
			collapseAllWalls();
			gameObject.SetActive(false);
			gameManager.GetComponent<GameManagerScript>().endGame();
		}
	}

	public void destroyAllWalls()
	{
		foreach(GameObject GO in myWalls)
		{
			Destroy (GO);
		}
		myWalls.Clear();
	}

	public void collapseAllWalls()
	{
		foreach(GameObject GO in myWalls)
		{
			GO.GetComponent <wallCollapseScript>().Collapse();
		}
		//myWalls.Clear();
	}
}
