using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public bool isMoving = false;
    public bool isAttacking = false;

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        Camera.main.GetComponent<CameraHandler>().OneShot("Hurt");

        Camera.main.GetComponent<CameraHandler>().Shake(0.25f, 0.1f);
        GetComponent<PlayerController>().RefreshUI();
    }
    public override void Die(string way = "Regular")
    {
        Camera.main.GetComponent<CameraHandler>().ChangeMusic("Menu");

        GetComponent<PlayerController>().RefreshUI();
        GameObject.Find("Arena").GetComponent<Logic>().SetPaused(true);
        GameObject.Find("Canvas/DeathScreen").GetComponent<SubMenu>().Enter();


    }

    public void SetSchemes(string moveType, string attackType, int direction)
    {
        SetMoveScheme(moveType, direction);
        SetAttackScheme(attackType, direction);
    }
    private void SetAttackScheme(string attackType, int direction)
    {
        attacks.Clear();
        Scheme newScheme = new Scheme();
        switch (attackType)
        {
            case "Basic":
                switch (direction)
                {
                    case 0:
                        newScheme.scheme.Add(new Vector2(0, 1));
                        break;
                    case 1:
                        newScheme.scheme.Add(new Vector2(1, 0));
                        break;
                    case 2:
                        newScheme.scheme.Add(new Vector2(0, -1));
                        break;
                    case 3:
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        break;
                }
                break;
            case "Thrust":
                switch (direction)
                {
                    case 0:
                        newScheme.scheme.Add(new Vector2(0, 1));
                        newScheme.scheme.Add(new Vector2(0, 2));
                        break;
                    case 1:
                        newScheme.scheme.Add(new Vector2(1, 0));
                        newScheme.scheme.Add(new Vector2(2, 0));
                        break;
                    case 2:
                        newScheme.scheme.Add(new Vector2(0, -1));
                        newScheme.scheme.Add(new Vector2(0, -2));
                        break;
                    case 3:
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        newScheme.scheme.Add(new Vector2(-2, 0));
                        break;
                }
                break;
            case "Cleave":
                switch (direction)
                {
                    case 0:
                        newScheme.scheme.Add(new Vector2(-1, 1));
                        newScheme.scheme.Add(new Vector2(0, 1));
                        newScheme.scheme.Add(new Vector2(1, 1));
                        break;
                    case 1:
                        newScheme.scheme.Add(new Vector2(1, -1));
                        newScheme.scheme.Add(new Vector2(1, 0));
                        newScheme.scheme.Add(new Vector2(1, 1));
                        break;
                    case 2:
                        newScheme.scheme.Add(new Vector2(1, -1));
                        newScheme.scheme.Add(new Vector2(0, -1));
                        newScheme.scheme.Add(new Vector2(-1, -1));
                        break;
                    case 3:
                        newScheme.scheme.Add(new Vector2(-1, -1));
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        newScheme.scheme.Add(new Vector2(-1, 1));
                        break;
                }
                break;
            case "Dual":
                switch (direction)
                {
                    case 0:
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        newScheme.scheme.Add(new Vector2(1, 0));
                        break;
                    case 1:
                        newScheme.scheme.Add(new Vector2(0, 1));
                        newScheme.scheme.Add(new Vector2(0, -1));
                        break;
                    case 2:
                        newScheme.scheme.Add(new Vector2(1, 0));
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        break;
                    case 3:
                        newScheme.scheme.Add(new Vector2(0, -1));
                        newScheme.scheme.Add(new Vector2(0, 1));
                        break;
                }
                break;
            case "Whirl":
                newScheme.scheme.Add(new Vector2(0, 1));
                newScheme.scheme.Add(new Vector2(1, 1));
                newScheme.scheme.Add(new Vector2(1, 0));
                newScheme.scheme.Add(new Vector2(1, -1));
                newScheme.scheme.Add(new Vector2(0, -1));
                newScheme.scheme.Add(new Vector2(-1, -1));
                newScheme.scheme.Add(new Vector2(-1, 0));
                newScheme.scheme.Add(new Vector2(-1, 1));
                break;
            case "Cross":
                newScheme.scheme.Add(new Vector2(0, 1));
                newScheme.scheme.Add(new Vector2(1, 0));
                newScheme.scheme.Add(new Vector2(0, -1));
                newScheme.scheme.Add(new Vector2(-1, 0));
                break;
            default:
                break;
        }

        attacks.Add(newScheme);
    }
    private void SetMoveScheme(string moveType, int direction)
    {
        moves.Clear();
        Scheme newScheme = new Scheme();
        switch (moveType)
        {
            case "Basic":
                switch (direction)
                {
                    case 0:
                        newScheme.scheme.Add(new Vector2(0, 1));
                        break;
                    case 1:
                        newScheme.scheme.Add(new Vector2(1, 0));
                        break;
                    case 2:
                        newScheme.scheme.Add(new Vector2(0, -1));
                        break;
                    case 3:
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        break;
                }
                break;
            case "Dash":
                switch (direction)
                {
                    case 0:
                        newScheme.scheme.Add(new Vector2(0, 1));
                        newScheme.scheme.Add(new Vector2(0, 1));
                        break;
                    case 1:
                        newScheme.scheme.Add(new Vector2(1, 0));
                        newScheme.scheme.Add(new Vector2(1, 0));
                        break;
                    case 2:
                        newScheme.scheme.Add(new Vector2(0, -1));
                        newScheme.scheme.Add(new Vector2(0, -1));
                        break;
                    case 3:
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        break;
                }
                break;
            case "L":
                switch (direction)
                {
                    case 0:
                        newScheme.scheme.Add(new Vector2(0, 1));
                        newScheme.scheme.Add(new Vector2(0, 1));
                        newScheme.scheme.Add(new Vector2(1, 0));
                        break;
                    case 1:
                        newScheme.scheme.Add(new Vector2(1, 0));
                        newScheme.scheme.Add(new Vector2(1, 0));
                        newScheme.scheme.Add(new Vector2(0, -1));
                        break;
                    case 2:
                        newScheme.scheme.Add(new Vector2(0, -1));
                        newScheme.scheme.Add(new Vector2(0, -1));
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        break;
                    case 3:
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        newScheme.scheme.Add(new Vector2(0, 1));
                        break;
                }
                break;
            case "Sprint":
                switch (direction)
                {
                    case 0:
                        newScheme.scheme.Add(new Vector2(0, 1));
                        newScheme.scheme.Add(new Vector2(0, 1));
                        newScheme.scheme.Add(new Vector2(0, 1));
                        break;
                    case 1:
                        newScheme.scheme.Add(new Vector2(1, 0));
                        newScheme.scheme.Add(new Vector2(1, 0));
                        newScheme.scheme.Add(new Vector2(1, 0));
                        break;
                    case 2:
                        newScheme.scheme.Add(new Vector2(0, -1));
                        newScheme.scheme.Add(new Vector2(0, -1));
                        newScheme.scheme.Add(new Vector2(0, -1));
                        break;
                    case 3:
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        break;
                }
                break;
            case "Clockwise":
                switch (direction)
                {
                    case 0:
                        newScheme.scheme.Add(new Vector2(0, 1));
                        newScheme.scheme.Add(new Vector2(1, 0));
                        break;
                    case 1:
                        newScheme.scheme.Add(new Vector2(1, 0));
                        newScheme.scheme.Add(new Vector2(0, -1));
                        break;
                    case 2:
                        newScheme.scheme.Add(new Vector2(0, -1));
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        break;
                    case 3:
                        newScheme.scheme.Add(new Vector2(-1, 0));
                        newScheme.scheme.Add(new Vector2(0, 1));
                        break;
                }
                break;
            default:
                break;
        }

        moves.Add(newScheme);
    }

    public override void TickAttacks()
    {
        if (isAttacking)
        {
            actionPerformed = true;
            targets.Clear();
            foreach (Vector2 vec in attacks.ElementAt(0).scheme)
            {
                if (position.Neighbour((int)vec.x, (int)vec.y) && position.Neighbour((int)vec.x, (int)vec.y).contester)
                {
                    position.Neighbour((int)vec.x, (int)vec.y).contester.TakeDamage(str);
                    targets.Add(position.Neighbour((int)vec.x, (int)vec.y));
                }
                else
                {
                    targets.Add(position.Neighbour((int)vec.x, (int)vec.y));
                }
            }
            if (targets.Count > 0)
            {
                actionPerformed = true;
                PerformAttacks(0f);
            }
            isAttacking = false;
        }
    }

    public override void ApplyPickup(Pickup pickup)
    {

        Camera.main.GetComponent<CameraHandler>().OneShot("Click");
        switch (pickup.type)
        {
            case "Pickup_HP":
                if (currHP < maxHP)
                {
                    currHP++;
                }
                GetComponent<PlayerController>().RefreshUI();
                break;
            case "Pickup_MaxHP":
                if (maxHP < 12)
                {
                    maxHP++;
                }
                else
                {
                    if (currHP < maxHP)
                    {
                        currHP++;
                    }
                    GameObject.Find("Canvas").GetComponent<UI>().Prompt("Max HP reached!");
                }
                GetComponent<PlayerController>().RefreshUI();
                break;
            case "Pickup_STR":
                str++;
                break;
            case "Pickup_Attack":
                NewAttackScheme();
                break;
            case "Pickup_Move":
                NewMoveScheme();
                break;
        }

        base.ApplyPickup(pickup);
    }
    private void NewAttackScheme()
    {
        string newScheme = "Basic";

        newScheme = GameObject.Find("Canvas/Attack").transform.GetChild(Random.Range(2, GameObject.Find("Canvas/Attack").transform.childCount)).gameObject.name;

        if (GetComponent<PlayerController>().attacks.Exists((x)=>x.Equals(newScheme)))
        {
            GameObject.Find("Canvas").GetComponent<UI>().Prompt("Attack reinforced - "+newScheme);
        }
        else
        {
            GameObject.Find("Canvas").GetComponent<UI>().Prompt("Attack learned - "+newScheme);
        }

        GetComponent<PlayerController>().attacks.Add(newScheme);
    }
    private void NewMoveScheme()
    {
        string newScheme = "Basic";

        newScheme = GameObject.Find("Canvas/Move").transform.GetChild(Random.Range(2, GameObject.Find("Canvas/Move").transform.childCount)).gameObject.name;

        if (GetComponent<PlayerController>().moves.Exists((x) => x.Equals(newScheme)))
        {
            GameObject.Find("Canvas").GetComponent<UI>().Prompt("Move reinforced - " + newScheme);
        }
        else
        {
            GameObject.Find("Canvas").GetComponent<UI>().Prompt("Move learned - " + newScheme);
        }

        GetComponent<PlayerController>().moves.Add(newScheme);
    }

    public override void TickMoves()
    {
        if (isMoving)
        {
            base.TickMoves();
            isMoving = false;
        }
    }

    public override void Transition(float speed)
    {
        Camera.main.GetComponent<CameraHandler>().following = true;
        Camera.main.GetComponent<CameraHandler>().OneShot("Step");
        base.Transition(speed);
    }

    public override void PerformAttacks(float foo)
    {
        Camera.main.GetComponent<CameraHandler>().following = false;
        base.PerformAttacks(foo);
    }
    public override void PerformAttacks()
    {
        if (progress >= 1-progressGain)
        {
            Camera.main.GetComponent<CameraHandler>().OneShot("Slash");
        }
        base.PerformAttacks();
    }
}
