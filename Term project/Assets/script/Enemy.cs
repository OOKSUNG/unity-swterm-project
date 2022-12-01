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

    //����
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

        //�̵����⿡ ���� ������Ʈ�� �¿� �����Ѵ�.
        if (dir < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        //�÷��̾�� ���� Ư�� �Ÿ� ���ϰ� �Ǹ� �÷��̾ �߰��ϱ� �����Ѵ�.
        if (Vector2.Distance(transform.position, Player.transform.position) < contactDistance)
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, pursuitSpeed * Time.deltaTime);


    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Bullet �±��� ������Ʈ�� �浹�ϸ� ü���� �����Ѵ�.
        if (collision.gameObject.tag == "Bullet")
        {
            health--;
            Debug.Log("Enemy Hit");
            OnDamaged(collision.transform.position);
        }
        //Bolt �±��� ������Ʈ�� �浹�ϸ� ü���� �����Ѵ�.
        if (collision.gameObject.tag == "Bolt")
        {
            health--;
            Debug.Log("Enemy Hit");
            OnDamaged(collision.transform.position);
        }

        //����������
        void OnDamaged(Vector2 targetPos)
        {
            spriteRenderer.color = new Color(1, 0, 0, 1);   //������Ʈ�� ������ ������ �ٲ۴�.
            gameObject.layer = 12;  //���̾ �̵��Ͽ� �÷��̾�� ������ �浹���� �ʰ��Ѵ�.
            Invoke("OffDamaged", 1);    //1���� ������ ���¿��� ����������.
        }
    }


    void OffDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);   //������Ʈ�� ���� �������
        gameObject.layer = 8;   //���̾ ������
    }

    //���� ����
    void Die()
    {
        isDie = true;
        Debug.Log("Enemy Die");
        anim.Play("Enmy_Die");
        gameObject.layer = 12;
        Invoke("Destroy", 1);   //1���� ������Ʈ �ı�
    }

    void Destroy()
    {
        Destroy(gameObject);
        GameObject coin = Instantiate(coinPrefab[0]);   //�ı��� ���ÿ� ���� �������� �����Ѵ�.
        coin.transform.position = gameObject.transform.position;
    }
}
