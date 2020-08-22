using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public float duration = 0.1f;

    private float createdOn = 0;
    private int str;

    public void Launch(Vector3 pos, int str, bool player = false)
    {
        transform.position = pos;
        createdOn = Time.time;
        this.str = 1;
        transform.localScale = new Vector3(str+0.5f, str+ 0.5f, 1f);

        if (player)
        {
            GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/spritesheet")[557];
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void Update()
    {
        Color newColor = GetComponent<SpriteRenderer>().color;
        newColor.a = Mathf.Abs(1f-(Time.time - createdOn) / duration);
        GetComponent<SpriteRenderer>().color = newColor;

        transform.localScale = new Vector3(Mathf.Abs(str - (Time.time - createdOn) / duration)*str+0.5f, Mathf.Abs(str - (Time.time - createdOn) / duration)* str + 0.5f, 1f);

        if (Time.time - createdOn >= duration)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
