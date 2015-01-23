using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {

	public GameObject player;
	public GameObject enemyModel;
	public int numEnemiesToStart;
	public GameObject playAgainText;


	private List<GameObject> enemyCycles;
	private bool gameIsOver = false;
	private int numEnemies;
	private int score;
	// Use this for initialization


	void Start () {
		enemyCycles = new List<GameObject>();
		//reset ();
		gameIsOver = true;
		score = 0;
	}

	void reset ()
	{
		foreach(GameObject go in enemyCycles)
		{
			Destroy (go);
		}
		numEnemies = 0;
		foreach(GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
		{
//			Debug.Log("Destroyed an extra wall.");
			Destroy (wall);
		}

		enemyCycles.Clear();
		for (int i=0; i<numEnemiesToStart; i++)
			MakeEnemy();
		playAgainText.SetActive(false);
		player.GetComponent<CycleScript>().reset();
		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameIsOver)
		{
			if (Input.GetKey (KeyCode.Space))
			{
				reset ();
				player.GetComponent<CycleScript>().BeginGame();
				foreach(GameObject e in enemyCycles)
				{
					e.GetComponentInChildren<EnemyCycleScript>().BeginGame();
				}
				gameIsOver = false;
			}

		}

	}

	public void endGame()
	{
		playAgainText.GetComponent<SpinningText>().setText("Score:"+score+"\nPress Space\nTo Play Again.");
		playAgainText.SetActive (true);
//		playAgainText.transform.LookAt(GameObject.FindGameObjectWithTag("Camera").transform.position);
		gameIsOver = true;

	}

	public void enemyDied(GameObject enemy)
	{
		if (gameIsOver)
			return;

		float d = Vector3.Distance(enemy.transform.position, player.transform.position);
		int increase = 50 + (int)(5000/(d+1));
		score += increase;
//		Debug.Log("An enemy died. Count is now: "+numEnemies);
		playAgainText.GetComponent<SpinningText>().setText ("+"+increase);
		numEnemies--;
		if (numEnemies < 1)
		{
				playAgainText.GetComponent<SpinningText>().setText("+"+increase+"\nLevel cleared!");
				
			player.GetComponent<CycleScript>().destroyAllWalls();
			player.GetComponent<CycleScript>().spawnWall();
			for(int i=0; i<numEnemiesToStart; i++)
				MakeEnemy();
//			foreach(GameObject enemy in enemyCycles)
//				enemy.GetComponent<EnemyCycleScript>().BeginGame();
		}
		playAgainText.GetComponent<SpinningText>().showForDuration(2.0f);
	}

	void MakeEnemy()
	{
		GameObject enemy = Instantiate(enemyModel,new Vector3(Random.Range(-125,125),0,Random.Range (-125,125)),
		                               Quaternion.identity) as GameObject;

		enemyCycles.Add (enemy);
		numEnemies++;


	}
}
