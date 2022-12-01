using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    public GameObject[] itemPrefab;
    public BoxCollider2D maparea;

    public CaveGeneratorByCellularAutomata cave;



    // Start is called before the first frame update
    void Start()
    {
        maparea = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector2 GetRandomPosition()
    {
        Vector2 basePosition = transform.position; //������Ʈ�� ��ġ
        Vector2 size = maparea.size;                  //box colider2D, �� ���� ũ�� ����

        //x, y�� ���� ��ǥ ���
        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);

        
        Vector2 spawnPos = new Vector2(posX, posY);
        return spawnPos;
    }

    public void itemSpawn()
    {
        Vector3 spawnPos = GetRandomPosition(); //���� ��ġ return

        int i = Random.Range(0, 2);

        //����, ��ġ, ȸ������ �Ű������� �޾� ������Ʈ ����
        GameObject instance = Instantiate(itemPrefab[i], spawnPos, Quaternion.identity);  
    }
}
