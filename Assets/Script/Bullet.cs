using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float initialSpeed;
	public float acceleration;
	public float maxSpeed;

	void Start () {
		// Set initial speed
		gameObject.GetComponent<Rigidbody2D>().velocity = transform.forward * initialSpeed;
		// Adjust transform so that bullet spawns at an offset with respect to the parent weapon
		transform.position = transform.position + transform.parent.gameObject.GetComponent<Weapon>().bulletSpawnOffset;
	}

	void Update () {
		//TODO: do acceleration stuff but make sure 0 <= currentSpeed <= maxSpeed always
	}
}
