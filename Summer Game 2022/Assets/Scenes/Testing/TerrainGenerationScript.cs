using System.Collections;
using UnityEngine;

public class TerrainGenerationScript : MonoBehaviour
{
    public int worldSize = 1000;
    public float noiseFreq = 0.03f;
    private float seed;
    public Texture2D noiseTexture;

    public float HeightMultiplier = 50f; //Steepness of terrain
    public float HeightAddition = 200; //Overall Height of terrain
    public float caveFreq = 0.05f; //Freq of caves (higher = more caves) (Used when making perlin texture; If higher, the noise in that area is more so more likely to make a cave)
    public float TerrainFreq = 0.02f; //Freq of terrain changes (greater = changes more often)
    public float CaveChance = 0.4f; //How dark/light a spot needs to be to become a cave (derived from perlin noise texture) (More = more caves)

    public int dirtlayerDepth = 5;
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
                if (y >= height-2)
                {
                    tileSprite = grass;
                    GameObject newTile = new GameObject();
                    newTile.transform.parent = this.transform;
                    newTile.AddComponent<SpriteRenderer>();
                    newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
                    newTile.name = tileSprite.name;
                    newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);
                }
                else if (y < BedRockLayerHeight)
                {
                    tileSprite = bedRock;
                    GameObject newTile = new GameObject();
                    newTile.transform.parent = this.transform;
                    newTile.AddComponent<SpriteRenderer>();
                    newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
                    newTile.name = tileSprite.name;
                    newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);
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
                        GameObject newTile = new GameObject();
                        newTile.transform.parent = this.transform;
                        newTile.AddComponent<SpriteRenderer>();
                        newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
                        newTile.name = tileSprite.name;
                        newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);
                    }
                    else
                    {
                        if (y > height - dirtlayerDepth)
                        {
                            tileSprite = dirt;
                            GameObject newTile = new GameObject();
                            newTile.transform.parent = this.transform;
                            newTile.AddComponent<SpriteRenderer>();
                            newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
                            newTile.name = tileSprite.name;
                            newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);
                        }
                        if (y < height - dirtlayerDepth && y > height - dirtlayerDepth - DirtandStoneSeperationDistance )
                        {
                            tileSprite = stone;
                            GameObject newTile = new GameObject();
                            newTile.transform.parent = this.transform;
                            newTile.AddComponent<SpriteRenderer>();
                            newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
                            newTile.name = tileSprite.name;
                            newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);
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
}
