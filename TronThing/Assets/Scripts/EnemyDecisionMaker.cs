using UnityEngine;
using System.Collections;

public class EnemyDecisionMaker : MonoBehaviour {

	public GameObject frontDetector;
	public GameObject leftSideDetector;
	public GameObject rightSideDetector;
	public GameObject leftRearDetector;
	public GameObject rightRearDetector;
	public GameObject optionalMoodIndicator;

	public float chanceInThousandOfStupidity = 5.0f;
	public float chanceInThousandOfCleverness = 150.0f;
	public float chanceInThousandOfRandomChoices = 10.0f;

	enum Mood{NORMAL, STUPID, CLEVER, RANDOM};
	// Use this for initialization
	float timeToNextMoodChange;
	Mood myMood;
	bool hasIndicator;
	int lastChoice = 0;

	const int FORWARD = 0;
	const int LEFT = 1;
	const int RIGHT = 2;

	const float MIN_MOOD_DURATION = 0.5f;
	const float MAX_MOOD_DURATION = 1.25f;



	void Start()
	{
		timeToNextMoodChange = Random.Range (MIN_MOOD_DURATION,MAX_MOOD_DURATION);
		myMood = Mood.NORMAL;
		hasIndicator = (optionalMoodIndicator != null) && (optionalMoodIndicator.gameObject.activeSelf);

	}

	public int getChoice() // a wrapper for the makeChoice method, which allows us to record the previous choice.
	{
		lastChoice = makeChoice ();
		return lastChoice;
	}

    private int makeChoice()
	{

		timeToNextMoodChange -= Time.deltaTime;
		if (timeToNextMoodChange>0)
		{
			myMood = getMood();

			timeToNextMoodChange = Random.Range (MIN_MOOD_DURATION,MAX_MOOD_DURATION);
		}
		bool frontSeesWall = frontDetector.GetComponent<EnemyCycleDetectionZone>().WallDetected;
		bool leftSeesWall = leftSideDetector.GetComponent<EnemyCycleDetectionZone>().WallDetected;
		bool rightSeesWall = rightSideDetector.GetComponent<EnemyCycleDetectionZone>().WallDetected;
		bool leftRearSeesPlayer = leftRearDetector.GetComponent<EnemyCycleDetectionZone>().PlayerDetected;
		bool rightRearSeesPlayer = rightRearDetector.GetComponent<EnemyCycleDetectionZone>().PlayerDetected;

		int wallAvoidChoice = -1;
		if (frontSeesWall)
		{
			wallAvoidChoice = pickLeftOrRight(15);

			if (leftSeesWall && rightSeesWall) // it's hopeless!
				wallAvoidChoice = FORWARD;
			// otherwise turn away from the wall.
			else if (leftSeesWall)
				wallAvoidChoice = RIGHT;
			else if (rightSeesWall)
				wallAvoidChoice = LEFT;
		}
		switch(myMood)
		{
			case Mood.NORMAL:
				if (wallAvoidChoice >-1)
					return wallAvoidChoice;
				else
					return FORWARD; // keep goin' straight - no walls ahead.
				break;
			case Mood.STUPID:
				if (frontSeesWall)
					return FORWARD;
				if (leftSeesWall)
					return LEFT;
				if (rightSeesWall)
					return RIGHT;
				return FORWARD;
				break;
			case Mood.RANDOM:
				myMood = Mood.NORMAL;// for next time - we've made our choice.
				if (wallAvoidChoice >-1)
					return wallAvoidChoice;
				return pickLeftOrRight(50);
				break;
			case Mood.CLEVER:
				if (wallAvoidChoice >-1)
					return wallAvoidChoice;
				if (rightRearSeesPlayer)
					return RIGHT;
				if (leftRearSeesPlayer)
					return LEFT;
				return FORWARD;
				break;
			default:
				return FORWARD;
		}
	}

	private int pickLeftOrRight(int percentToTurnSameWay)
	{
		if (lastChoice == LEFT)
			if (Random.Range(0,100)>percentToTurnSameWay)
				return RIGHT;
			else
				return LEFT;
		else
			if (Random.Range(0,100)>percentToTurnSameWay)
				return LEFT;
			else
				return RIGHT;
	}

	private Mood getMood()
	{


		int die = Random.Range (0,1000);
		if (die<chanceInThousandOfStupidity)
		{
//			Debug.Log("I'm with Stupid!");
			if (hasIndicator)
				optionalMoodIndicator.gameObject.renderer.material.color = Color.red;
			return Mood.STUPID;
		}
		if (die<chanceInThousandOfStupidity+chanceInThousandOfCleverness)
		{
//			Debug.Log("Very clever, Mr. Bond");
			if (hasIndicator)
				optionalMoodIndicator.gameObject.renderer.material.color = Color.yellow;
			return Mood.CLEVER;
		}
		if (die<chanceInThousandOfStupidity+chanceInThousandOfCleverness+chanceInThousandOfRandomChoices)
		{
//			Debug.Log("random...");
			if (hasIndicator)
				optionalMoodIndicator.gameObject.renderer.material.color = Color.green;

			return Mood.RANDOM;
		}
		if (hasIndicator)
			optionalMoodIndicator.gameObject.renderer.material.color = Color.white;

		return Mood.NORMAL;



	}

}
