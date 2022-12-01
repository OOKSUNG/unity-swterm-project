using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatebolt : MonoBehaviour
{
    public float t = 0f;    //1초 마다 움직일 각도
    public float k = 0f;    //오브젝트가 공전을 시작할 위치
    public float n = 0f;    //오브젝트가 공전하는 방향을 바라보도록 하기 위한 회전할 각도

    void Update()
    {
        
        GameObject Player = GameObject.Find("Player");

        if (transform.position.magnitude > 10000.0f)
        {
            Destroy(gameObject);
        }
        
        t += 90f *Time.deltaTime; // 1초에 90씩 회전한다.
        //Time.deltaTime가 없으면 프레임 마다 90씩 회전한다.
        //오브젝트는 x축에서 코사인의 주기로 y축에서 사인의 주기로 회전한다. 그래서 플레이어를 중심으로 공전하는 것으로 보인다.
        //Mathf.Sin/Cos은 값을 라디안으로 받으므로 Ddg2Rad으로 각도를 라디안으로 바꾸어준다. 따라서 1초에 90도씩 회전하게 된다.
        gameObject.transform.position = new Vector3(Player.transform.position.x + Mathf.Cos(t*Mathf.Deg2Rad + k * Mathf.Deg2Rad) * 2, Player.transform.position.y + Mathf.Sin(t*Mathf.Deg2Rad + k * Mathf.Deg2Rad) * 2,0);
        gameObject.transform.rotation = Quaternion.AngleAxis(t + n, Vector3.forward);//오브젝트가 회전하는 방향을 바라보게 한다.
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
