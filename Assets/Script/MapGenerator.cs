using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * DOESN'T WORK WITH MULTIPLAYER
 * @author Miv 3/31/18
 */
[RequireComponent(typeof(BoxCollider2D))]
public class MapGenerator : MonoBehaviour {
	private class Building {
		public Vector2 position;
		public float width;
		public float height;
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
		borders.width = mapWidth;
		borders.height = mapHeight;
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
			float left = getNearestEdgeToLeft(building);
			float right = getNearestEdgeToRight(building);
			float top = getNearestEdgeToTop(building);
			float bottom = getNearestEdgeToBottom(building);
			Debug.Log(i + ": ");
			Debug.Log("\tNearest edge distances:");
			Debug.Log("\t\tright="+right);
			Debug.Log("\t\tleft="+left);
			Debug.Log("\t\ttop="+top);
			Debug.Log("\t\tbottom="+bottom);
			Debug.Log("\tPosition: " + building.position);

			//TODO: find better way to make sure all buildings can be of min size
			if (left + right < buildingMinWidth || top + bottom < buildingMinHeight) {
				return false;
			}

			// Move building spawn point to be center of nearest edges
			float oldLeft = left;
			float oldRight = right;
			float oldTop = top;
			float oldBottom = bottom;

			building.position.Set(building.position.x + (right - left) / 2f, building.position.y + (top - bottom) / 2f);
			// Adjust edge distances accordingly
			right -= (oldRight - oldLeft) / 2f;
			left += (oldRight - oldLeft) / 2f;
			top -= (oldTop - oldBottom) / 2f;
			bottom += (oldTop - oldBottom) / 2f;

			Debug.Log("\tAdjusted nearest edge distances:");
			Debug.Log("\t\tright="+right);
			Debug.Log("\t\tleft="+left);
			Debug.Log("\t\ttop="+top);
			Debug.Log("\t\tbottom="+bottom);
			Debug.Log("\tPosition: " + building.position);
			Debug.Log("\tAdjusted position: " + building.position);

			building.width = Mathf.Min(buildingMaxWidth, (left + right) * RandomBuildingSizeMultiplier(Random.value));
			building.height = Mathf.Min(buildingMaxHeight, (top + bottom) * RandomBuildingSizeMultiplier(Random.value));

			Debug.Log("\tWidth=" + building.width + "\n\tHeight="+building.height);

			// Adjust building position because pivot is at bottom-left
			building.position.Set(building.position.x - building.width/2f, building.position.y - building.height/2f);
				
			Debug.Log("\tFinal adjusted position: " + building.position);
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

			GameObject newBuilding = Instantiate(buildingNineSliceObject, building.position, Quaternion.identity) as GameObject;
			// Set size of building
			newBuilding.GetComponent<SpriteRenderer>().size = new Vector2(building.width, building.height);
			//newBuilding.transform.parent = transform;
		}

		// Make sure template passed in from outside the script is not drawn
		buildingNineSliceObject.GetComponent<SpriteRenderer>().enabled = false;
	}

	float getNearestEdgeToRight(Building source) {
		//Debug.Log("Right");
		float min = mapWidth;
		foreach (Building building in buildings) {
			if (building.Equals(source)) {
				continue;
			}

			// Building must be able to collide on y-axis
			if (!yOverlap(source, building)) {
				continue;
			}

			//Debug.Log("min = " + min);
			//Debug.Log(source.position.x + " compared to " + (building.position.x - building.leftWidth - buildingMinPadding) + " and " + (building.position.x + building.rightWidth + buildingMinPadding));
			if (source.position.x <= building.position.x - buildingMinPadding) {
				if (building.position.x - source.position.x - buildingMinPadding < min) {
					min = building.position.x - source.position.x - buildingMinPadding;
				}
			} 
			if (source.position.x <= building.position.x + building.width + buildingMinPadding) {
				if (building.position.x + building.width - source.position.x + buildingMinPadding < min) {
					min = building.position.x + building.width - source.position.x + buildingMinPadding;
				}
			}
			//Debug.Log("min -> " + min);
		}

		return min;
	}

	float getNearestEdgeToLeft(Building source) {
		//Debug.Log("Left");

		float min = mapWidth;
		foreach (Building building in buildings) {
			if (building.Equals(source)) {
				continue;
			}

			// Building must be able to collide on y-axis
			if (!yOverlap(source, building)) {
				continue;
			}

			//Debug.Log("min = " + min);
			//Debug.Log(source.position.x + " compared to " + (building.position.x - buildingMinPadding) + " and " + (building.position.x + building.width + buildingMinPadding));
			if (source.position.x >= building.position.x - buildingMinPadding) {
				if (source.position.x - (building.position.x - buildingMinPadding) < min) {
					min = source.position.x - (building.position.x - buildingMinPadding);
				}
			} 
			if (source.position.x >= building.position.x + building.width + buildingMinPadding) {
				if (source.position.x - (building.position.x + building.width + buildingMinPadding) < min) {
					min = building.position.x + building.width - source.position.x + buildingMinPadding;
				}
			}
			//Debug.Log("min -> " + min);
		}

		return min;
	}

	float getNearestEdgeToTop(Building source) {
		float min = mapHeight;
		foreach (Building building in buildings) {
			if (building.Equals(source)) {
				continue;
			}

			// Building must be able to collide on x-axis
			if (!xOverlap(source, building)) {
				continue;
			}

			if (source.position.y <= building.position.y - buildingMinPadding) {
				if (building.position.y - source.position.y - buildingMinPadding < min) {
					min = building.position.y - source.position.y - buildingMinPadding;
				}
			} 
			if (source.position.y <= building.position.y + building.height + buildingMinPadding) {
				if (building.position.y + building.height - source.position.y + buildingMinPadding < min) {
					min = building.position.y + building.height - source.position.y + buildingMinPadding;
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

			// Building must be able to collide on x-axis
			if (!xOverlap(source, building)) {
				continue;
			}

			if (source.position.y >= building.position.y - buildingMinPadding) {
				if (source.position.y - (building.position.y - buildingMinPadding) < min) {
					min = source.position.y - (building.position.y - buildingMinPadding);
				}
			} 
			if (source.position.y >= building.position.y + building.height + buildingMinPadding) {
				if (source.position.y - (building.position.y + building.height + buildingMinPadding) < min) {
					min = source.position.y - (building.position.y + building.height + buildingMinPadding);
				}
			}
		}

		return min;
	}

	bool yOverlap(Building a, Building b) {
		return true;
		//return a.position.y < b.position.y + Mathf.Max(buildingMinHeight, b.height) + buildingMinPadding && a.position.y + Mathf.Max(buildingMinHeight, a.height) + buildingMinPadding > b.position.y;
	}

	bool xOverlap(Building a, Building b) {
		return true;
		//return a.position.x < b.position.x + Mathf.Max(buildingMinWidth, b.width) + buildingMinPadding && a.position.x + Mathf.Max(buildingMinWidth, a.width) + buildingMinPadding > b.position.x;
	}
}