using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatebolt : MonoBehaviour
{
    public float t = 0f;    //1�� ���� ������ ����
    public float k = 0f;    //������Ʈ�� ������ ������ ��ġ
    public float n = 0f;    //������Ʈ�� �����ϴ� ������ �ٶ󺸵��� �ϱ� ���� ȸ���� ����

    void Update()
    {
        
        GameObject Player = GameObject.Find("Player");

        if (transform.position.magnitude > 10000.0f)
        {
            Destroy(gameObject);
        }
        
        t += 90f *Time.deltaTime; // 1�ʿ� 90�� ȸ���Ѵ�.
        //Time.deltaTime�� ������ ������ ���� 90�� ȸ���Ѵ�.
        //������Ʈ�� x�࿡�� �ڻ����� �ֱ�� y�࿡�� ������ �ֱ�� ȸ���Ѵ�. �׷��� �÷��̾ �߽����� �����ϴ� ������ ���δ�.
        //Mathf.Sin/Cos�� ���� �������� �����Ƿ� Ddg2Rad���� ������ �������� �ٲپ��ش�. ���� 1�ʿ� 90���� ȸ���ϰ� �ȴ�.
        gameObject.transform.position = new Vector3(Player.transform.position.x + Mathf.Cos(t*Mathf.Deg2Rad + k * Mathf.Deg2Rad) * 2, Player.transform.position.y + Mathf.Sin(t*Mathf.Deg2Rad + k * Mathf.Deg2Rad) * 2,0);
        gameObject.transform.rotation = Quaternion.AngleAxis(t + n, Vector3.forward);//������Ʈ�� ȸ���ϴ� ������ �ٶ󺸰� �Ѵ�.
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
