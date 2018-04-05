using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FillGround : MonoBehaviour {
	[SerializeField]
	public GameObject tileObject;
	public float widthInTiles;
	public float heightInTiles;

	void Start () {
		DrawTiledBackground();
	}
	
	void DrawTiledBackground() {
		var templateTile = Instantiate(tileObject, Vector2.zero, Quaternion.identity) as GameObject;
		Vector2 tileSize = templateTile.GetComponent<Renderer>().bounds.size;

		Destroy(templateTile);

		for (int i = 0; i < widthInTiles; i++) {
			for (int j = 0; j < heightInTiles; j++) {
				var newTilePos = new Vector2 (i * tileSize.x, j * tileSize.y);
				var newTile = Instantiate (tileObject, newTilePos, Quaternion.identity) as GameObject;
				newTile.transform.parent = transform;
			}
		}

		tileObject.GetComponent<SpriteRenderer>().enabled = false;
	}
}
