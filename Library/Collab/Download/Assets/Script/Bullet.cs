using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float initialSpeed;
	public float acceleration;
	public float maxSpeed;

	void Start () {
		// Set initial speed
		//Debug.Log(transform.rotation.eulerAngles);
		gameObject.GetComponent<Rigidbody2D>().velocity = transform.rotation * new Vector2(initialSpeed, 0);
	}

	void Update () {
		//TODO: do acceleration stuff but make sure 0 <= currentSpeed <= maxSpeed always
	}
}
