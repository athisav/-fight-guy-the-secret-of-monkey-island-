using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/**
 * 
 * 
 * @author Miv
 */
public class FillGround : NetworkBehaviour {
	// GameObject used to hold all tiles
	public GameObject tilesContainer;

	// Tile used to fill inside of map
	public GameObject fillTile;
	// Tile that surrounds the map
	public GameObject boundaryTile;

	public int widthInTiles;
	public int heightInTiles;

	private TilesManager tileManager;

	void Awake() {
		// Initialize TilesContainer tiles
		tileManager = GameObject.Find("Tiles Manager").GetComponent<TilesManager>();
		tileManager.InitializeTiles(widthInTiles, heightInTiles);

		DrawTiledBackground();
	}
	
	void DrawTiledBackground() {
		var boundaryTemplate = Instantiate(boundaryTile, Vector2.zero, Quaternion.identity) as GameObject;
		Vector2 boundarySize = boundaryTemplate.GetComponent<Renderer>().bounds.size;
		Destroy(boundaryTemplate);

		var fillTemplate = Instantiate(fillTile, Vector2.zero, Quaternion.identity) as GameObject;
		Vector2 fillSize = fillTemplate.GetComponent<Renderer>().bounds.size;
		Destroy(fillTemplate);

		// Left boundary
		for (int j = 0; j < heightInTiles - 1; j++) {
			var newTilePos = new Vector2 (0, boundarySize.y + j * fillSize.y);
			var newTile = Instantiate(boundaryTile, newTilePos, Quaternion.identity) as GameObject;
			NetworkServer.Spawn(newTile);

			tileManager.SetTile(0, j, newTile);
		}

		// Right boundary
		for (int j = 0; j < heightInTiles - 1; j++) {
			var newTilePos = new Vector2(boundarySize.x + (widthInTiles - 2) * fillSize.x, boundarySize.y + j * fillSize.y);
			var newTile = Instantiate(boundaryTile, newTilePos, Quaternion.identity) as GameObject;
			NetworkServer.Spawn(newTile);

			tileManager.SetTile(widthInTiles - 1, j, newTile);
		}

		// Top boundary
		for (int i = 0; i < widthInTiles; i++) {
			var newTilePos = new Vector2 (i * boundarySize.x, boundarySize.y + (heightInTiles - 2) * fillSize.y);
			var newTile = Instantiate(boundaryTile, newTilePos, Quaternion.identity) as GameObject;

			tileManager.SetTile(i, heightInTiles - 1, newTile);
		}

		// Bottom boundary
		for (int i = 0; i < widthInTiles; i++) {
			var newTilePos = new Vector2(i * boundarySize.x, 0);
			var newTile = Instantiate(boundaryTile, newTilePos, Quaternion.identity) as GameObject;
			NetworkServer.Spawn(newTile);

			tileManager.SetTile(i, 0, newTile);
		}

		// -2 for the boundary tiles
		for (int i = 0; i < widthInTiles - 2; i++) {
			for (int j = 0; j < heightInTiles - 2; j++) {
				var newTilePos = new Vector2 (boundarySize.x + i * fillSize.x, boundarySize.y + j * fillSize.y);
				var newTile = Instantiate(fillTile, newTilePos, Quaternion.identity) as GameObject;
				NetworkServer.Spawn(newTile);

				tileManager.SetTile(i, j, newTile);
			}
		}

		fillTile.GetComponent<SpriteRenderer>().enabled = false;
		boundaryTile.GetComponent<SpriteRenderer>().enabled = false;
	}
}
