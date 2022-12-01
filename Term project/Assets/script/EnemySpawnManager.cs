using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemySpawnManager : MonoBehaviour
{
    public GameObject[] Enemy;

    public BoxCollider2D area;         //BoxCollicer2D�� ����� �������� ���� ����
    //public List<GameObject> EnemyList = new List<GameObject>();        //������ �� ������Ʈ ����Ʈ

    // Start is called before the first frame update
    void Start()
    {
        area = GetComponent<BoxCollider2D>();
    }

    public void Spawn(int i)
    {
         Vector3 spawnPos = GetRandomPosition(); //���� ��ġ return
        //����, ��ġ, ȸ������ �Ű������� �޾� ������Ʈ ����
        GameObject instance = Instantiate(Enemy[i], spawnPos, Quaternion.identity);
        //EnemyList.Add(instance);   // ������Ʈ ������ ���� ����Ʈ�� add
    }

    //BoxCollider2D ���� ������ ��ġ�� return
    private Vector2 GetRandomPosition()
    {

        Vector2 basePosition = transform.position; //������Ʈ�� ��ġ
        Vector2 size = area.size;                  //box colider2D, �� ���� ũ�� ����

        //x, y�� ���� ��ǥ ���
        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);

        Vector2 spawnPos = new Vector2(posX, posY);
        return spawnPos;        
    }
}
