using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CaveGeneratorByCellularAutomata : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    int CentralX = 0;
    int CentralY = 0;

    private string seed;

    [Range(0, 100)]
    [SerializeField] private int randomFillPercent;
    [SerializeField] private int smoothNum;

    public int[,] map;      //������ �迭
    private const int ROAD = 0;
    private const int WALL = 1;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile[] tile;
    [SerializeField] private Color[] colors;

    public Button regen;
    public GameObject prefab;
    public List<GameObject> WallList = new List<GameObject>();

    public void GenerateMap()
    {
        map = new int[width, height];
        MapRandomFill();

        for (int i = 0; i < smoothNum; i++) //�ݺ��� �������� ������ ������ �Ų���������.
            SmoothMap();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 k = new Vector2((float)x -(float)width/2+0.5f, (float)y - (float)height/2+0.5f);
                if (map[x, y] == WALL)
                {
                    GameObject wall = Instantiate(prefab, k, Quaternion.identity);
                    WallList.Add(wall);
                    OnDrawTile(x, y);
                }//Ÿ�� ����
                else if (map[x, y] == ROAD) OnDrawFloor(x, y);
            }
        }
    }
    public void BossMappGenerator()
    {
        for (int i = 0; i < 42; i++)
        {//�������� ��Ÿ���� �����.
            Vector2 XY1 = new Vector2(79 + i + 0.5f, 79 + 0.5f);
            Vector2 XY2 = new Vector2(79 + i + 0.5f, 120 + 0.5f);
            Vector2 XY3 = new Vector2(79 + 0.5f, 79 + i + 0.5f);
            Vector2 XY4 = new Vector2(120 + 0.5f, 79 + i + 0.5f);
            Vector2 XY5 = new Vector2(79 + 0.5f, 79 + 0.5f);
            Instantiate(prefab, XY1, Quaternion.identity);
            Instantiate(prefab, XY2, Quaternion.identity);
            Instantiate(prefab, XY3, Quaternion.identity);
            Instantiate(prefab, XY4, Quaternion.identity);
            Instantiate(prefab, XY5, Quaternion.identity);
        }   
    }
    private void MapRandomFill() //���� ������ ���� �� Ȥ�� �� �������� �����ϰ� ä��� �޼ҵ�
    {
        seed = Time.time.ToString();//�õ�

        System.Random pseudoRandom = new System.Random(seed.GetHashCode()); //�õ�� ���� �ǻ� ���� ����

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1) map[x, y] = WALL; //�����ڸ��� ������ ���
                else if (x == CentralX || y == CentralY) map[x, y] = ROAD;
                else map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? WALL : ROAD; //������ ���� �� Ȥ�� �� ���� ����
                //Ÿ�� ����
            }
        }
    }
    private void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                if (neighbourWallTiles > 4) map[x, y] = WALL; //�ֺ� ĭ �� ���� 4ĭ�� �ʰ��� ��� ���� Ÿ���� ������ �ٲ�
                else if (neighbourWallTiles < 4) map[x, y] = ROAD; //�ֺ� ĭ �� ���� 4ĭ �̸��� ��� ���� Ÿ���� �� �������� �ٲ�
                //SetTileColor(x, y); //Ÿ�� ���� ����
            }
        }
    }
    private int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        { //���� ��ǥ�� �������� �ֺ� 8ĭ �˻�
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                { //�� ������ �ʰ����� �ʰ� ���ǹ����� �˻�
                    if (neighbourX != gridX || neighbourY != gridY) wallCount += map[neighbourX, neighbourY]; //���� 1�̰� �� ������ 0�̹Ƿ� ���� ��� wallCount ����
                }
                else wallCount++; //�ֺ� Ÿ���� �� ������ ��� ��� wallCount ����
            }
        }
        return wallCount;
    }
    private void OnDrawTile(int x, int y)
    {
        Vector3Int pos = new Vector3Int(CentralX -width / 2 + x, CentralY -height / 2 + y, 0);
        tilemap.SetTile(pos, tile[0]);
    }
    private void OnDrawFloor(int x, int y)
    {
        Vector3Int pos = new Vector3Int(CentralX -width / 2 + x,CentralY -height / 2 + y, 0);
        tilemap.SetTile(pos, tile[1]);
    }
}
