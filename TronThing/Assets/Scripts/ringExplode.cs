using UnityEngine;
using System.Collections;

public class ringExplode : MonoBehaviour {

	public float expandSpeed = 5.0f;
	public float duration = 0.3f;
	// Use this for initialization
	void Start () {
		Destroy(gameObject,duration);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = new Vector3(transform.localScale.x+expandSpeed*Time.deltaTime,
		                                   transform.localScale.y+expandSpeed*Time.deltaTime,
		                                   transform.localScale.z);
	}
}
