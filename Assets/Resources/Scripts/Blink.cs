using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    
    private Color baseColor;
    public float str = 0.1f;
    public bool useCos = false;

    void Start()
    {
        baseColor = GetComponent<Image>().color;
    }

    void Update()
    {

        float shift = str;
        if (useCos)
        {
            shift *= Mathf.Cos(Time.time);

        }
        else
        {
            shift*=Mathf.Sin(Time.time);
        }
        Color newColor = baseColor;
        newColor.r += shift;
        newColor.g += shift;
        newColor.b += shift;
        GetComponent<Image>().color = newColor;

    }
}
