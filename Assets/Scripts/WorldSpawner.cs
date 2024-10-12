using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldSpawner : MonoBehaviour
{
    public Tilemap tiles; // 地面
    public Tilemap otherThings; // 长草等其他东西
    public TileBase dirt_up, dirt_in, grass; // 地表的土块, 地下的土块, 草

    int[,] tileType;
    int[] groundPos;
    public int worldHeight = 100, worldWidth = 100;

    void Start()
    {
        InitMapTilesInfo(); // 初始化地形数据
        InitData(); // 根据地形数据生成Tile
    }

    void InitMapTilesInfo()
    {
        tileType = new int[worldWidth, worldHeight];
        groundPos = new int[worldWidth];

        int groundHeight = 10;

        // 初步生成地图
        for (int i = 0; i < worldWidth; i++)
        {
            groundHeight = Mathf.Max(groundHeight + Random.Range(-3, 3), 5);
            groundPos[i] = groundHeight + 1;
            tileType[i, groundHeight] = 1;

            for (int j = 0; j < groundHeight; j++)
            {
                tileType[i, j] = 2;
            }
        }

        // 美化生成的地图
        for (int i = 1; i < worldWidth - 1; i++)
        {
            if (tileType[i - 1, groundPos[i - 1] - 1] == 1 && tileType[i + 1, groundPos[i + 1] - 1] == 1 && tileType[i, groundPos[i] - 1] == 0)
            {
                tileType[i, groundPos[i - 1] - 1] = 1;
                groundPos[i] = groundPos[i - 1];
            }
        }

        // 长草
        for (int i = 0; i < worldWidth; i++)
        {
            if (Random.Range(0, 5) == 1)
                tileType[i, groundPos[i]] = 3;
        }
    }

    void InitData()
    {
        for (int i = 0; i < worldWidth; i++)
        {
            for (int j = 0; j < worldHeight; j++)
            {
                switch (tileType[i, j])
                {
                    case 1:
                        tiles.SetTile(new Vector3Int(i, j, 0), dirt_up);
                        break;
                    case 2:
                        tiles.SetTile(new Vector3Int(i, j, 0), dirt_in);
                        break;
                    case 3:
                        otherThings.SetTile(new Vector3Int(i, j, 0), grass);
                        break;
                }
            }
        }
    }
}
