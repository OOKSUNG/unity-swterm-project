using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    Rigidbody2D rigid;
    public GameObject Pos;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GameObject Player = GameObject.Find("Player");
        //transform.position = PlayerPos.transform.position;

        rigid.position = Player.transform.position;
        Invoke("destroy", 1);
    }

    private void destroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}