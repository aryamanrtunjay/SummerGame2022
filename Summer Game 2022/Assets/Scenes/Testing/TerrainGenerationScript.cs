using System.Collections;
using UnityEngine;

public class TerrainGenerationScript : MonoBehaviour
{
    public int worldSize = 200;
    public float noiseFreq = 0.03f;
    private float seed;
    public Texture2D noiseTexture;

    public float HeightMultiplier = 50f; //Steepness of terrain
    public float HeightAddition = 200; //Overall Height of terrain
    public float caveFreq = 0.05f; //Freq of caves (higher = more caves) (Used when making perlin texture; If higher, the noise in that area is more so more likely to make a cave)
    public float TerrainFreq = 0.02f; //Freq of terrain changes (greater = changes more often)
    public float CaveChance = 0.4f; //How dark/light a spot needs to be to become a cave (derived from perlin noise texture) (More = more caves)

    public int dirtlayerDepth = 7;
    public int BedRockLayerHeight = 5;
    public int DirtandStoneSeperationDistance = 5;
    public Sprite grass;
    public Sprite dirt;
    public Sprite stone;
    public Sprite bedRock;

    private void Start()
    {
        seed = Random.Range(-10000, 10000);
        GenerateNoiseTexture();
        GenerateTerrain();
       
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

    public void PlaceTile(Sprite tileSprite, float x, float y)
    {
        GameObject newTile = new GameObject();
        newTile.transform.parent = this.transform;
        newTile.AddComponent<SpriteRenderer>();
        newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
        newTile.name = tileSprite.name;
        newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);
        newTile.AddComponent<BoxCollider2D>();
    }
}

