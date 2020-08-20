using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public GameObject player;

    private float shakeRemaining = 0f;
    private float shakeMod = 0f;
    private float shakeDecay = 0f;
    private Vector2 shakePivot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeRemaining <= 0)
        {
            Vector3 newPos = player.transform.position;
            newPos.z = -1;
            transform.position = newPos;
        }
    }

    public void Shake(float duration, float modifier)
    {
        shakeMod = modifier;
        shakeRemaining += duration;
        shakeDecay = duration / 10f;
        shakePivot = transform.position;
        InvokeRepeating("Shake", 0, shakeDecay);
    }

    private void Shake()
    {
        shakeRemaining -= shakeDecay;
        Vector3 newPos = shakePivot + (Vector2)Random.insideUnitSphere * shakeMod;
        newPos.z = -1;
        transform.position = newPos;

        if (shakeRemaining <= 0)
        {
            CancelInvoke();
        }
    }
}
