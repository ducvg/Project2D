using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class MapGeneration2 : MonoBehaviour
{
    public int mapSize;
    public float scale;
    public TileSet[] OrderedTileSet;
    public Tilemap groundMap;
    public Tilemap foregroundMap;

    void Start()
    {
        GenerateNoiseMap();
    }

    [ContextMenu("generate")]
    public void GenerateNoiseMap()
    {
        groundMap.ClearAllTiles();
        foregroundMap.ClearAllTiles();
        if (scale <= 0)
        {
            scale = 0.00001f;
        }
        //for(int x = 0; x < mapSize; x++)
        //{
        //    for(int y = 0; y < mapSize; y++)
        //    {
        //        float perlinValue = Mathf.PerlinNoise(x/scale, y/scale);
        //        perlinValue = Mathf.Clamp(perlinValue, 0f, 1f);
        //        perlinValue = perlinValue * OrderedTileSet.Length;
        //        if (perlinValue == OrderedTileSet.Length) perlinValue--;
        //        int tileIndex = Mathf.FloorToInt(perlinValue);
        //        if (OrderedTileSet[tileIndex].isGround)
        //        {
        //            if (OrderedTileSet[Til])
                    //groundMap.SetTile(new Vector3Int(x - mapSize / 2, y - mapSize / 2, 0), OrderedTileSet[tileIndex].tile);
        //        }
        //        else
        //        {
        //            foregroundMap.SetTile(new Vector3Int(x - mapSize / 2, y - mapSize / 2, 0), OrderedTileSet[tileIndex].tile);
        //        }
        //    }
        //}
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                float perlinValue = Mathf.PerlinNoise(x / scale, y / scale);
                if (perlinValue < 0.2)
                {
                    PaintSingleTile(foregroundMap, new Vector3Int(x - mapSize / 2, y - mapSize / 2, 0), OrderedTileSet[0].tile);
                }
                else if (perlinValue < 0.6)
                {
                    groundMap.SetTile(new Vector3Int(x - mapSize / 2, y - mapSize / 2, 0), OrderedTileSet[1].tile);
                }
                else
                {
                    groundMap.SetTile(new Vector3Int(x - mapSize / 2, y - mapSize / 2, 0), OrderedTileSet[2].tile);
                }
            }
        }
    }
    private void PaintSingleTile(Tilemap tilemap, Vector3Int position, TileBase tile)
    {
        tilemap.SetTile(position, tile);
        tilemap.SetTile(new Vector3Int(position.x + 1, position.y, 0), tile);
        tilemap.SetTile(new Vector3Int(position.x, position.y + 1, 0), tile);
        tilemap.SetTile(new Vector3Int(position.x + 1, position.y + 1, 0), tile);
    }
}

[Serializable]
public class TileSet
{
    public TileBase tile;
    public bool isGround;
    public bool isPoor;
}
