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
        Camera.main.GetComponent<CameraHandler>().Shake(0.25f, 0.1f);
        GetComponent<PlayerController>().RefreshUI();
    }
    public override void Die(string way = "Regular")
    {
        GetComponent<PlayerController>().RefreshUI();
        GameObject.Find("Arena").GetComponent<Logic>().SetPaused(true);
        GameObject.Find("Canvas").GetComponent<UI>().DeathDisplay();
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
        switch (pickup.type)
        {
            case "Pickup_HP":
                if (currHP < maxHP)
                {
                    currHP++;
                }
                break;
            case "Pickup_MaxHP":
                if (maxHP < 12)
                {
                    maxHP++;
                }
                else
                {
                    GameObject.Find("Canvas").GetComponent<UI>().Prompt("Max HP reached!");
                }
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

        newScheme = GameObject.Find("Canvas/Attack").transform.GetChild(Random.Range(1, GameObject.Find("Canvas/Attack").transform.childCount)).gameObject.name;

        if (GetComponent<PlayerController>().moves.Exists((x)=>x.Equals(newScheme)))
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

    }

    public override void TickMoves()
    {
        if (isMoving)
        {
            base.TickMoves();
            isMoving = false;
        }
    }
}
