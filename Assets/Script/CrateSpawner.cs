using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Script to instantiate a random number of crates at random positions within the map dimensions
 * such that each crate is at least crateMinPadding tiles away from each other and
 * the edges of the maps. 
 * 
 * A crate's bounds are determined by its Renderer component.
 * A crate's position is not necessarily an integer number.
 * @author Miv
 */
public class CrateSpawner : MonoBehaviour
{
    public GameObject crate;
    public int mapWidthInTiles;
    public int mapHeightInTiles;
    public int minCrates;
    public int maxCrates;
    // Min distance between each crates' edges
    public float crateMinPadding;

    void Start()
    {
        // TODO: use Random.InitState(int seed) such that all players
        // in the game are using the same seed so that crate positions are all the same

        GameObject templateCrate = Instantiate(crate, Vector2.zero, Quaternion.identity);
        Vector2 crateSize = templateCrate.GetComponent<Renderer>().bounds.size;

        int numCrates = Random.Range(minCrates, maxCrates);

        // Split map into rectangles
        float boxSize = Mathf.Sqrt(mapWidthInTiles * mapHeightInTiles / (numCrates * 3f));

		int w = (int)(mapWidthInTiles / boxSize);
		int h = (int)(mapHeightInTiles / boxSize);
        List<int> rand = new List<int>();
        for (int i = 0; i < w*h; i++)
        {
            rand.Add(i);
        }
        for (int i = 0; i < w*h - numCrates; i++)
        {
            rand.RemoveAt(Random.Range(0, rand.Count));
        }

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (!rand.Contains(y*w + x))
                {
                    continue;
                }

				float minX = x * boxSize + crateMinPadding;
				float minY = y * boxSize + crateMinPadding;
				float maxX = (x + 1) * boxSize - crateMinPadding - crateSize.x;
				float maxY = (y + 1) * boxSize - crateMinPadding - crateSize.y;

                Vector2 pos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                Instantiate(crate, pos, Quaternion.identity);
				Debug.Log(pos);
            }
        }
    }
}
