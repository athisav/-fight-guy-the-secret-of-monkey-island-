using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject target;

	// Offset between player and camera
	private Vector3 offset;

	// Use this for initialization
	void Start() {
		// Initial offset depends on how scene was set up
		offset = transform.position - target.transform.position;
		// Deactivate player dummy
		target.SetActive(false);
	}
	
	// Update is called once per frame
	void LateUpdate() {
		transform.position = target.transform.position + offset;
	}

	public void SetTarget(GameObject target) {
		// Maintain initial offset
		this.target = target;
		transform.position = target.transform.position + offset;
	}
}
