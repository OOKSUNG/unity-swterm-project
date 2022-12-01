using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bExit : MonoBehaviour
{
    public void OnClickExit()
    {
        Application.Quit();
        Debug.Log("Botton Click");
    }
}
