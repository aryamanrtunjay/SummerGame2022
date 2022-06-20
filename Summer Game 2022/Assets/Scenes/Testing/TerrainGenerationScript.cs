using System.Collections;
using UnityEngine;

public class TerrainGenerationScript : MonoBehaviour
{
    public int worldSize = 100;
    public float noiseFreq = 0.05f;
    private float seed;
    public Texture2D noiseTexture;
    public float HeightMultiplier = 40f;
    public float HeightAddition = 25;
    public float caveFreq = 0.05f;
    public float TerrainFreq = 0.05f;
    public float CaveChance = 0.2f;
    public int dirtlayerDepth = 5;
    public Sprite grass;
    public Sprite dirt;
    public Sprite stone;

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
