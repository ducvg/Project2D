using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private Tilemap GroundMap;
    [SerializeField] private Tilemap ForegroundMap;
    [SerializeField] private TileBase[] grassTiles;
    [SerializeField] private TileBase[] bigRockTiles;
    [SerializeField] private TileBase[] smallRockTiles;
    [SerializeField] private TileBase[] pepples;
    [SerializeField] private TileBase dirtTile;
    [SerializeField] private TileBase waterTile;
    [SerializeField] private int mapSize = 50;
    [SerializeField] private int dirtArea = 30;
    [SerializeField] private int dirtDensity = 10;
    [SerializeField] private int dirtLength = 10;
    [SerializeField] private int lakeArea = 10;
    [SerializeField] private int lakeWidthMax = 8;
    [SerializeField] private int lakeHeightMax = 8;
    private HashSet<Vector3Int> groundPositions = new();
    private HashSet<Vector3Int> lakePositions = new();

    // Start is called before the first frame update
    [ContextMenu("generate")]
    void Start()
    {
        GroundMap.ClearAllTiles();
        ForegroundMap.ClearAllTiles();
        groundPositions.Clear();
        lakePositions.Clear();
        FillGround();

        for (int i = 1; i <= dirtArea; i++)
        {
            HashSet<Vector3Int> tempGround = RandomDirt();
            groundPositions.UnionWith(tempGround);
            PaintTiles(groundPositions, GroundMap, dirtTile);
        }

        for (int i = 1; i <= lakeArea; i++)
        {
            HashSet<Vector3Int> tempLake = RandomLake();
            lakePositions.UnionWith(tempLake);
            foreach (Vector3Int position in lakePositions)
            {
                ForegroundMap.SetTile(position, waterTile);
                GroundMap.SetTile(position, null);
            }
        }
        StartCoroutine(LateStart(1f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        AstarPath.active.Scan();
    }

    private void FillGround()
    {
        for (int i = -mapSize; i < mapSize; i++)
        {
            for (int j = -mapSize; j < mapSize; j++)
            {
                TileBase selectedTile = grassTiles[Random.Range(0, grassTiles.Length)];
                GroundMap.SetTile(new Vector3Int(i, j, 0), selectedTile);
            }
        }
    }

    private HashSet<Vector3Int> RandomDirt()
    {
        Vector3Int currentPos;
        currentPos = new Vector3Int(Random.Range(-mapSize + dirtLength / 2, mapSize - dirtLength / 2), Random.Range(-mapSize + dirtLength / 2, mapSize - dirtLength / 2), 0);
        HashSet<Vector3Int> floorPos = new();
        for (int i = 0; i < dirtDensity; i++)
        {
            var path = SimpleRandomWalk(currentPos, dirtLength);
            floorPos.UnionWith(path);
            currentPos = floorPos.ElementAt(Random.Range(0, floorPos.Count));
        }
        return floorPos;
    }

    private HashSet<Vector3Int> RandomLake()
    {
        int lakeWidth = Random.Range(3, lakeWidthMax);
        int lakeHeight = Random.Range(3, lakeHeightMax);

        Vector3Int lakePosition;
        HashSet<Vector3Int> lakePositions = new();

        do
        {
            lakePosition = new Vector3Int(Random.Range(-mapSize + lakeWidth / 2, mapSize - lakeWidth / 2), Random.Range(-mapSize + lakeHeight / 2, mapSize - lakeHeight / 2), 0);
        } while ((lakePosition.x <= lakeWidthMax && lakePosition.x >= -lakeWidthMax) && (lakePosition.y <= lakeHeightMax && lakePosition.y >= -lakeHeightMax));

        // Define the bounding rectangle
        int minX = lakePosition.x - 1;
        int maxX = lakePosition.x + lakeWidth + 1;
        int minY = lakePosition.y - 1;
        int maxY = lakePosition.y + lakeHeight + 1;

        // Check if any ground tile is within the bounding rectangle
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Vector3Int checkPos = new Vector3Int(x, y, 0);
                if (groundPositions.Contains(checkPos))
                {
                    return RandomLake();
                }
            }
        }

        for (int x = lakePosition.x; x < lakePosition.x + lakeWidth; x++)
        {
            for (int y = lakePosition.y; y < lakePosition.y + lakeHeight; y++)
            {
                //lakePositions.Add(new Vector3Int(x, y, 0));
                lakePositions.Add(new Vector3Int(x, y, 0));
            }
        }

        return lakePositions;
    }

    private static HashSet<Vector3Int> SimpleRandomWalk(Vector3Int startPos, int walkLength)
    {
        HashSet<Vector3Int> path = new();
        path.Add(startPos);
        var previousPos = startPos;

        for (int i = 0; i < walkLength; i++)
        {
            var newPos = previousPos + Direction2D.GetRandomCardinalDirection();
            path.Add(newPos);
            previousPos = newPos;
        }
        return path;
    }

    private void PaintTiles(IEnumerable<Vector3Int> pos, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in pos)
        {
            PaintSingleTile(tilemap, position, tile);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, Vector3Int position, TileBase tile)
    {
        tilemap.SetTile(position, tile);
        tilemap.SetTile(new Vector3Int(position.x + 1, position.y, 0), tile);
        tilemap.SetTile(new Vector3Int(position.x, position.y + 1, 0), tile);
        tilemap.SetTile(new Vector3Int(position.x + 1, position.y + 1, 0), tile);

        //RockState newstate = new()
        //{
        //    isFloorTile = tile == floorTile
        //};
        //loadedTile[tilePos] = newstate;
    }
}

public static class Direction2D
{
    public static List<Vector3Int> cardinalDirectionsList = new(){
            new Vector3Int(0,1,0), //UP
            new Vector3Int(1,0,0), //RiGHT
            new Vector3Int(0,-1,0),//DOWN
            new Vector3Int(-1,0,0) //LEFT
        };

    public static Vector3Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}
