using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Player : MonoBehaviour
{
    float angle;
    Vector2 target, mouse;

    //버튼 텍스트
    public TextMeshProUGUI Bolt_text;
    public TextMeshProUGUI Pulse_text;
    public TextMeshProUGUI Crossed_text;
    public TextMeshProUGUI Circle_text;
    public TextMeshProUGUI Bow_text;

    //버튼
    public Button Bolt_button;
    public Button Pulse_button;
    public Button Crossed_button;
    public Button Circle_button;
    public Button Bow_button;

    //돈
    int money;

    //상점 텍스트
    public TextMeshProUGUI storetext;
    public TextMeshProUGUI lack;

    //체력연장 버튼
    public Button Health_button;


    float BolttimeSpan;  //경과 시간을 갖는 변수
    float PulsetimeSpan;
    float BowtimeSpan;
    float CrossedtimeSpan;
    float CircletimeSpan;// 특정 시간을 갖는 변수

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

    //체력바
    public Image nowHpbar;

    //경험치바
    public Image nowExpbar;

    public TextMeshProUGUI leveltext;


    //레벨
    public int level = 1;

    //마법레벨
    public int pulselevel = 1;
    public int boltlevel = 1;
    public int crossedlevel = 1;
    public int bowlevel = 1;
    public int circlelevel = 1;

    public Button Restat;

    public GameManager GM;

    //다음 스테이지
    public Button Nextstage;

    public Button Bossstage;

    //사운드
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


    //로테이트
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

        //최대체력 늘리기
        Health_button.onClick.AddListener(Health_Button);

        //초기화
        Restat.onClick.AddListener(ReStart);

        maxExp = 5;

        Nextstage.onClick.AddListener(Next);

        Bossstage.onClick.AddListener(BossButton);

        //사운드
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
        
        //체력바 줄어들기
        nowHpbar.fillAmount = (float)health / (float)maxhealth;
        //경험치바 늘리기
        nowExpbar.fillAmount = (float)Exp / (float)maxExp;
        //레벨 텍스트
        leveltext.text = "level " + level;

        GetInput();
        Move();
  
        //방향 전환
        if(Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            anim.SetBool("Walk", true);
        else
            anim.SetBool("Walk", false);

        BolttimeSpan += Time.deltaTime;  // 경과 시간을 계속 등록
        PulsetimeSpan += Time.deltaTime;
        BowtimeSpan += Time.deltaTime;
        CrossedtimeSpan += Time.deltaTime;
        CircletimeSpan += Time.deltaTime;


        if (BolttimeSpan > BoltAttackTerm + AttakTerm)  // 경과 시간이 특정 시간이 보다 커졋을 경우
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

        //레벨 성장
        if (Exp > maxExp)
        {
            level++;
            money++;
            Exp = 0;
            maxExp += 5;
            
        }

        //버튼 텍스트
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
        //스테이지
        

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
            gameObject.layer = 9;//플레이어 레이어를 9번 레이어로
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);//플레이어 투명

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

    

    //마법
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

    


    void FireBolt(int i, float j, float k, float n)     //i = 프리팹 종류 (j,k) = 발사할 방향의 벡터값 n = 발사방향으로 나아가는 것처럼 보이기 위해 오브젝트를 회전할 각도
    {
        
        Vector2 Dir = new Vector2(j, k);    //발사할 방향을 벡터로 받는다.
        GameObject bullet = Instantiate(bulletPrefab[i]);   
        bullet.transform.position = boltPos.transform.position;
        bullet.transform.rotation = Quaternion.AngleAxis(n, Vector3.forward);
        bullet.gameObject.GetComponent<Rigidbody2D>().AddForce(Dir * boltSpeed, ForceMode2D.Impulse);
    }

    void FirePulse(int i, float m, float n)
    {
        Vector2 PlayerPos = rigid.position; //플레이어의 위치를 가져온다.
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //마우스  포인터의 위치를 가져온다.

        //플레이어와 마우스 사이의 벡터 차이를 그대로 가져오면 플레이어와 마우스 사이의 거리 차에 따라 발사 속도가달라진다.
        //그래서 플레이어와 마우스의 벡터를 단위벡터로 만들어준다.
        float dy = MousePos.y - PlayerPos.y;    //마우스와 플레이어 사이의 y축 거리 차이
        float dx = MousePos.x - PlayerPos.x;    //마우스와 플레이어 사이의 x축 거리 차이
        float unitVector = Mathf.Sqrt(dx * dx + dy * dy); //이 값을 벡터에 나누면 단위벡터가 될 것이다.

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
