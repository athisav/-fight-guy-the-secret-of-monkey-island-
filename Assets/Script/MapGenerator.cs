using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Miv 3/31/18
 */
[RequireComponent(typeof(BoxCollider2D))]
public class MapGenerator : MonoBehaviour {
	private class Building {
		public Vector2 position;
		public float leftWidth = 0;
		public float rightWidth = 0;
		public float topHeight = 0;
		public float bottomHeight = 0;
	}

	[SerializeField]
	// -------------------- IMPORTANT: all distances are in tiles --------------------------
	public float mapWidth;
	public float mapHeight;
	public int minBuildings;
	public int maxBuildings;
	// Minimum distance between buildings' edges
	// Also used for padding between buildings' edges and map borders
	public float buildingMinPadding;
	// Building min dimensions
	public float buildingMinWidth;
	public float buildingMinHeight;
	// Building max dimensions
	public float buildingMaxWidth;
	public float buildingMaxHeight;

	public GameObject buildingNineSliceObject;

	private Building[] buildings;
	private int numBuildings;

	void Start () {
		numBuildings = Random.Range(minBuildings, maxBuildings);

		CreateBuildingSpawnPoints();
		bool passed = RandomizeBuildingSizes();
		if (!passed) {
			Start();
		} else {
			DrawBuildings();
		}
	}

	/**
	 * Function to randomize building sizes such that they tend to be smaller than bigger.
	 * @param x In range [0, 1]
	 * @return A number in range [0.1, 1]
	 */
	float RandomBuildingSizeMultiplier(float x) {
		float y = Mathf.Pow(x, 2.5f) + 0.1f;
		return Mathf.Min(1f, y);
	}

	void CreateBuildingSpawnPoints() {
		// Make room for map borders building
		buildings = new Building[numBuildings + 1];

		// Add map borders as a building for easier calculations
		Building borders = new Building();
		borders.position = new Vector2(0, 0);
		borders.rightWidth = mapWidth;
		borders.topHeight = mapHeight;
		buildings[0] = borders;

		// Randomize building spawn points
		// Start at 1 to skip borders building
		for (int i = 1; i < numBuildings + 1; i++) {
			Vector2 point = new Vector2();
			point.Set(Random.Range(buildingMinPadding, mapWidth - buildingMinPadding), Random.Range(buildingMinPadding, mapHeight - buildingMinPadding));

			Building building = new Building();
			building.position = point;
			Debug.Log("Building spawn point at " + building.position);
			buildings[i] = building;
		}
	}

	bool RandomizeBuildingSizes() {
		// Start at 1 to skip borders building
		for (int i = 1; i < numBuildings + 1; i++) {
			Building building = buildings[i];

			// Buildings' sides cannot exceed half of max width/height
			float left = Mathf.Min(buildingMaxWidth/2f, getNearestEdgeToLeft(building));
			float right = Mathf.Min(buildingMaxWidth/2f, getNearestEdgeToRight(building));
			float top = Mathf.Min(buildingMaxHeight/2f, getNearestEdgeToTop(building));
			float bottom = Mathf.Min(buildingMaxHeight/2f, getNearestEdgeToBottom(building));

			//TODO: find better way to make sure all buildings can be of min size
			if (left + right < buildingMinWidth || top + bottom < buildingMinHeight) {
				return false;
			}

			Debug.Log("Building index " + i + ": " + left + ", " + right + ", " + top + ", " + bottom);

			building.leftWidth = left * RandomBuildingSizeMultiplier(Random.value);
			building.rightWidth = right * RandomBuildingSizeMultiplier(Random.value);
			building.topHeight = top * RandomBuildingSizeMultiplier(Random.value);
			building.bottomHeight = bottom * RandomBuildingSizeMultiplier(Random.value);

			Debug.Log("\tSet to " + building.leftWidth + ", " + building.rightWidth + ", " + building.topHeight + ", " + building.bottomHeight);
		}
		return true;
	}

