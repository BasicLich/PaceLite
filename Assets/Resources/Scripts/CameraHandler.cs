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

    public bool following = true;

    private AudioClip sfx_click;
    private AudioClip sfx_slash;
    private AudioClip sfx_step;
    private AudioClip sfx_hurt;

    public AudioClip music_menu;
    public AudioClip music_game;
    public AudioClip music_win;

    // Start is called before the first frame update
    void Start()
    {
        sfx_click = Resources.Load("SFX/Click") as AudioClip;
        sfx_slash = Resources.Load("SFX/Slash") as AudioClip;
        sfx_step = Resources.Load("SFX/Step") as AudioClip;
        sfx_hurt = Resources.Load("SFX/Hurt") as AudioClip;


        GetComponent<AudioSource>().PlayOneShot(sfx_click, 0.01f);
        ChangeMusic("Menu");
    }

    public void StopMusic()
    {
        GetComponent<AudioSource>().Pause();
    }
    public void ResumeMusic()
    {
        GetComponent<AudioSource>().Play();
    }

    public void ChangeMusic(string music)
    {
        switch (music)
        {
            case "Menu":
                GetComponent<AudioSource>().clip = music_menu;
                break;
            case "Game":
                GetComponent<AudioSource>().clip = music_game;
                break;
            case "Win":
                GetComponent<AudioSource>().clip = music_win;
                break;
        }
        GetComponent<AudioSource>().Play();

    }

    // Update is called once per frame
    void Update()
    {
        if (shakeRemaining <= 0 && following)
        {
            Vector3 newPos = player.transform.position;
            newPos.z = -1;
            transform.position = newPos;
        }
    }

    public void OneShot(string sfx)
    {
        if (sfx.Equals("Click"))
        {
            GetComponent<AudioSource>().PlayOneShot(sfx_click, 1f);
        }
        else if (sfx.Equals("Slash"))
        {
            GetComponent<AudioSource>().pitch = (int)Random.Range(1f, 3f);
            GetComponent<AudioSource>().PlayOneShot(sfx_slash, 1f);
            GetComponent<AudioSource>().pitch = 1;
        }
        else if (sfx.Equals("Step"))
        {
            GetComponent<AudioSource>().PlayOneShot(sfx_step, 0.2f);

        }
        else if (sfx.Equals("Hurt"))
        {
            GetComponent<AudioSource>().PlayOneShot(sfx_hurt, 1f);
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(Resources.Load("SFX/"+sfx) as AudioClip, 1f);
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
