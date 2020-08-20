using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public void DeathDisplay()
    {
        GameObject.Find("Canvas").transform.Find("DeathScreen").gameObject.SetActive(true);
        DeathFade();
    }

    public void DisplayWave(int number)
    {
        GameObject.Find("Canvas/Next Wave").transform.Find("Image").GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
        GameObject.Find("Canvas/Next Wave").transform.Find("Image").gameObject.SetActive(true);
        Vector2 newVec = GameObject.Find("Canvas/Wave Number").GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        GameObject.Find("Canvas/Wave Number").GetComponent<TextMeshProUGUI>().text = ""+number;
        InvokeRepeating("WaveFade", 0.15f, 0.01f);
    }
    private void WaveFade()
    {
        Vector2 newVec = GameObject.Find("Canvas/Wave Number").GetComponent<RectTransform>().anchorMin;
        newVec.y += 0.01f;
        newVec.x += 0.01f;

        GameObject.Find("Canvas/Wave Number").GetComponent<RectTransform>().anchorMin = newVec;

        Color newColor = GameObject.Find("Canvas/Next Wave").transform.Find("Image").GetComponent<Image>().color;
        newColor.a -= 0.01f;
        GameObject.Find("Canvas/Next Wave").transform.Find("Image").GetComponent<Image>().color = newColor;
        GameObject.Find("Canvas/Next Wave").transform.Find("Image").GetComponent<RectTransform>().localScale = new Vector2(1f-newVec.x, 1f-newVec.y);

        if (newVec.y >= 0.9f)
        {
            GameObject.Find("Canvas/Wave Number").GetComponent<TextMeshProUGUI>().text = "Wave: " + GameObject.Find("Canvas/Wave Number").GetComponent<TextMeshProUGUI>().text;
            GameObject.Find("Canvas/Next Wave").transform.Find("Image").gameObject.SetActive(false);
            CancelInvoke();
        }
    }

    public void Button_Start()
    {
        GameObject.Find("Canvas/Menu/Content/Start").SetActive(false);
        GameObject.Find("Canvas/Menu/Content/Logo").GetComponent<Pulse>().isOn = false;


        InvokeRepeating("Fade", 0, 0.01f);
    }

    private void Invoke_DeathFade()
    {
        Color newColor = GameObject.Find("Canvas/DeathScreen/Fade").GetComponent<Image>().color;
        newColor.a += 0.01f;
        GameObject.Find("Canvas/DeathScreen/Fade").GetComponent<Image>().color = newColor;
        if (newColor.a > 0.5f)
        {
            CancelInvoke();
        }
    }
    public void DeathFade()
    {
        GameObject.Find("Canvas/DeathScreen/").transform.Find("Content").gameObject.SetActive(true);
        GameObject.Find("Canvas/DeathScreen/Fade").GetComponent<Image>().raycastTarget = true;
        InvokeRepeating("Invoke_DeathFade", 0, 0.01f);
    }
    public void DeathDefade()
    {
        GameObject.Find("Canvas/DeathScreen/").transform.Find("Content").gameObject.SetActive(false);
        GameObject.Find("Canvas/DeathScreen/Fade").GetComponent<Image>().raycastTarget = false;
        GameObject.Find("Canvas/DeathScreen/Fade").GetComponent<Image>().color = new Color(0, 0, 0, 0);

    }

    private void Fade()
    {
        Color newColor = GameObject.Find("Canvas/Menu/Fade").GetComponent<Image>().color;
        newColor.a -= 0.01f;
        GameObject.Find("Canvas/Menu/Fade").GetComponent<Image>().color = newColor;



        Vector2 vecMax = GameObject.Find("Canvas/Menu/Content").GetComponent<RectTransform>().anchorMax;
        vecMax.y -= 0.01f;

        GameObject.Find("Canvas/Menu/Content").GetComponent<RectTransform>().anchorMax = vecMax;


        if (newColor.a <= 0)
        {
            GameObject.Find("Canvas/Menu/Content/").SetActive(false);
            GameObject.Find("Canvas/Menu/Fade/").SetActive(false);
            GameObject.Find("Arena/").GetComponent<Logic>().SetPaused(false);
            CancelInvoke();
        }
    }

    public void Prompt(string msg)
    {
        Debug.Log(msg);
    }
}
