using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Player : MonoBehaviour
{
    float angle;
    Vector2 target, mouse;

    //��ư �ؽ�Ʈ
    public TextMeshProUGUI Bolt_text;
    public TextMeshProUGUI Pulse_text;
    public TextMeshProUGUI Crossed_text;
    public TextMeshProUGUI Circle_text;
    public TextMeshProUGUI Bow_text;

    //��ư
    public Button Bolt_button;
    public Button Pulse_button;
    public Button Crossed_button;
    public Button Circle_button;
    public Button Bow_button;

    //��
    int money;

    //���� �ؽ�Ʈ
    public TextMeshProUGUI storetext;
    public TextMeshProUGUI lack;

    //ü�¿��� ��ư
    public Button Health_button;


    float BolttimeSpan;  //��� �ð��� ���� ����
    float PulsetimeSpan;
    float BowtimeSpan;
    float CrossedtimeSpan;
    float CircletimeSpan;// Ư�� �ð��� ���� ����

    float BoltAttackTerm = 3;
    float PulseAttackTerm = 10;
    float CircleAttackTerm = 11;
    float BowAttackTerm = 7;
    float CrossedAttackTerm = 4;

    [SerializeField]
    private float speed;
    private Vector2 direction;

    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;

    ProjectileController projectileController;

    [SerializeField]
    private GameObject[] bulletPrefab;

    public GameObject boltPos;
    public GameObject pulsePos;
    public GameObject circlePos;

    [SerializeField]
    private GameObject[] bowPos;

    [SerializeField]
    float boltSpeed;
    [SerializeField]
    float pulseSpeed;
    [SerializeField]
    float crossedSpeed;

    [SerializeField]
    public int maxhealth;

    int health;

    int maxExp;
    int Exp;


    public bool isDie = false;

    bool bolt = false;
    public bool pulse = false;
    public bool circle = false;
    public bool bow = false;
    public bool crossed = false;

    [SerializeField]
    float AttakTerm;

    //ü�¹�
    public Image nowHpbar;

    //����ġ��
    public Image nowExpbar;

    public TextMeshProUGUI leveltext;


    //����
    public int level = 1;

    //��������
    public int pulselevel = 1;
    public int boltlevel = 1;
    public int crossedlevel = 1;
    public int bowlevel = 1;
    public int circlelevel = 1;

    public Button Restat;

    public GameManager GM;

    //���� ��������
    public Button Nextstage;

    public Button Bossstage;

    //����
    public AudioClip Elec2;
    public AudioClip Lazer1;
    public AudioClip Magic3;
    public AudioClip Lazer2;
    public AudioClip CrossBow;
    public AudioClip Healsound;
    public AudioClip Boxsound;
    public AudioClip Coinsound;

    AudioSource audioSource;

    public GameObject End;


    //������Ʈ
    float t = 0;
    public int boltnum = 5;

    public GameObject[] bolts;
    public GameObject[] bullets;

    float boltcooltime = 3;

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        health = maxhealth;

        BolttimeSpan = 0.0f;
        PulsetimeSpan = 0.0f;

        Bolt_button.onClick.AddListener(Bolt_Button);
        Pulse_button.onClick.AddListener(Pulse_Button);
        Crossed_button.onClick.AddListener(Crossed_Button);
        Circle_button.onClick.AddListener(Circle_Button);
        Bow_button.onClick.AddListener(Bow_Button);

        //�ִ�ü�� �ø���
        Health_button.onClick.AddListener(Health_Button);

        //�ʱ�ȭ
        Restat.onClick.AddListener(ReStart);

        maxExp = 5;

        Nextstage.onClick.AddListener(Next);

        Bossstage.onClick.AddListener(BossButton);

        //����
        audioSource = GetComponent<AudioSource>();

        money = 500;

        End.SetActive(false);

        
        InvokeRepeating("Rotate", 3, boltcooltime);

        //GameObject bullet = Instantiate(bulletPrefab[0]);
    }

    void ReStart()
    {
        direction = Vector2.zero;
        health = maxhealth;

        BolttimeSpan = 0.0f;
        PulsetimeSpan = 0.0f;

        pulselevel = 1;
        boltlevel = 1;
        crossedlevel = 1;
        bowlevel = 1;
        circlelevel = 1;
        level = 1;

        bolt = false;
        pulse = false;
        circle = false;
        bow = false;
        crossed = false;

        money = 1;

        gameObject.transform.position = Vector2.zero;
        anim.Play("idle");
}


    // Update is called once per frame
    void Update()
    {
        
        //ü�¹� �پ���
        nowHpbar.fillAmount = (float)health / (float)maxhealth;
        //����ġ�� �ø���
        nowExpbar.fillAmount = (float)Exp / (float)maxExp;
        //���� �ؽ�Ʈ
        leveltext.text = "level " + level;

        GetInput();
        Move();
  
        //���� ��ȯ
        if(Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            anim.SetBool("Walk", true);
        else
            anim.SetBool("Walk", false);

        BolttimeSpan += Time.deltaTime;  // ��� �ð��� ��� ���
        PulsetimeSpan += Time.deltaTime;
        BowtimeSpan += Time.deltaTime;
        CrossedtimeSpan += Time.deltaTime;
        CircletimeSpan += Time.deltaTime;


        if (BolttimeSpan > BoltAttackTerm + AttakTerm)  // ��� �ð��� Ư�� �ð��� ���� Ŀ���� ���
        {
            BolttimeSpan = 0;
            //Bolt();
        }

        if (PulsetimeSpan > PulseAttackTerm + AttakTerm)
        {
            PulsetimeSpan = 0;
            Pulse();
        }

        if (CircletimeSpan > CircleAttackTerm + AttakTerm)
        {
            CircletimeSpan = 0;

            if (circle == true)
            {
                Circle();
            }
        }

        if (BowtimeSpan > BowAttackTerm + AttakTerm)
        {
            BowtimeSpan = 0;
            Bow();
        }

        if (CrossedtimeSpan > CrossedAttackTerm + AttakTerm)
        {
            CrossedtimeSpan = 0;
            Crossed();
        }

        //���� ����
        if (Exp > maxExp)
        {
            level++;
            money++;
            Exp = 0;
            maxExp += 5;
            
        }

        //��ư �ؽ�Ʈ
        if (bolt == false)
            Bolt_text.text = "Bolt unactivated";
        else
            Bolt_text.text = "Bolt [" + boltlevel + "/5]";

        if (pulse == false)
            Pulse_text.text = "Pulse unactivated";
        else
            Pulse_text.text = "Pulse [" + pulselevel + "/5]";

        if (crossed == false)
            Crossed_text.text = "Crossed unactivated";
        else
            Crossed_text.text = "Crossed [" + crossedlevel + "/5]";

        if (circle == false)
            Circle_text.text = "Circle unactivated";
        else
            Circle_text.text = "Circle [" + circlelevel + "/5]";

        if (bow == false)
            Bow_text.text = "Bow unactivated";
        else
            Bow_text.text = "Bow [" + bowlevel + "/5]";

        storetext.text = "[Store] Money: " + money;
        if (money == 0)
            lack.text = "Lack of money";
        //��������
        

        if (health == 0)
        {
            if (!isDie)
                Die();

            return;
        }


        
        
    }

    void FixedUpdate()
    {
        
    }

    void Next()
    {
        if (money >= 50)
        {
            GM.NextStage();
            money-=50;
        }
            
    }

    void BossButton()
    {
        if (money >= 50)
        {
            GM.BossStage();
            money -= 50;
        }

        transform.position = new Vector3(100, 100, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health--;
            Debug.Log("Player Hit");
            OnDamaged(collision.transform.position);
        }

        if (collision.gameObject.tag == "Fire")
        {
            health--;
            Debug.Log("Player Hit");
            OnDamaged(collision.transform.position);
        }

        if (collision.gameObject.tag == "Coin")
        {
            Exp++;
            Debug.Log("Exp +1");
            audioSource.clip = Coinsound;
            audioSource.Play();
        }

        if (collision.gameObject.tag == "Heart")
        {
            if (maxhealth - health <= 10)
                health = maxhealth;
            else if (maxhealth - health > 10)
                health += 10;
            Debug.Log("health +10");
            audioSource.clip = Healsound;
            audioSource.Play();
        }

        if (collision.gameObject.tag == "Box")
        {
            int k = Random.Range(1, 11);
            money += k;
            Debug.Log("Money +" + k);
            audioSource.clip = Boxsound;
            audioSource.Play();
        }


        void OnDamaged(Vector2 targetPos)
        {
            gameObject.layer = 9;//�÷��̾� ���̾ 9�� ���̾��
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);//�÷��̾� ����

            Invoke("OffDamaged", 1);
        }

        if (collision.gameObject.tag == "Door")
        {
            gameObject.transform.position = new Vector3(230, 100, 0);
            End.SetActive(true);
            
        }

    }
    
    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void Move()
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }

    private void GetInput()
    {
        Vector2 moveVector;

        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.y = Input.GetAxisRaw("Vertical");

        direction = moveVector;

    }

    void Die()
    {
        isDie = true;
        Debug.Log("Player Die");
        anim.Play("Die");

    }

    

    //����
    /*
    void Bolt()
    {
        if (bolt == true)
        {
            switch (boltlevel)
            {
                case 1:
                    audioSource.clip = Elec2;
                    audioSource.Play();
                    FireBolt(0, 1, 1, 225);
                    break;
                case 2:
                    audioSource.clip = Elec2;
                    audioSource.Play();
                    FireBolt(0, 1, 1, 225);
                    FireBolt(0, 1, -1, 135);
                    break;
                case 3:
                    audioSource.clip = Elec2;
                    audioSource.Play();
                    FireBolt(0, 1, 1, 225);
                    FireBolt(0, 1, -1, 135);
                    FireBolt(0, -1, 1, 315);
                    break;
                case 4:
                    audioSource.clip = Elec2;
                    audioSource.Play();
                    FireBolt(0, 1, 1, 225);
                    FireBolt(0, 1, -1, 135);
                    FireBolt(0, -1, 1, 315);
                    FireBolt(0, -1, -1, 45);
                    break;
                case 5:
                    audioSource.clip = Elec2;
                    audioSource.Play();
                    boltSpeed = 7;
                    FireBolt(0, 1, 1, 225);
                    FireBolt(0, 1, -1, 135);
                    FireBolt(0, -1, 1, 315);
                    FireBolt(0, -1, -1, 45);
                    break;
            }

        }
    }
    */
    void Pulse()
    {
        if (pulse == true)
        {
            switch (pulselevel)
            {
                case 1:
                    break;
                case 2:
                    PulseAttackTerm =8;
                    pulseSpeed = 4;
                    break;
                case 3:
                    PulseAttackTerm = 6;
                    pulseSpeed = 6;
                    break;
                case 4:
                    PulseAttackTerm = 4;
                    pulseSpeed = 8;
                    break;
                case 5:
                    PulseAttackTerm = 2;
                    pulseSpeed = 10;
                    break;

            }
            audioSource.clip = Lazer1;
            audioSource.Play();
            FirePulse(1, 0, 0);

        }
    }

    void Bow()
    {
        if (bow == true)
        {
            switch (bowlevel)
            {
                case 1:
                    audioSource.clip = CrossBow;
                    audioSource.Play();
                    FireBow(3, 0);
                    break;
                case 2:
                    audioSource.clip = CrossBow;
                    audioSource.Play();
                    FireBow(3, 0);
                    FireBow(3, 1);
                    break;
                case 3:
                    audioSource.clip = CrossBow;
                    audioSource.Play();
                    FireBow(3, 0);
                    FireBow(3, 1);
                    FireBow(3, 2);
                    break;
                case 4:
                    audioSource.clip = CrossBow;
                    audioSource.Play();
                    FireBow(3, 0);
                    FireBow(3, 1);
                    FireBow(3, 2);
                    FireBow(3, 3);
                    break;
                case 5:
                    audioSource.clip = CrossBow;
                    audioSource.Play();
                    FireBow(3, 0);
                    FireBow(3, 1);
                    FireBow(3, 2);
                    FireBow(3, 3);
                    FireBow(3, 4);
                    break;
            }

        }
    }

    void Crossed()
    {
        if (crossed == true)
        {
            switch (crossedlevel)
            {
                case 1:
                    audioSource.clip = Magic3;
                    audioSource.Play();
                    FireBolt(4, 1, 0, 0);
                    break;
                case 2:
                    audioSource.clip = Magic3;
                    audioSource.Play();
                    FireBolt(4, 1, 0, 0);
                    FireBolt(4, -1, 0, 180);
                    break;
                case 3:
                    audioSource.clip = Magic3;
                    audioSource.Play();
                    FireBolt(4, 0, 1, 90);
                    FireBolt(4, Mathf.Cos(60), Mathf.Sin(60), 210);
                    FireBolt(4, -Mathf.Cos(60), Mathf.Sin(60), 330);
                    break;
                case 4:
                    audioSource.clip = Magic3;
                    audioSource.Play();
                    FireBolt(4, 1, 0, 0);
                    FireBolt(4, 0, 1, 90);
                    FireBolt(4, -1, 0, 180);
                    FireBolt(4, 0, -1, 270);
                    break;
                case 5:
                    audioSource.clip = Magic3;
                    audioSource.Play();
                    FireBolt(4, -Mathf.Cos(72), Mathf.Sin(72), 18);
                    FireBolt(4, 0, 1, 90);
                    FireBolt(4, Mathf.Cos(72), Mathf.Sin(72), 162);
                    FireBolt(4, -Mathf.Cos(18), Mathf.Sin(18), 234);
                    FireBolt(4, Mathf.Cos(18), Mathf.Sin(18), 306);
                    break;
            }

        }
    }

    void Circle()
    {
        audioSource.clip = Lazer2;
        audioSource.Play();
        GameObject bullet = Instantiate(bulletPrefab[2]);
        bullet.transform.position = rigid.position;

        switch (circlelevel)
        {
            case 1:
                break;
            case 2:
                CircleAttackTerm = 9;
                break;
            case 3:
                CircleAttackTerm = 7;
                break;
            case 4:
                CircleAttackTerm = 5;
                break;
            case 5:
                CircleAttackTerm = 3;
                break;
        }

    }


    private void destroy()
    {
        Destroy(gameObject);
    }

    

    void Rotate()
    {
        bolts = GameObject.FindGameObjectsWithTag("Bolt");
        if (bolt == true)
        {
            if (bolts.Length <= boltnum)
            {
                
                switch (boltlevel)
                {
                    case 1:
                        Instantiate(bulletPrefab[0]);
                        break;
                    case 2:
                        Instantiate(bulletPrefab[0]);
                        Instantiate(bulletPrefab[5]);
                        break;
                    case 3:
                        Instantiate(bulletPrefab[0]);
                        Instantiate(bulletPrefab[6]);
                        Instantiate(bulletPrefab[7]);
                        break;
                    case 4:
                        Instantiate(bulletPrefab[0]);
                        Instantiate(bulletPrefab[5]);
                        Instantiate(bulletPrefab[8]);
                        Instantiate(bulletPrefab[9]);
                        break;
                    case 5:
                        Instantiate(bulletPrefab[0]);
                        Instantiate(bulletPrefab[10]);
                        Instantiate(bulletPrefab[11]);
                        Instantiate(bulletPrefab[12]);
                        Instantiate(bulletPrefab[13]);
                        break;

                }
                
                
                    


            }
        }
        
    }

    


    void FireBolt(int i, float j, float k, float n)     //i = ������ ���� (j,k) = �߻��� ������ ���Ͱ� n = �߻�������� ���ư��� ��ó�� ���̱� ���� ������Ʈ�� ȸ���� ����
    {
        
        Vector2 Dir = new Vector2(j, k);    //�߻��� ������ ���ͷ� �޴´�.
        GameObject bullet = Instantiate(bulletPrefab[i]);   
        bullet.transform.position = boltPos.transform.position;
        bullet.transform.rotation = Quaternion.AngleAxis(n, Vector3.forward);
        bullet.gameObject.GetComponent<Rigidbody2D>().AddForce(Dir * boltSpeed, ForceMode2D.Impulse);
    }

    void FirePulse(int i, float m, float n)
    {
        Vector2 PlayerPos = rigid.position; //�÷��̾��� ��ġ�� �����´�.
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //���콺  �������� ��ġ�� �����´�.

        //�÷��̾�� ���콺 ������ ���� ���̸� �״�� �������� �÷��̾�� ���콺 ������ �Ÿ� ���� ���� �߻� �ӵ����޶�����.
        //�׷��� �÷��̾�� ���콺�� ���͸� �������ͷ� ������ش�.
        float dy = MousePos.y - PlayerPos.y;    //���콺�� �÷��̾� ������ y�� �Ÿ� ����
        float dx = MousePos.x - PlayerPos.x;    //���콺�� �÷��̾� ������ x�� �Ÿ� ����
        float unitVector = Mathf.Sqrt(dx * dx + dy * dy); //�� ���� ���Ϳ� ������ �������Ͱ� �� ���̴�.

        Vector2 Dir = (MousePos - PlayerPos);
        GameObject bullet = Instantiate(bulletPrefab[i]);
        bullet.transform.position = pulsePos.transform.position;
        bullet.transform.rotation = Quaternion.Euler(m, n, 0);
        bullet.gameObject.GetComponent<Rigidbody2D>().AddForce(Dir * pulseSpeed / unitVector, ForceMode2D.Impulse);

        target = transform.position;
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - target.y, mouse.x - target.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void FireBow(int i, int j)
    {
        GameObject bullet = Instantiate(bulletPrefab[i]);
        bullet.transform.position = bowPos[j].transform.position;
        bullet.transform.rotation = Quaternion.AngleAxis(270, Vector3.forward);
    }

    void Bolt_Button()
    {
        if (money > 0)
        {
            if (bolt == false)
            {
                bolt = true;
                money--;
            }
            else
            {
                if (boltlevel < 5)
                {
                    boltlevel++;
                    money--;
                }

            }
        }

    }

    void Pulse_Button()
    {
        if (money > 0)
        {
            if (pulse == false)
            {
                pulse = true;
                money--;
            }
            else
            {
                if (pulselevel < 5)
                {
                    pulselevel++;
                    money--;
                }

            }
        }

    }

    void Crossed_Button()
    {
        if (money > 0)
        {
            if (crossed == false)
            {
                crossed = true;
                money--;
            }
            else
            {
                if (crossedlevel < 5)
                {
                    crossedlevel++;
                    money--;
                }

            }
        }

    }

    void Circle_Button()
    {
        if (money > 0)
        {
            if (circle == false)
            {
                circle = true;
                money--;
            }
            else
            {
                if (circlelevel < 5)
                {
                    circlelevel++;
                    money--;
                }

            }
        }


    }

    void Bow_Button()
    {
        if (money > 0)
        {
            if (bow == false)
            {
                bow = true;
                money--;
            }
            else
            {
                if (bowlevel < 5)
                {
                    bowlevel++;
                    money--;
                }

            }
        }
    }

    void Health_Button()
    {
        if (money > 1)
        {
            maxhealth += 10;
            money -= 2;
        }
    }

   
}
