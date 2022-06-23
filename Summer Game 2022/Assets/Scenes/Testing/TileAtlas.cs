using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TileAtlas", menuName = "Tile Atlas")]
public class TileAtlas : ScriptableObject
{
    public TileClass grass;
    public TileClass dirt;
    public TileClass stone;
    public TileClass bedRock;
    public TileClass log;
    public TileClass leaf;
}
