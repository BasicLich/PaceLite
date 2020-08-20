using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public float str = 0.001f;
    public bool useCos = false;

    void Start()
    {
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
            shift *= Mathf.Sin(Time.time);
        }
        GetComponent<RectTransform>().Rotate(0, 0, shift);
    }
}
