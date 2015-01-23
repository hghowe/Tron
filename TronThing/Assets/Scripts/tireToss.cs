using UnityEngine;
using System.Collections;

public class tireToss : MonoBehaviour {

	public float duration = 0.6f;
	public float speed = 25.0f;
	// Use this for initialization
	void Start () {
		float direction = Random.Range(-60,+60);
		transform.Rotate (Vector3.right,-direction);
		rigidbody.AddForce(new Vector3(100*speed*Mathf.Sin(direction),100*speed,100*speed*Mathf.Cos(direction)));
		Destroy (gameObject,duration);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Floor" && rigidbody.velocity.y <0)
		{
			rigidbody.velocity = new Vector3(rigidbody.velocity.x,-rigidbody.velocity.y,rigidbody.velocity.z);
		}
	}
}
