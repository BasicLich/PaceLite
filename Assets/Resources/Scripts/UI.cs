using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{

    public void DisplayWave(int number)
    {
        GameObject.Find("Canvas/Wave Number").GetComponent<TextMeshProUGUI>().text = ""+number;
        GameObject.Find("Canvas/Next Wave/Image").GetComponent<CanvasGroup>().alpha = 1f;
        InvokeRepeating("WaveFade", 1f, 0.01f);
    }
    private void WaveFade()
    {
        GameObject.Find("Canvas/Next Wave/Image").GetComponent<CanvasGroup>().alpha -= 0.01f;
        if(GameObject.Find("Canvas/Next Wave/Image").GetComponent<CanvasGroup>().alpha <= 0)
        {
            CancelInvoke();
        }
    }


    public void Prompt(string msg)
    {
        GameObject.Find("Canvas/Prompt").GetComponent<Prompt>().ShowMessage(msg);
    }

    public void HideUI()
    {
        GameObject.Find("Canvas").transform.Find("HP").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("Attack").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("Move").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("Pacer").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("Enemy Counter").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("Wave Number").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("HP").gameObject.SetActive(false);

    }

    public void ClickTutorial()
    {
        Camera.main.GetComponent<CameraHandler>().OneShot("Click");
        bool activate = false;
        int i = 0;
        foreach(Transform trans in GameObject.Find("Canvas/Tutorial/Content").transform)
        {
            if (activate)
            {
                trans.gameObject.SetActive(true);
                if (i > 1)
                {
                    GameObject.Find("Canvas/").transform.Find(trans.gameObject.name).gameObject.SetActive(true);
                }

                return;
            }
            else if (trans.gameObject.activeSelf)
            {
                
                if (i == GameObject.Find("Canvas/Tutorial/Content").transform.childCount-1)
                {
                    GameObject.Find("Arena").GetComponent<Logic>().SetPaused(false);
                    GameObject.Find("Arena").GetComponent<Logic>().StartingSpawn();
                    GameObject.Find("Canvas/").transform.Find("Wave Number").gameObject.SetActive(true);
                    GameObject.Find("Canvas/Tutorial").GetComponent<SubMenu>().Exit();
                }
                else if (i != GameObject.Find("Canvas/Tutorial/Content").transform.childCount)
                {
                    activate = true;
                    trans.gameObject.SetActive(false);
                }
            }
            i++;
        }
        GameObject.Find("Canvas/Tutorial/Content").transform.GetChild(0).gameObject.SetActive(true);
    }
}
