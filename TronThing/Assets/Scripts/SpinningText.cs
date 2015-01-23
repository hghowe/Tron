using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpinningText : MonoBehaviour {

	public float angularVelocity = 180f;
	private float count;
	// Use this for initialization
	void Start () {
		count = 0;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up,angularVelocity*Time.deltaTime);
		if (count>0)
		{
			count-=Time.deltaTime;
			if (count<=0)
			{
				gameObject.SetActive(false);
			}
		}
	}

	public void showForDuration(float duration)
	{
		count = duration;
		gameObject.SetActive(true);
	}

	public void setText(string t)
	{
		Text target = gameObject.GetComponent<Text>();
		target.text = t;
	}
}
