using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {

	public float moveSpeed;
	private Rigidbody2D rigidbody;

	// Use this for initialization
	void Start() {
		rigidbody = GetComponent<Rigidbody2D>();
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

		var mouse = Input.mousePosition;
		var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
		var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
		var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle);
		rigidbody.MovePosition(transform.position + new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0));
	}

}
