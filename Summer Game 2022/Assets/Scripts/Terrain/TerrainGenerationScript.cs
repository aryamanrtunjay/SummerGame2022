using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TerrainGenerationScript : MonoBehaviour
{

    private float LightingTickCounter = 0;

    public Location location;
    public Camera Camera;
    public SpawnPoint SpawnPoint;


    private float seconds = 0;
    private List<int> ActiveChunks = new List<int>();


    [Header("World Settings (world size must be divisible by chunk size)")]
    public int worldSize = 320;
    public int chunkSize = 16;
    public float noiseFreq = 0.03f;
    private float seed;

    private GameObject[] worldChunks;

    public GameObject[,] worldTiles;
    public Texture2D caveNoiseTexture; //black = caves

    [Header("Terrain and Cave Settings")]
    public float HeightMultiplier = 50f; //Steepness of terrain
    public float HeightAddition = 200; //Overall Height of terrain
    public float caveFreq = 0.05f; //Freq of caves (higher = more caves) (Used when making perlin texture; If higher, the noise in that area is more so more likely to make a cave)
    public float TerrainFreq = 0.02f; //Freq of terrain changes (greater = changes more often)
    public float CaveChance = 0.4f; //How dark/light a spot needs to be to become a cave (derived from perlin noise texture) (More = more caves)

    [Header("Dirt, Grass, Stone, Tree, and Tall Grass Settings")]
    public int dirtlayerDepth = 5;
    public int BedRockLayerHeight = 5;
    public int DirtandStoneSeperationDistance = 5;
    public int TreeProbability = 10; // probability of tree spawning on a block (1/var = probability)
    public int TallGrassProbability = 5;
    public int MinTreeHeight = 4;
    public int MaxTreeHeight = 7;

    [Header("Ore Settings")]
    public float coalRarity; //Dont change
    public float coalVeinSize; // More equals less common
    public float ironRarity, ironVeinSize;
    public float goldRarity, goldVeinSize;
    public float diamondRarity, diamondVeinSize;
    public Texture2D coalSpread;
    public Texture2D ironSpread;
    public Texture2D goldSpread;
    public Texture2D diamondSpread;


    [Header("Player Vars")]
    public List<float> PlayerPosition = new List<float>();
    int PlayerStartX;
    int PlayerStartY;


    [Header("Tile Atlas")]
    public TileAtlas tileAtlas;

    private void OnValidate()
    {
        if (caveNoiseTexture == null)
        {
            caveNoiseTexture = new Texture2D(worldSize, worldSize);
            coalSpread = new Texture2D(worldSize, worldSize);
            ironSpread = new Texture2D(worldSize, worldSize);
            goldSpread = new Texture2D(worldSize, worldSize);
            diamondSpread = new Texture2D(worldSize, worldSize);
        }
        GenerateNoiseTexture(caveFreq, CaveChance, caveNoiseTexture);
        GenerateNoiseTexture(coalRarity, coalVeinSize, coalSpread);
        GenerateNoiseTexture(ironRarity, ironVeinSize, ironSpread);
        GenerateNoiseTexture(goldRarity, goldVeinSize, goldSpread);
        GenerateNoiseTexture(diamondRarity, diamondVeinSize, diamondSpread);
    }

    private void Start()
    {



        //Generate Terain
        worldTiles = new GameObject[worldSize, worldSize];
        seed = Random.Range(-10000, 10000);
        if (caveNoiseTexture == null)
        {
            caveNoiseTexture = new Texture2D(worldSize, worldSize);
            coalSpread = new Texture2D(worldSize, worldSize);
            ironSpread = new Texture2D(worldSize, worldSize);
            goldSpread = new Texture2D(worldSize, worldSize);
            diamondSpread = new Texture2D(worldSize, worldSize);
        }


        GenerateNoiseTexture(caveFreq, CaveChance, caveNoiseTexture);
        GenerateNoiseTexture(coalRarity, coalVeinSize, coalSpread);
        GenerateNoiseTexture(ironRarity, ironVeinSize, ironSpread);
        GenerateNoiseTexture(goldRarity, goldVeinSize, goldSpread);
        GenerateNoiseTexture(diamondRarity, diamondVeinSize, diamondSpread);

        GenerateChunks();
        GenerateTerrain();



        RefreshChunks();

        PlayerStartX = (int)Mathf.Round(worldSize / 2);
        // PlayerStartY = worldSize;
        for (int i = worldSize - 1; i >= 0; i--)
        {
            if (worldTiles[PlayerStartX - 1, i] != null)
            {
                PlayerStartY = (int)worldTiles[PlayerStartX - 1, i].transform.position.y + 3;
                i = -1;
            }
        }
        SpawnPoint.spawn.x = PlayerStartX;
        SpawnPoint.spawn.y = PlayerStartY;
        PlayerPosition.Add(location.x);
        PlayerPosition.Add(location.y);

    }

    void RefreshChunks()
    {
        for (int i = 0; i < worldChunks.Length; i++)
        {
            if (Vector2.Distance(new Vector2((i * chunkSize) + (chunkSize / 2), 0), new Vector2(location.x, 0)) > Camera.fieldOfView + 15)
            {
                worldChunks[i].SetActive(false);

                ActiveChunks.Remove(i);

            }
            else
            {
                worldChunks[i].SetActive(true);
                ActiveChunks.Add(i);
            }
        }

    }




    void Update()
    {


        //Updating time and player coords
        seconds += Time.deltaTime;
        PlayerPosition[0] = Mathf.Round(location.x - 0.5f);
        PlayerPosition[1] = Mathf.Round(location.y - 0.5f);
        float CurrentPlayerX = PlayerPosition[0];
        float CurrentPlayerY = PlayerPosition[1];

        if (CurrentPlayerX >= 0 && CurrentPlayerX < worldSize && CurrentPlayerY >= 0 && CurrentPlayerY < worldSize)
        {
            //Debug.Log(CheckNextTile("d", (int)CurrentPlayerX, (int)CurrentPlayerY));
        }

        RefreshChunks();
        //CheckTallGrassSurvivability();

        int CurrentTime = (int)(LightingTickCounter);

        // if (CurrentTime % 5 == 0)
        // {
        //     //Debug.Log("Updated Lighting");
        //     UpdateLighting();
        // }

        LightingTickCounter += 1 * Time.deltaTime;


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
                Sprite tileSprite = tileAtlas.stone.tileSprite;//Temp fix
                if (y >= height - 1) //Generates grass
                {
                    tileSprite = tileAtlas.grass.tileSprite;
                    PlaceTile(tileSprite, x, y);
                    int t = Random.Range(0, TreeProbability);
                    if (t == 1)
                    {
                        GenerateTree(x, y + 1);
                    }
                    else
                    {
                        // Generates Tall Grass
                        int g = Random.Range(0, TallGrassProbability);
                        if (g == 1)
                        {
                            tileSprite = tileAtlas.tallgrass.tileSprite;
                            PlaceTile(tileSprite, x, y + 1);
                        }
                    }
                }
                else if (y < BedRockLayerHeight + Random.Range(-1, 2)) //Generates Bedrock
                {
                    tileSprite = tileAtlas.bedRock.tileSprite;
                    PlaceTile(tileSprite, x, y);
                }
                else
                {
                    if (y < height - dirtlayerDepth)
                    {


                        if (ironSpread.GetPixel(x, y).r > 0.5f)
                        {
                            if (y >= height * 0.4 && y <= height * 0.7)
                            {
                                tileSprite = tileAtlas.iron.tileSprite;
                            }
                        }
                        else if (coalSpread.GetPixel(x, y).r > 0.5f)
                        {
                            tileSprite = tileAtlas.coal.tileSprite;
                        }
                        else if (goldSpread.GetPixel(x, y).r > 0.5f)
                        {
                            if (y >= height * 0.225 && y <= height * 0.5)
                            {
                                tileSprite = tileAtlas.gold.tileSprite;
                            }
                        }
                        else if (diamondSpread.GetPixel(x, y).r > 0.5f)
                        {
                            if (y <= height * 0.25)
                            {
                                tileSprite = tileAtlas.diamond.tileSprite;
                            }
                        }
                        else
                        {
                            tileSprite = tileAtlas.stone.tileSprite;
                        }

                    }
                    else
                    {
                        tileSprite = tileAtlas.dirt.tileSprite;
                    }
                    if (caveNoiseTexture.GetPixel(x, y).r > 0.5f)
                    {
                        PlaceTile(tileSprite, x, y);
                    }
                    else
                    {
                        if (y > height - dirtlayerDepth) //Generates dirt
                        {
                            tileSprite = tileAtlas.dirt.tileSprite;
                            PlaceTile(tileSprite, x, y);
                        }
                        if (y < height - dirtlayerDepth && y > height - dirtlayerDepth - DirtandStoneSeperationDistance) //Ensures there is always stone between dirt and caves
                        {
                            if (coalSpread.GetPixel(x, y).r > 0.5f)
                            {
                                tileSprite = tileAtlas.iron.tileSprite;
                            }
                            else
                            {
                                tileSprite = tileAtlas.stone.tileSprite;
                            }
                            PlaceTile(tileSprite, x, y);
                        }
                    }
                }

            }
        }

    }

    public void GenerateNoiseTexture(float frequency, float limit, Texture2D noiseTexture)
    {

        for (int x = 0; x < noiseTexture.width; x++)
        {
            for (int y = 0; y < noiseTexture.height; y++)
            {
                float v = Mathf.PerlinNoise((x + seed) * frequency, (y + seed) * frequency);
                if (v > limit)
                {
                    noiseTexture.SetPixel(x, y, Color.white);
                }
                else
                {
                    noiseTexture.SetPixel(x, y, Color.black);
                }
            }
        }

        noiseTexture.Apply();

    }



    void GenerateTree(int x, int y)
    {
        int TreeHeight = Random.Range(MinTreeHeight, MaxTreeHeight);
        for (int i = 0; i < TreeHeight; i++)
        {
            PlaceTile(tileAtlas.log.tileSprite, x, y + i);
        }

        //Leaves
        PlaceTile(tileAtlas.leaf.tileSprite, x, y + TreeHeight);
        PlaceTile(tileAtlas.leaf.tileSprite, x, y + TreeHeight + 1);
        PlaceTile(tileAtlas.leaf.tileSprite, x, y + TreeHeight + 2);

        PlaceTile(tileAtlas.leaf.tileSprite, x - 1, y + TreeHeight);
        PlaceTile(tileAtlas.leaf.tileSprite, x - 1, y + TreeHeight + 1);

        PlaceTile(tileAtlas.leaf.tileSprite, x + 1, y + TreeHeight);
        PlaceTile(tileAtlas.leaf.tileSprite, x + 1, y + TreeHeight + 1);
    }


    public void PlaceTile(Sprite tileSprite, int x, int y)
    {
        GameObject newTile = new GameObject();

        float chunkCoord = (Mathf.Round(x / chunkSize) * chunkSize) / chunkSize;
        newTile.transform.parent = worldChunks[(int)chunkCoord].transform;

        newTile.AddComponent<SpriteRenderer>();
        newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
        newTile.AddComponent<TileInfo>();

        newTile.name = tileSprite.name;
        newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);

        if (tileSprite == tileAtlas.dirt.tileSprite || tileSprite == tileAtlas.stone.tileSprite || tileSprite == tileAtlas.leaf.tileSprite || tileSprite == tileAtlas.coal.tileSprite || tileSprite == tileAtlas.iron.tileSprite || tileSprite == tileAtlas.gold.tileSprite || tileSprite == tileAtlas.diamond.tileSprite)
        {
            int rotationNum = (Random.Range(0, 4)) * 90;
            newTile.transform.Rotate(new Vector3(0, 0, rotationNum));
        }

        if (tileSprite != tileAtlas.log.tileSprite && tileSprite != tileAtlas.tallgrass.tileSprite && tileSprite != tileAtlas.leaf.tileSprite)
        {
            newTile.AddComponent<BoxCollider2D>();
        }


        worldTiles[x, y] = newTile;
    }

    public GameObject CheckNextTile(string Direction, int CurrentTileX, int CurrentTileY)
    {
        GameObject newCheckedTile = new GameObject();
        if (Direction == "r")
        {
            if (CurrentTileX == worldSize - 1)
            {
                newCheckedTile = worldTiles[CurrentTileX, CurrentTileY];
            }
            else
            {
                newCheckedTile = worldTiles[CurrentTileX + 1, CurrentTileY];
            }
        }
        else if (Direction == "l")
        {
            if (CurrentTileX == 0)
            {
                newCheckedTile = worldTiles[CurrentTileX, CurrentTileY];
            }
            else
            {
                newCheckedTile = worldTiles[CurrentTileX - 1, CurrentTileY];
            }
        }
        else if (Direction == "u")
        {
            if (CurrentTileY == worldSize - 1)
            {
                newCheckedTile = worldTiles[CurrentTileX, CurrentTileY];
            }
            else
            {
                newCheckedTile = worldTiles[CurrentTileX, CurrentTileY + 1];
            }
        }
        else if (Direction == "d")
        {
            if (CurrentTileY == 0)
            {
                newCheckedTile = worldTiles[CurrentTileX, CurrentTileY];
            }
            else
            {
                newCheckedTile = worldTiles[CurrentTileX, CurrentTileY - 1];
            }
        }
        else
        {
            Debug.Log("Invalid Direction : Function CheckNextTile");
        }
        return newCheckedTile;
    }

}

