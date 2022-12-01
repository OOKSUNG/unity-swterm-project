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
        Vector2 basePosition = transform.position; //오브젝트의 위치
        Vector2 size = maparea.size;                  //box colider2D, 즉 맵의 크기 벡터

        //x, y축 랜덤 좌표 얻기
        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);

        
        Vector2 spawnPos = new Vector2(posX, posY);
        return spawnPos;
    }

    public void itemSpawn()
    {
        Vector3 spawnPos = GetRandomPosition(); //랜덤 위치 return

        int i = Random.Range(0, 2);

        //원본, 위치, 회전값을 매개변수로 받아 오브젝트 복제
        GameObject instance = Instantiate(itemPrefab[i], spawnPos, Quaternion.identity);  
    }
}
