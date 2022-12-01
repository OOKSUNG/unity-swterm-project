using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class Enemy : MonoBehaviour
{
    public float pursuitSpeed;

    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;

    [SerializeField]
    public int E_maxhealth;

    int health;
    bool isDie = false;

    //코인
    [SerializeField]
    private GameObject[] coinPrefab;

    [SerializeField] float contactDistance;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        health = E_maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            if (!isDie)
                Die();

            return;
        }
        
        GameObject Player = GameObject.Find("Player");


        float dir = Player.transform.position.x - transform.position.x;

        //이동방향에 따라 오브젝트를 좌우 반전한다.
        if (dir < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        //플레이어와 적이 특정 거리 이하가 되면 플레이어를 추격하기 시작한다.
        if (Vector2.Distance(transform.position, Player.transform.position) < contactDistance)
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, pursuitSpeed * Time.deltaTime);


    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Bullet 태그의 오브젝트와 충돌하면 체력이 감소한다.
        if (collision.gameObject.tag == "Bullet")
        {
            health--;
            Debug.Log("Enemy Hit");
            OnDamaged(collision.transform.position);
        }
        //Bolt 태그의 오브젝트와 충돌하면 체력이 감소한다.
        if (collision.gameObject.tag == "Bolt")
        {
            health--;
            Debug.Log("Enemy Hit");
            OnDamaged(collision.transform.position);
        }

        //데미지상태
        void OnDamaged(Vector2 targetPos)
        {
            spriteRenderer.color = new Color(1, 0, 0, 1);   //오브젝트의 색깔을 빨같게 바꾼다.
            gameObject.layer = 12;  //레이어를 이동하여 플레이어와 마법에 충돌하지 않게한다.
            Invoke("OffDamaged", 1);    //1초후 데미지 상태에서 빠져나간다.
        }
    }


    void OffDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);   //오브젝트의 색을 원래대로
        gameObject.layer = 8;   //레이어도 원래로
    }

    //죽음 상태
    void Die()
    {
        isDie = true;
        Debug.Log("Enemy Die");
        anim.Play("Enmy_Die");
        gameObject.layer = 12;
        Invoke("Destroy", 1);   //1초후 오브젝트 파괴
    }

    void Destroy()
    {
        Destroy(gameObject);
        GameObject coin = Instantiate(coinPrefab[0]);   //파괴와 동시에 코인 프리팹을 생성한다.
        coin.transform.position = gameObject.transform.position;
    }
}
