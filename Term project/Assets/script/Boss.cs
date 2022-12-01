using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]


public class Boss : MonoBehaviour
{
    float timeSpan;  //경과 시간을 갖는 변수
    float checkTime = 3;  // 특정 시간을 갖는 변수

    public float pursuitSpeed;
    
    public bool followPlayer;

    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;

    [SerializeField]
    public int E_maxhealth;

    int health;
    bool isDie = false;

    public GameObject player;

    //탄막
    [SerializeField]
    private GameObject[] firePrefab;

    public int fireSpeed;

    //코인
    [SerializeField]
    private GameObject[] doorPrefab;


    [SerializeField] float contactDistance;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        health = E_maxhealth;

        gameObject.transform.position = new Vector3(110, 110, 0); 
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

        if (dir < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (Vector2.Distance(transform.position, Player.transform.position) < contactDistance)
        {
            timeSpan += Time.deltaTime;  // 경과 시간을 계속 등록
        
            if (timeSpan > checkTime)  // 경과 시간이 특정 시간이 보다 커졋을 경우
            {
                FireAttack();
                timeSpan = 0;
            }
            
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, pursuitSpeed * Time.deltaTime);
        }
            

        
     
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            health--;
            Debug.Log("Enemy Hit");
            OnDamaged(collision.transform.position);
        }

        if (collision.gameObject.tag == "Bolt")
        {
            health--;
            Debug.Log("Enemy Hit");
            OnDamaged(collision.transform.position);
        }


        void OnDamaged(Vector2 targetPos)
        {
            spriteRenderer.color = new Color(1, 0, 0, 1);
            gameObject.layer = 12;

            Invoke("OffDamaged", 1);
        }
    }

    void OffDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        gameObject.layer = 8;
    }


    void Die()
    {
        isDie = true;
        Debug.Log("Enemy Die");
        anim.Play("Enmy_Die");
        gameObject.layer = 12;
        Invoke("Destroy", 1);
    }

    void Destroy()
    {
        Destroy(gameObject);

        GameObject coin = Instantiate(doorPrefab[0]);

        coin.transform.position = gameObject.transform.position;

    }

    void FireAttack()
    {
        Fire(0, -Mathf.Cos(72), Mathf.Sin(72), 18);
        Fire(0, 0, 1, 90);
        Fire(0, Mathf.Cos(72), Mathf.Sin(72), 162);
        Fire(0, -Mathf.Cos(18), Mathf.Sin(18), 234);
        Fire(0, Mathf.Cos(18), Mathf.Sin(18), 306);
    }



    void Fire(int i, float j, float k, float n)
    {

        Vector2 Dir = new Vector2(j, k);
        GameObject bullet = Instantiate(firePrefab[i]);
        bullet.transform.position = gameObject.transform.position;
        bullet.transform.rotation = Quaternion.AngleAxis(n, Vector3.forward);
        bullet.gameObject.GetComponent<Rigidbody2D>().AddForce(Dir * fireSpeed, ForceMode2D.Impulse);
    }
}
