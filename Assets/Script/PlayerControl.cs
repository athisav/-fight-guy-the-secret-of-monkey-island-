using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {

	public float moveSpeed;
	[Tooltip("4 Starting equipped weapons")]
	public GameObject[] weaponPrefabs;

	private GameObject[] weapons;
	private SpriteRenderer[] weaponSpriteRenderers;
	private int activeWeapon;

	private Rigidbody2D rigidbody;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start() {
		rigidbody = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public override void OnStartLocalPlayer() {
		Camera.main.GetComponent<CameraController>().SetTarget(gameObject);

		// Create weapons
		weapons = new GameObject[weaponPrefabs.Length];
		weaponSpriteRenderers = new SpriteRenderer[weaponPrefabs.Length];
		for (int i = 0; i < weaponPrefabs.Length; i++) {
			weapons[i] = Instantiate(weaponPrefabs[i], transform.position, Quaternion.identity) as GameObject;
			// Attach transform to local player
			weapons[i].transform.parent = transform;
			NetworkServer.Spawn(weapons[i]);

			weaponSpriteRenderers[i] = weapons[i].GetComponent<SpriteRenderer>();
		}
		// Disable all weapons except slot 1 (active weapon)
		activeWeapon = 0;
		for (int i = 1; i < weaponPrefabs.Length; i++) {
			weapons[i].SetActive(false);
		}

		//TODO: color player differently to easily identify local player or something
	}

	[Command]
	void CmdSwitchWeapon(int slot) {
		weapons[activeWeapon].SetActive(false);
		activeWeapon = slot;
		weapons[slot].SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}

		// Weapon switching
		if (Input.GetButtonDown("Weapon1")) {
			CmdSwitchWeapon(0);
		} else if (Input.GetButtonDown("Weapon2")) {
			CmdSwitchWeapon(1);
		} else if (Input.GetButtonDown("Weapon3")) {
			CmdSwitchWeapon(2);
		} else if (Input.GetButtonDown("Weapon4")) {
			CmdSwitchWeapon(3);
		}

		// Rotate to mouse cursor
		var mouse = Input.mousePosition;
		var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
		var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
		var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle);
		// Flip sprite across y-axis if -90 < angle < 90
		bool flipY;
		if (angle > 0) {
			flipY = angle > 90;
		} else {
			flipY = angle < -90;
		}
		spriteRenderer.flipY = flipY;
		weaponSpriteRenderers[activeWeapon].flipY = flipY;


		// Move position 
		rigidbody.MovePosition(transform.position + new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0));
	}

}
