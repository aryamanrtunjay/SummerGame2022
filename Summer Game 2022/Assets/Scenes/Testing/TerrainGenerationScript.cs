using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerationScript : MonoBehaviour
{
    [Header("World Settings")]
    public int worldSize = 320;
    public int chunkSize = 16;
    public float noiseFreq = 0.03f;
    private float seed;
    public Texture2D noiseTexture;

    [Header("Terrain and Cave Settings")]
    public float HeightMultiplier = 50f; //Steepness of terrain
    public float HeightAddition = 200; //Overall Height of terrain
    public float caveFreq = 0.05f; //Freq of caves (higher = more caves) (Used when making perlin texture; If higher, the noise in that area is more so more likely to make a cave)
    public float TerrainFreq = 0.02f; //Freq of terrain changes (greater = changes more often)
    public float CaveChance = 0.4f; //How dark/light a spot needs to be to become a cave (derived from perlin noise texture) (More = more caves)

    [Header("Dirt, Grass, and Stone Settings")]
    public int dirtlayerDepth = 5;
    public int BedRockLayerHeight = 5;
    public int DirtandStoneSeperationDistance = 5;
    public Sprite grass;
    public Sprite dirt;
    public Sprite stone;
    public Sprite bedRock;

    private GameObject[] worldChunks;
    private List<Vector2> worldTiles = new List<Vector2>();

    private void Start()
    {
        seed = Random.Range(-10000, 10000);
        GenerateNoiseTexture();
        GenerateChunks();
        GenerateTerrain();
       
    }

    public void GenerateChunks()
    {
        int numOfChunks = worldSize / chunkSize;
        worldChunks = new GameObject[numOfChunks];
        for (int i = 0; i < numOfChunks; i++)
        {
            GameObject newChunk = new GameObject();
            newChunk.name = i.ToString();
            newChunk.transform.parent = this.transform;
            worldChunks[i] = newChunk;
        }
    }

    public void GenerateTerrain()
    {
        for (int x = 0; x < worldSize; x++)
        {
            float height = Mathf.PerlinNoise((x + seed) * TerrainFreq, seed * TerrainFreq) * HeightMultiplier + HeightAddition;
            for (int y = 0; y < height; y++)
            {
                Sprite tileSprite;
                if (y >= height-2) //Generates grass
                {
                    tileSprite = grass;
                    PlaceTile(tileSprite, x, y);
                }
                else if (y < BedRockLayerHeight + Random.Range(-1,2)) //Generates Bedrock
                {
                    tileSprite = bedRock;
                    PlaceTile(tileSprite, x, y);
                }
                else
                {
                    if (y < height - dirtlayerDepth)
                    {
                        tileSprite = stone;
                    }
                    else
                    {
                        tileSprite = dirt;
                    }
                    if (noiseTexture.GetPixel(x, y).r > CaveChance)
                    {
                        PlaceTile(tileSprite, x, y);
                    }
                    else
                    {
                        if (y > height - dirtlayerDepth) //Generates dirt
                        {
                            tileSprite = dirt;
                            PlaceTile(tileSprite, x, y);
                        }
                        if (y < height - dirtlayerDepth && y > height - dirtlayerDepth - DirtandStoneSeperationDistance ) //Ensures there is always stone between dirt and caves
                        {
                            tileSprite = stone;
                            PlaceTile(tileSprite, x, y);
                        }
                    }
                }
                    
            }
        }
    }

    public void GenerateNoiseTexture()
    {
        noiseTexture = new Texture2D(worldSize, worldSize);

        for (int x = 0; x < noiseTexture.width; x++)
        {
            for(int y = 0; y < noiseTexture.height; y++)
            {
                float v = Mathf.PerlinNoise((x + seed)  * caveFreq, (y + seed) * caveFreq);
                noiseTexture.SetPixel(x, y, new Color(v,v,v));
            }
        }

        noiseTexture.Apply();
    }

    public void PlaceTile(Sprite tileSprite,  int x, int y)
    {
        GameObject newTile = new GameObject();


        float chunkCoord = (Mathf.Round(x / chunkSize) * chunkSize)/chunkSize;
        newTile.transform.parent = worldChunks[(int)chunkCoord].transform;

        newTile.AddComponent<SpriteRenderer>();
        newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
        newTile.name = tileSprite.name;
        newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);
        newTile.AddComponent<BoxCollider2D>();

        worldTiles.Add(newTile.transform.position - (Vector3.one * 0.5f));
    }
}

