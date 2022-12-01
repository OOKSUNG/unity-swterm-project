using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieMenu : MonoBehaviour
{

    [SerializeField] private GameObject BaseUI;
    [SerializeField] private GameObject HpBar;
    [SerializeField] private GameObject ExpBar;

    public Player Player;

    public Button Exit;
    public Button restart;

    // Start is called before the first frame update
    void Start()
    {
        BaseUI.SetActive(false);

        Exit.onClick.AddListener(ClickExit);
        restart.onClick.AddListener(Restartgame);
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.isDie == true)
        {
            CallMenu();
        }
        else
            CloseMenu();
    }

    private void CallMenu()
    {
        BaseUI.SetActive(true);
        ExpBar.SetActive(false);
        HpBar.SetActive(false);
    }

    private void CloseMenu()
    {
        BaseUI.SetActive(false);
        ExpBar.SetActive(true);
        HpBar.SetActive(true);
    }

    public void ClickExit()
    {
        Application.Quit(); //게임 종료
    }

    void Restartgame()
    {
        Player.isDie = false;
        CloseMenu();
    }

}
