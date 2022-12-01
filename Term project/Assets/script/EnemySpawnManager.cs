using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemySpawnManager : MonoBehaviour
{
    public GameObject[] Enemy;

    public BoxCollider2D area;         //BoxCollicer2D의 사이즈를 가져오기 위한 변수
    //public List<GameObject> EnemyList = new List<GameObject>();        //생성한 적 오브젝트 리스트

    // Start is called before the first frame update
    void Start()
    {
        area = GetComponent<BoxCollider2D>();
    }

    public void Spawn(int i)
    {
         Vector3 spawnPos = GetRandomPosition(); //랜덤 위치 return
        //원본, 위치, 회전값을 매개변수로 받아 오브젝트 복제
        GameObject instance = Instantiate(Enemy[i], spawnPos, Quaternion.identity);
        //EnemyList.Add(instance);   // 오브젝트 관리를 위해 리스트에 add
    }

    //BoxCollider2D 내의 랜덤한 위치를 return
    private Vector2 GetRandomPosition()
    {

        Vector2 basePosition = transform.position; //오브젝트의 위치
        Vector2 size = area.size;                  //box colider2D, 즉 맵의 크기 벡터

        //x, y축 랜덤 좌표 얻기
        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);

        Vector2 spawnPos = new Vector2(posX, posY);
        return spawnPos;        
    }
}
