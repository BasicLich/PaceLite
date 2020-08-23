using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : SubMenu
{
    void Start()
    {
        GameObject.Find("Canvas").GetComponent<UI>().HideUI();
    }
    void Update()
    {
        if (content.gameObject.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                Camera.main.GetComponent<CameraHandler>().OneShot("Click");
                GameObject.Find("Arena").GetComponent<Logic>().SetPaused(false);
                GameObject.Find("Arena").GetComponent<Logic>().GameStart();
                Exit();
            }
        }
    }
}
