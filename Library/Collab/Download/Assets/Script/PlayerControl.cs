using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {

	public float moveSpeed;
	private Animator anim;
	private Rigidbody2D rigidbody;
	private BoxCollider2D boxCollider;

	// Use this for initialization
	void Start() {
		anim = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();
	}

	public override void OnStartLocalPlayer() {
		Camera.main.GetComponent<CameraController>().SetTarget(gameObject);
		//TODO: color player differently to easily identify local player or something
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}

		rigidbody.MovePosition(transform.position + new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0));

		anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
		anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
	}
}
