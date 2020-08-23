using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public string movementType = "Basic";
    public string attackType = "Basic";

    public List<string> moves;
    public List<string> attacks;

    public bool rerollAttack = false;
    public bool rerollMove = false;

    public void Start()
    {
        moves = new List<string>();
        moves.Add("Basic");
        moves.Add("Basic");

        attacks = new List<string>();
        attacks.Add("Basic");
        attacks.Add("Basic");

        Reroll();
    }

    public void Reroll()
    {
        string prevMovement = movementType;
        string prevAttack = attackType;

        if (rerollAttack || (!rerollAttack && !rerollMove))
        {
            if (attacks.Count > 2)
            {
                do
                {
                    attackType = attacks.ElementAt(Random.Range(0, attacks.Count));
                } while (attackType.Equals(prevAttack));
            }
            rerollAttack = false;
        }
        if (rerollMove || (!rerollAttack && !rerollMove))
        {
            if (moves.Count > 2)
            {
                do
                {
                    movementType = moves.ElementAt(Random.Range(0, moves.Count));
                } while (movementType.Equals(prevMovement));
            }
            rerollMove = false;
        }
    }

    void Update()
    {
        if (!GameObject.Find("Canvas/DeathScreen").GetComponent<SubMenu>().IsOn() && !GameObject.Find("Canvas/VictoryScreen").GetComponent<SubMenu>().IsOn() && !GameObject.Find("Canvas/Menu").GetComponent<SubMenu>().IsOn() && !GameObject.Find("Canvas/Pause").GetComponent<SubMenu>().IsOn() && !GameObject.Find("Canvas/Tutorial").GetComponent<SubMenu>().IsOn())
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject.Find("Arena").GetComponent<Logic>().SetPaused(true);
                GameObject.Find("Canvas/Pause").GetComponent<SubMenu>().Enter();
                Camera.main.GetComponent<CameraHandler>().StopMusic();
                return;
            }
        }
        if (GameObject.Find("Canvas/Pause").GetComponent<SubMenu>().IsOn())
        {
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            else if (Input.anyKeyDown)
            {
                GameObject.Find("Arena").GetComponent<Logic>().SetPaused(false);
                GameObject.Find("Canvas/Pause").GetComponent<SubMenu>().Exit();
                Camera.main.GetComponent<CameraHandler>().ResumeMusic();
                return;
            }
        }

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
        else if (GameObject.Find("Arena").GetComponent<Logic>().tutorial)
        {
            if (Input.anyKeyDown)
            {
                GameObject.Find("Canvas").GetComponent<UI>().ClickTutorial();
            }
        }
        else
        {
            if (!GetComponent<Player>().alive || GameObject.Find("Arena").GetComponent<Logic>().isVictorious)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Camera.main.GetComponent<CameraHandler>().OneShot("Click");
                    Application.Quit();
                }
                else if (Input.anyKeyDown)
                {
                    if (GameObject.Find("Arena").GetComponent<Logic>().isVictorious)
                    {
                        Camera.main.GetComponent<CameraHandler>().ChangeMusic("Menu");
                    }

                    Camera.main.GetComponent<CameraHandler>().OneShot("Click");
                    GameObject.Find("Canvas/DeathScreen").GetComponent<SubMenu>().Exit();
                    GameObject.Find("Canvas/VictoryScreen").GetComponent<SubMenu>().Exit();
                    GameObject.Find("Arena").GetComponent<Logic>().Restart();
                    GameObject.Find("Arena").GetComponent<Logic>().GameStart();
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
                hp.GetComponent<Image>().color = new Color(1f, 1f, 1f, 100f / 255f);
            }
            else
            {
                hp.GetComponent<Image>().color = Color.clear;
            }
            i++;
        }
    }
}
