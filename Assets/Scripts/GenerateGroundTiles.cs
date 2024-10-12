using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateGroundTiles : MonoBehaviour
{
    public Tilemap tiles; // 主要地形的Tilemap
    public Tilemap otherThings; // 用于放置装饰物的Tilemap
    public TileBase dirt_up; // 地表土块
    public TileBase dirt_in; // 地下土块
    public TileBase terrainProtrusion; // 地形突起物

    public Transform player; // 玩家的Transform组件
    public int chunkSize = 50; // 每个地形块的宽度
    public int generationDistance = 20; // 触发新地形生成的距离
    public int maxTerrainHeight = 15; // 最大地形高度
    public int initialTerrainDistance = 10; // 玩家初始位置两侧生成地形的距离

    private HashSet<Vector3Int> generatedChunks = new HashSet<Vector3Int>(); // 已生成的地形块集合
    private int lastGroundHeight = 0; // 记录最后生成的地形高度
    private int lastGeneratedChunkX = 0; // 记录最后生成的chunk的X坐标

    void Start()
    {
        GenerateInitialGround();
    }

    void Update()
    {
        int playerChunkX = Mathf.FloorToInt(player.position.x / chunkSize);
        int playerPositionInChunk = Mathf.FloorToInt(player.position.x) % chunkSize;

        // 检查玩家是否接近chunk边缘
        if (playerPositionInChunk > chunkSize - generationDistance)
        {
            // 生成前方的chunk
            GenerateChunk(playerChunkX + 1);
        }
        else if (playerPositionInChunk < generationDistance)
        {
            // 生成后方的chunk
            GenerateChunk(playerChunkX - 1);
        }

        // 始终确保玩家所在的chunk被生成
        GenerateChunk(playerChunkX);
    }

    void GenerateInitialGround()
    {
        int initialGroundX = Mathf.FloorToInt(player.position.x);
        int initialGroundY = Mathf.FloorToInt(player.position.y);

        // 设置初始地面高度
        lastGroundHeight = initialGroundY;

        // 生成玩家两侧指定距离的地形
        for (int x = initialGroundX - initialTerrainDistance; x <= initialGroundX + initialTerrainDistance; x++)
        {
            lastGroundHeight = GetNextGroundHeight(x, lastGroundHeight);
            GenerateGroundTile(x, lastGroundHeight);
        }

        lastGeneratedChunkX = Mathf.FloorToInt(initialGroundX / chunkSize);
    }

    void GenerateChunk(int chunkX)
    {
        if (generatedChunks.Contains(new Vector3Int(chunkX, 0, 0)))
            return;

        generatedChunks.Add(new Vector3Int(chunkX, 0, 0));

        int startX = chunkX * chunkSize;
        int endX = (chunkX + 1) * chunkSize;

        for (int x = startX; x < endX; x++)
        {
            lastGroundHeight = GetNextGroundHeight(x, lastGroundHeight);
            GenerateGroundTile(x, lastGroundHeight);
        }

        lastGeneratedChunkX = chunkX;
    }

    int GetNextGroundHeight(int x, int currentHeight)
    {
        // 使用柏林噪声来生成更自然的地形变化，同时限制最大高度
        float perlinValue = Mathf.PerlinNoise(x * 0.05f, 0) * 10;
        int newHeight = Mathf.RoundToInt(currentHeight + perlinValue - 5);
        return Mathf.Clamp(newHeight, 5, maxTerrainHeight);
    }

    void GenerateGroundTile(int x, int groundHeight)
    {
        for (int y = 0; y <= groundHeight; y++)
        {
            TileBase tile = (y == groundHeight) ? dirt_up : dirt_in;
            tiles.SetTile(new Vector3Int(x, y, 0), tile);
        }

        // 在地面上方生成突起物，而不是在地形内部
        if (Random.Range(0, 5) == 0)
        {
            otherThings.SetTile(new Vector3Int(x, groundHeight + 1, 0), terrainProtrusion);
        }
    }
}