	void DrawBuildings() {
		GameObject templateTile = Instantiate(buildingNineSliceObject, Vector2.zero, Quaternion.identity) as GameObject;
		Vector2 tileSize = templateTile.GetComponent<Renderer> ().bounds.size;

		Destroy(templateTile);

		// Start at 1 to skip borders building
		for (int i = 1; i < numBuildings + 1; i++) {
			Building building = buildings[i];

			// Set position of building to be bottom left of building
			Vector2 position = new Vector2(building.position.x - building.leftWidth, building.position.y - building.bottomHeight);
			GameObject newBuilding = Instantiate(buildingNineSliceObject, position, Quaternion.identity) as GameObject;
			// Set size of building
			newBuilding.GetComponent<SpriteRenderer>().size = new Vector2(building.leftWidth + building.rightWidth, building.bottomHeight + building.topHeight);
			//newBuilding.transform.parent = transform;
		}

		// Make sure template passed in from outside the script is not drawn
		buildingNineSliceObject.GetComponent<SpriteRenderer>().enabled = false;
	}

	float getNearestEdgeToRight(Building source) {
		Debug.Log("Right");
		float min = mapWidth;
		foreach (Building building in buildings) {
			if (building.Equals(source)) {
				continue;
			}
			Debug.Log("min = " + min);
			Debug.Log(source.position.x + " compared to " + (building.position.x - building.leftWidth - buildingMinPadding) + " and " + (building.position.x + building.rightWidth + buildingMinPadding));
			if (source.position.x <= building.position.x - building.leftWidth - buildingMinPadding) {
				if (building.position.x - building.leftWidth - source.position.x - buildingMinPadding < min) {
					min = building.position.x - building.leftWidth - source.position.x - buildingMinPadding;
				}
			} 
			if (source.position.x <= building.position.x + building.rightWidth + buildingMinPadding) {
				if (building.position.x + building.rightWidth - source.position.x + buildingMinPadding < min) {
					min = building.position.x + building.rightWidth - source.position.x + buildingMinPadding;
				}
			}
			Debug.Log("min -> " + min);
		}

		return min;
	}

	float getNearestEdgeToLeft(Building source) {
		Debug.Log("Left");

		float min = mapWidth;
		foreach (Building building in buildings) {
			if (building.Equals(source)) {
				continue;
			}
			Debug.Log("min = " + min);
			Debug.Log(source.position.x + " compared to " + (building.position.x - building.leftWidth - buildingMinPadding) + " and " + (building.position.x + building.rightWidth + buildingMinPadding));
			if (source.position.x >= building.position.x - building.leftWidth - buildingMinPadding) {
				if (source.position.x - (building.position.x - building.leftWidth - buildingMinPadding) < min) {
					min = source.position.x - (building.position.x - building.leftWidth - buildingMinPadding);
				}
			} 
			if (source.position.x >= building.position.x + building.rightWidth + buildingMinPadding) {
				if (source.position.x - (building.position.x + building.rightWidth + buildingMinPadding) < min) {
					min = building.position.x + building.rightWidth - source.position.x + buildingMinPadding;
				}
			}
			Debug.Log("min -> " + min);
		}

		return min;
	}

	float getNearestEdgeToTop(Building source) {
		float min = mapHeight;
		foreach (Building building in buildings) {
			if (building.Equals(source)) {
				continue;
			}

			if (source.position.y <= building.position.y - building.bottomHeight - buildingMinPadding) {
				if (building.position.y - building.bottomHeight - source.position.y - buildingMinPadding < min) {
					min = building.position.y - building.bottomHeight - source.position.y - buildingMinPadding;
				}
			} 
			if (source.position.y <= building.position.y + building.topHeight + buildingMinPadding) {
				if (building.position.y + building.topHeight - source.position.y + buildingMinPadding < min) {
					min = building.position.y + building.topHeight - source.position.y + buildingMinPadding;
				}
			}
		}

		return min;
	}

	float getNearestEdgeToBottom(Building source) {
		float min = mapHeight;
		foreach (Building building in buildings) {
			if (building.Equals(source)) {
				continue;
			}

			if (source.position.y >= building.position.y - building.bottomHeight - buildingMinPadding) {
				if (source.position.y - (building.position.y - building.bottomHeight - buildingMinPadding) < min) {
					min = source.position.y - (building.position.y - building.bottomHeight - buildingMinPadding);
				}
			} 
			if (source.position.y >= building.position.y + building.topHeight + buildingMinPadding) {
				if (source.position.y - (building.position.y + building.topHeight + buildingMinPadding) < min) {
					min = source.position.y - (building.position.y + building.topHeight + buildingMinPadding);
				}
			}
		}

		return min;
	}
}