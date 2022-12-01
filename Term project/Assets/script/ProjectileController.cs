using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.magnitude > 10000.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 Direction, float Speed)
    {
        rigid.AddForce(Direction * Speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
