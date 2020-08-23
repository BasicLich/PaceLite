using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubMenu : MonoBehaviour
{
    public float intensity = 0.6f;
    public bool raycast;

    public float speed = 0.03f;

    private float progress = 0f;

    public GameObject fade;
    public GameObject content;

    public bool isOn = false;


    public void Enter()
    {
        isOn = true;
        progress = speed;
        if (raycast)
        {
            fade.GetComponent<Image>().raycastTarget = true;
        }
        content.SetActive(true);
        InvokeRepeating("InvokeFade", 0f, 0.03f);
    }
    public void Exit()
    {
        isOn = false;
        progress = -speed;
        content.SetActive(false);
        fade.GetComponent<Image>().raycastTarget = false;
        InvokeRepeating("InvokeFade", 0f, 0.03f);
    }
    private void InvokeFade()
    {
        fade.GetComponent<Image>().color = AddAlpha(fade.GetComponent<Image>().color, progress);
        if (fade.GetComponent<Image>().color.a <= 0f)
        {
            CancelInvoke();
        }
        else if (fade.GetComponent<Image>().color.a >= intensity)
        {
            CancelInvoke();
        }
    }

    private Color AddAlpha(Color color, float alpha) {
        Color newColor = color;
        newColor.a += alpha;
        if (newColor.a > 1f)
        {
            newColor.a = 1f;
        }
        else if (newColor.a < 0)
        {
            newColor.a = 0f;
        }
        return newColor;
    }

    public bool IsOn()
    {
        return isOn;
    }
}
