using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speedMod = 100f;

    private string movementType = "Basic";
    private string attackType = "Basic";

    public List<string> moves;
    public List<string> attacks;

    public void Start()
    {
        moves = new List<string>();
        moves.Add("Basic");
        moves.Add("Basic");

        attacks = new List<string>();
        attacks.Add("Basic");
        attacks.Add("Basic");

        Reroll();
        RefreshUI();
    }

    public void Reroll()
    {
        movementType = moves.ElementAt(Random.Range(0, moves.Count));
        attackType = attacks.ElementAt(Random.Range(0, attacks.Count));
    }

    void Update()
    {
        if (!GameObject.Find("Arena").GetComponent<Logic>().IsPaused())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Player>().isMoving = false;
                GetComponent<Player>().isAttacking = false;
                GetComponent<Player>().SetSchemes("None", "None", 0);
                RefreshUI();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                GetComponent<Player>().isMoving = true;
                GetComponent<Player>().isAttacking = false;
                GetComponent<Player>().SetSchemes(movementType, "None", 3);
                RefreshUI();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                GetComponent<Player>().isMoving = true;
                GetComponent<Player>().isAttacking = false;
                GetComponent<Player>().SetSchemes(movementType, "None", 1);
                RefreshUI();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                GetComponent<Player>().isMoving = true;
                GetComponent<Player>().isAttacking = false;
                GetComponent<Player>().SetSchemes(movementType, "None", 2);
                RefreshUI();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                GetComponent<Player>().isMoving = true;
                GetComponent<Player>().isAttacking = false;
                GetComponent<Player>().SetSchemes(movementType, "None", 0);
                RefreshUI();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                GetComponent<Player>().isMoving = false;
                GetComponent<Player>().isAttacking = true;
                GetComponent<Player>().SetSchemes("None", attackType, 3);
                RefreshUI();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                GetComponent<Player>().isMoving = false;
                GetComponent<Player>().isAttacking = true;
                GetComponent<Player>().SetSchemes("None", attackType, 1);
                RefreshUI();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                GetComponent<Player>().isMoving = false;
                GetComponent<Player>().isAttacking = true;
                GetComponent<Player>().SetSchemes("None", attackType, 2);
                RefreshUI();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                GetComponent<Player>().isMoving = false;
                GetComponent<Player>().isAttacking = true;
                GetComponent<Player>().SetSchemes("None", attackType, 0);
                RefreshUI();
            }
        }
        else
        {
            if (GetComponent<Player>().currHP == 0 || GameObject.Find("Arena").GetComponent<Logic>().isVictorious)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameObject.Find("Canvas").GetComponent<UI>().DeathDefade();
                    GameObject.Find("Canvas").GetComponent<UI>().VictoryDefade();
                    GameObject.Find("Arena").GetComponent<Logic>().Restart();
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Application.Quit();
                }
            }
        }
    }

    public void RefreshUI()
    {
        if (GetComponent<Player>().isMoving)
        {
            Color newColor = new Color(0, 1, 0, (float)100 / 255);
            GameObject.Find("Canvas/Move").GetComponent<Image>().color = newColor;
        }
        else
        {
            Color newColor = new Color(1, 1, 1, (float)100 / 255);
            GameObject.Find("Canvas/Move").GetComponent<Image>().color = newColor;
        }

        if (GetComponent<Player>().isAttacking)
        {
            Color newColor = new Color(1, 0, 0, (float)100 / 255);
            GameObject.Find("Canvas/Attack").GetComponent<Image>().color = newColor;
        }
        else
        {
            Color newColor = new Color(1, 1, 1, (float)100 / 255);
            GameObject.Find("Canvas/Attack").GetComponent<Image>().color = newColor;
        }

        //moves
        Transform frame = GameObject.Find("Canvas/Move/").transform;
        foreach (Transform trans in frame)
        {
            if (!trans.gameObject.name.Equals("Background"))
            {
                trans.gameObject.SetActive(false);
            }
        }
        frame.Find(movementType).gameObject.SetActive(true);

        //attacks
        frame = GameObject.Find("Canvas/Attack/").transform;
        foreach (Transform trans in frame)
        {
            if (!trans.gameObject.name.Equals("Background"))
            {
                trans.gameObject.SetActive(false);
            }
        }
        frame.Find(attackType).gameObject.SetActive(true);

        //hps
        int i = 0;
        foreach(Transform hp in GameObject.Find("Canvas/HP").transform)
        {
            if (i < GetComponent<Player>().currHP)
            {
                hp.GetComponent<Image>().color = Color.white;
            }
            else if (i < GetComponent<Player>().maxHP)
            {
                hp.GetComponent<Image>().color = new Color(hp.GetComponent<Image>().color.r, hp.GetComponent<Image>().color.g, hp.GetComponent<Image>().color.b, 100 / 255f);
            }
            else
            {
                hp.GetComponent<Image>().color = Color.clear;
            }
            i++;
        }
    }
}
