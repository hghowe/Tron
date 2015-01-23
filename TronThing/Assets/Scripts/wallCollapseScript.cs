using UnityEngine;
using System.Collections;

public class wallCollapseScript : MonoBehaviour {

	public float duration = 1.5f;
	private float collapseSpeed;
	private bool isCollapsing;
	// Use this for initialization
	void Start () {
		collapseSpeed = transform.localScale.y/duration;
		isCollapsing = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isCollapsing)
		{
			transform.localScale = new Vector3(transform.localScale.x,
		    	                               transform.localScale.y-collapseSpeed*Time.deltaTime,
		        	                           transform.localScale.z);
			if (transform.localScale.y<0)
				Destroy (gameObject);
		}
	}

	public void Collapse()
	{
		isCollapsing = true;
	}
}
