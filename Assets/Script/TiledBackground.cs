using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TiledBackground : MonoBehaviour {
		[SerializeField]
		public GameObject tileObject;
		public BoxCollider2D canvasCollider;

		void Start () {
				canvasCollider = GetComponent<BoxCollider2D> ();
				DrawTiledBackground();
			}
		
		void DrawTiledBackground() {
				Vector2 canvasSize = canvasCollider.bounds.size;

				var templateTile = Instantiate (tileObject, Vector2.zero, Quaternion.identity) as GameObject;
				Vector2 tileSize = templateTile.GetComponent<Renderer> ().bounds.size;

				float tilesX = canvasSize.x / tileSize.x;
				float tilesY = canvasSize.y / tileSize.y;
				Destroy (templateTile);

				Vector2 bottomLeft = new Vector2 (canvasCollider.transform.position.x - canvasSize.x/2 + tileSize.x/2, canvasCollider.transform.position.y - canvasSize.y/2 + tileSize.y/2);

				for (int i = 0; i < tilesX; i++) {
						for (int j = 0; j < tilesY; j++) {
								var newTilePos = new Vector2 (bottomLeft.x + i * tileSize.x, bottomLeft.y + j * tileSize.y);
								var newTile = Instantiate (tileObject, newTilePos, Quaternion.identity) as GameObject;
								newTile.transform.parent = transform;
							}
					}

				tileObject.GetComponent<SpriteRenderer>().enabled = false;
			}
	}