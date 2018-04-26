using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Component used by the TilesManager GameObject to hold and manage all tiles in the map.
 * The tiles variable is initialized by the FillGround script.
 * 
 * @author Miv
 */
public class TilesManager : MonoBehaviour {
	public class Tile {
		// Position of the tile in the grid
		public int x;
		public int y;

		public GameObject gameObject;

		bool itemCanSpawnOn;

		public Tile(int x, int y, GameObject gameObject) {
			this.x = x;
			this.y = y;
			this.gameObject = gameObject;
		}
	}

	private Tile[,] tiles;

	public void InitializeTiles(int xLength, int yLength) {
		tiles = new Tile[xLength, yLength];
	}

	public void SetTile(int x, int y, GameObject tile) {
		tiles[x, y] = new Tile(x, y, tile);

		// TODO: check if tile is collidable --> set tiles[x, y].itemCanSpawnOn
	}

	public Tile GetRandomItemSpawnableTile() {
		//TODO: have a list of tiles with itemCanSpawnOn == false --> pick a random
		return null;
	}
}
