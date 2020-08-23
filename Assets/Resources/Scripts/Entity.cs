using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected List<Scheme> moves = new List<Scheme>();
    protected List<Scheme> attacks = new List<Scheme>();

    public Tile position;

    public string anim_death = "Death_Animal";
    public string anim_move = "Move_Animal";
    public string anim_drown = "Drown_Animal";
    public string anim_spawn = "Spawn_Drop";

    protected List<Tile> path = new List<Tile>();
    protected List<Tile> targets = new List<Tile>();
    protected float progress = 0;
    protected float secondsPerMove = 0.5f;
    protected float secondsPerAttack = 0.2f;
    protected float progressGain;
    protected float attackDelay;

    public int currHP;
    public int maxHP;
    public int str;

    public bool bleeding = true;

    public bool friendlyFiring;
    private int direction = 0;
    public  bool actionPerformed = false;
    public bool alive = true;
    public bool stationary = false;
    private bool pathBlocked = false;
    private bool enemyInRange = false;

    public virtual void TakeDamage(int dmg)
    {
        if (bleeding)
        {
            position.Splatter();
        }

        if (currHP > dmg)
        {
            currHP -= dmg;
        }
        else
        {
            currHP = 0;
            alive = false;
            Die();
        }
    }

    public virtual void Die(string way = "Regular")
    {
        CancelInvoke();
        position.contester = null;
        if (way.Equals("Regular"))
        {
            GetComponent<Animator>().Play(anim_death);
        }
        else
        {
            GetComponent<Animator>().Play(anim_drown);
        }
        InvokeRepeating("Invoke_Death", 1f, 0.1f);
    }
    private void Invoke_Death()
    {
        CancelInvoke();
        Destroy(gameObject);
    }

    public virtual void Start()
    {
        SetSchemes();
        GetComponent<Animator>().Play(anim_spawn);
    }

    public virtual void SetSchemes()
    {

    }

    public virtual void TickAttacks()
    {
        if (!alive)
        {
            return;
        }
        enemyInRange = false;
        actionPerformed = false;
        attackDelay = 0f;

        targets.Clear();
        for (int i = 0; i < 4; i++)
        {
            targets.Clear();
            foreach (Vector2 vec in attacks.ElementAt((direction + i) % 4).scheme)
            {
                if (position.Neighbour((int)vec.x, (int)vec.y))
                {
                    if (position.Neighbour((int)vec.x, (int)vec.y).contester && (position.Neighbour((int)vec.x, (int)vec.y).contester.GetComponent<Player>() || friendlyFiring))
                    {
                        targets.Add(position.Neighbour((int)vec.x, (int)vec.y));
                        attackDelay = position.Neighbour((int)vec.x, (int)vec.y).contester.path.Count > 1 ? secondsPerMove : 0f;
                        enemyInRange = true;
                    }
                    else
                    {
                        targets.Add(position.Neighbour((int)vec.x, (int)vec.y));
                    }
                }
            }
            if (targets.Count > 0)
            {
                if (enemyInRange)
                {
                    actionPerformed = true;
                    PerformAttacks(0f);
                    break;
                }
            }
        }
    }
    public virtual void TickMoves()
    {
        if (!actionPerformed)
        {
            path.Clear();
            path.Add(position);
            pathBlocked = false;
            if (moves.Count == 0)
            {
                Debug.Log("here");
            }
            Scheme scheme = moves.ElementAt((int)Random.Range(0, (float)moves.Count));



            //movement
            if (!enemyInRange)
            {
                foreach (Vector2 vec in scheme.scheme)
                {
                    Tile nextPos = position.Neighbour((int)vec.x, (int)vec.y);
                    if (vec.x == 1 && vec.y == 0)
                    {
                        direction = 1;
                    }
                    else if (vec.x == 0 && vec.y == 1)
                    {
                        direction = 0;
                    }
                    else if (vec.x == -1 && vec.y == 0)
                    {
                        direction = 3;
                    }
                    else if (vec.x == 0 && vec.y == -1)
                    {
                        direction = 2;
                    }

                    if (nextPos && !nextPos.obstructed && !nextPos.contester)
                    {
                        Move(nextPos);
                    }
                    else if(nextPos)
                    {
                        Move(nextPos, true);
                        Move(position, true);
                        break;
                    }
                }
                if (path.Count > 1)
                {
                    actionPerformed = true;
                    Transition(0f);
                }
            }
        }
    }

    public void Move(Tile newPos, bool blocked = false)
    {
        GetComponent<Animator>().Play(anim_move);
        if (!blocked)
        {
            position.contester = null;
            position = newPos;
            newPos.contester = this;
        }
        else
        {
            pathBlocked = true;
        }
        path.Add(newPos);
    }

    public virtual void Transition(float speed)
    {
        CancelInvoke();

        progressGain = secondsPerMove / 30f;
        progress = 0;

        InvokeRepeating("Transition", 0, secondsPerMove / 30f / path.Count);

    }
    private void Transition()
    {
        progress += progressGain;
        if (path.Count>1 && !pathBlocked)
        {
            transform.position = Vector2.Lerp(path.ElementAt(0).transform.position, path.ElementAt(1).transform.position, progress);
        }
        else if(path.Count == 3 && pathBlocked)
        {
            transform.position = Vector2.Lerp(path.ElementAt(0).transform.position, path.ElementAt(1).transform.position, progress/4f);
        }
        else if(path.Count == 2 && pathBlocked)
        {
            transform.position = Vector2.Lerp(path.ElementAt(0).transform.position, path.ElementAt(1).transform.position, 3f / 4 + progress / 4f);
        }
        else
        {
            transform.position = Vector2.Lerp(path.ElementAt(0).transform.position, path.ElementAt(1).transform.position, progress);
        }

        if (progress >= 1f)
        {
            if (path.ElementAt(1).pickup)
            {
                if (!(pathBlocked && path.Count == 3))
                {
                    ApplyPickup(path.ElementAt(1).pickup);
                }
            }
            path.RemoveAt(0);
            progress -= 1f;
            if (path.Count < 2)
            {
                
                CancelInvoke();
            }
        }
    }

    public virtual void ApplyPickup(Pickup pickup)
    {
        pickup.Pick();
    }

    public virtual void PerformAttacks(float foo)
    {
        CancelInvoke();

        progressGain = 0.1f;
        progress = 0;

        InvokeRepeating("PerformAttacks", attackDelay, secondsPerAttack / 30f / targets.Count);

    }
    public virtual void PerformAttacks()
    {
        progress += progressGain;
        transform.position = Vector2.Lerp(position.transform.position, targets.ElementAt(0).transform.position, progress/3);

        if (progress >= 1f)
        {
            if (targets.First().contester && targets.First().contester.GetComponent<Player>())
            {
                targets.First().contester.TakeDamage(str);
            }
            Instantiate(Resources.Load("Prefabs/Slash") as GameObject, GameObject.Find("Arena/Effects").transform).GetComponent<Slash>().Launch(targets.ElementAt(0).transform.position, str, tag == "Player");
            targets.RemoveAt(0);
            progress -= 1f;
            if (targets.Count==0)
            {
                transform.position = position.transform.position;
                CancelInvoke();
            }
        }
    }
}
