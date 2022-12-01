using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float contactDistance = 3f;

    // Update is called once per frame
    void Update()
    {
        GameObject Player = GameObject.Find("Player");
        
        if ( Vector2.Distance(transform.position, Player.transform.position) < contactDistance )
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, moveSpeed * Time.deltaTime);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
