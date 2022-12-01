using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Player player;
    public CaveGeneratorByCellularAutomata Cave;
    public EnemySpawnManager enemySpawnManager;
    public ItemSpawnManager itemSpawnManager;
    public GameObject CoverImage;

    public static bool isPause = false;//일시 정지 메뉴 창 활성화

    bool enemySpawnStart = true;

    float timeSpan;  //경과 시간을 갖는 변수
    float checkTime = 5;  // 특정 시간을 갖는 변수

    float itemtimeSpan;
    float itemcheckTime = 20;

    //재시작 버튼
    public Button Restart;

    public EnemySpawnManager Espawn;

    //스테이지 텍스트
    public TextMeshProUGUI stagetext;

    //스테이지
    int stage;

    //스테이지 버튼
    public GameObject NextStageButton;

    public GameObject BossButton;
    //보스 스테이지
    string bossstring = " ";

    //다음 스테이지 버튼
    //public Button Nextstage;

    // Start is called before the first frame update
    void Start()
    {
        stage = 1;
        Time.timeScale = 0f;
        Restart.onClick.AddListener(RestartStage);
        InvokeRepeating("StageT", 3, 1);
        BossButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        itemtimeSpan += Time.deltaTime;
        if (itemtimeSpan > itemcheckTime)
        {
            itemSpawnManager.itemSpawn();
            itemtimeSpan = 0;
        }
        
        switch (stage)
        {
            case 1:
                timeSpan += Time.deltaTime;
                if (enemySpawnStart == true)
                {
                    if (timeSpan > checkTime)  
                    {
                        enemySpawnManager.Spawn(0);
                        
                        timeSpan = 0;
                    }
                }
                break;
            case 2:
                timeSpan += Time.deltaTime;  
                if (enemySpawnStart == true)
                {
                    if (timeSpan > checkTime)  
                    {
                        enemySpawnManager.Spawn(0);
                        enemySpawnManager.Spawn(1);
                        timeSpan = 0;
                    }
                }
                break;
            case 3:
                timeSpan += Time.deltaTime;  
                if (enemySpawnStart == true)
                {
                    if (timeSpan > checkTime)  
                    {
                        enemySpawnManager.Spawn(0);
                        enemySpawnManager.Spawn(1);
                        enemySpawnManager.Spawn(2);
                        timeSpan = 0;
                    }
                }
                break;
            case 4:
                timeSpan += Time.deltaTime;  
                if (enemySpawnStart == true)
                {
                    if (timeSpan > checkTime)  
                    {
                        enemySpawnManager.Spawn(0);
                        enemySpawnManager.Spawn(1);
                        enemySpawnManager.Spawn(2);
                        enemySpawnManager.Spawn(3);
                        timeSpan = 0;
                    }
                }
                NextStageButton.SetActive(false);
                BossButton.SetActive(true);
                break;
            case 5:
                
                break;
        }
        
        
    }

    void StageT()
    {
        stagetext.text = "STAGE " + stage + bossstring;
    }

    public void OnClickStartButton()
    {
        StageStart();
    }

    void StageStart()
    {
        CoverImage.SetActive(false);
        Cave.GenerateMap();
        Time.timeScale = 1f;
        enemySpawnStart = true;
    }

    void RestartStage()
    {
        stage = 1;
        CoverImage.SetActive(false);
        Time.timeScale = 1;
        enemySpawnStart = true;
        DestroyClone("Enemy");
        DestroyClone("Wall");
        Cave.GenerateMap();
        NextStageButton.SetActive(true);
        BossButton.SetActive(false);
    }

    public void NextStage()
    {
        stage += 1;
        CoverImage.SetActive(false);
        if (isPause == false)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
        enemySpawnStart = true;
        DestroyClone("Enemy");
        DestroyClone("Wall");
        Cave.GenerateMap();
    }

    public void BossStage()
    {
        if (isPause == false)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
        stage = 0;
        bossstring = " Boss";
        CoverImage.SetActive(false);
        enemySpawnStart = true;
        DestroyClone("Enemy");
        Cave.BossMappGenerator();
        BossButton.SetActive(false);
    }

    public void DestroyClone(string str)
    {
        GameObject[] clone = GameObject.FindGameObjectsWithTag(str);

        for (int i = 0; i < clone.Length; i++)
        {
            Destroy(clone[i]);
        }
    }
}
