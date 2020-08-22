using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Prompt : MonoBehaviour
{
    public float delay = 0.5f;

    private Color baseColor;
    void Start()
    {
        baseColor = GetComponent<TextMeshProUGUI>().color;
        baseColor.a = 0.96f;
    }

    public void ShowMessage(string msg)
    {
        GetComponent<TextMeshProUGUI>().text = msg;
        GetComponent<TextMeshProUGUI>().color = baseColor;
        InvokeRepeating("Invoke_Fade", delay, 0.05f);
    }

    private void Invoke_Fade()
    {
        Color newColor = GetComponent<TextMeshProUGUI>().color;
        newColor.a -= 0.05f;
        GetComponent<TextMeshProUGUI>().color = newColor;

        if (newColor.a <= 0)
        {
            CancelInvoke();
        }
    }
}
