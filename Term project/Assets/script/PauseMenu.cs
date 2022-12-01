using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject go_BaseUI;
    [SerializeField] private GameObject HpBar;
    [SerializeField] private GameObject ExpBar;
    
    void Start()
    {
        go_BaseUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager.isPause)
                CallMenu();
            else
                CloseMenu();
        }

    }

    private void CallMenu()
    {
        GameManager.isPause = true;
        go_BaseUI.SetActive(true);
        ExpBar.SetActive(false);
        HpBar.SetActive(false);
        Time.timeScale = 0f;
    }

    private void CloseMenu()
    {
        GameManager.isPause = false;
        go_BaseUI.SetActive(false);
        ExpBar.SetActive(true);
        HpBar.SetActive(true);
        Time.timeScale = 1.0f;
    }

    public void ClickExit()
    {
        Application.Quit(); //게임 종료
    }

}
