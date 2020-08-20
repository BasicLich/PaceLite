using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public float baseAnchor = 0.5f;
    public bool isOn = true;
    public bool useCos = false;

    void Update()
    {
        if (isOn)
        {
            if (useCos)
            {
                GetComponent<RectTransform>().anchorMin = new Vector2(0, Mathf.Cos(Time.time) * 0.01f + baseAnchor);
            }
            else
            {
                GetComponent<RectTransform>().anchorMin = new Vector2(0, Mathf.Sin(Time.time) * 0.01f + baseAnchor);
            }
        }
    }
}
