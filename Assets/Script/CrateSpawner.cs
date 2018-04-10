using System;

/**
 * Script to instantiate a random number of crates at random positions within the map dimensions
 * such that each crate is at least crateMinPadding tiles away from each other and
 * the edges of the maps. 
 * 
 * A crate's bounds are determined by its Renderer component.
 * A crate's position is not necessarily an integer number.
 * @author Miv
 */
public class CrateSpawner
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
        GameObject templateCrate = Instantiate(crate, Vector2.zero, Quaternion.identity);
        Vector2 crateSize = templateCrate.GetComponent<Renderer>().bounds.size;

        float minX = crateMinPadding;
        float minY = crateMinPadding;
        float maxX = mapWidthInTiles - maxX;
        float maxY = mapHeightInTiles - maxY;

        for (int i = 0; i < Random.Range(minCrates, maxCrates); i++)
        {
            Vector2 pos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            Instantiate(crate, pos, Quaternion.identity);
        }
    }
}
