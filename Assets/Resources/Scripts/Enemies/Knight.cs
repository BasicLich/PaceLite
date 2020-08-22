using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Entity
{
    public bool invincible = true;
    public bool awakeAble = false;
    public string anim_awake = "Awake_Knight";

    public override void TakeDamage(int dmg)
    {
        if (!invincible)
        {
            base.TakeDamage(dmg);
        }
        else if (awakeAble)
        {
            WakeUp();
            currHP = 1;
            alive = true;
        }
    }
    public override void Die(string way)
    {
        invincible = true;
        GetComponent<Animator>().Play(anim_death);
    }
    public override void TickMoves()
    {
        if (!invincible)
        {
            base.TickMoves();
        }
    }
    public override void TickAttacks()
    {
        if (!invincible)
        {
            base.TickAttacks();
        }
    }

    public void WakeUp()
    {
        invincible = false;
        awakeAble = true;
        GetComponent<Animator>().Play(anim_awake);
    }

    public override void SetSchemes()
    {
        //Basic Movement
        Scheme newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(1, 1));
        moves.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(-1, 1));
        moves.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(-1, -1));
        moves.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(1, -1));
        moves.Add(newScheme);

        //Basic Attack
        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(0, 1));
        newScheme.scheme.Add(new Vector2(0, 1));
        attacks.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(0, 1));
        newScheme.scheme.Add(new Vector2(1, 0));
        attacks.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(0, 1));
        newScheme.scheme.Add(new Vector2(0, -1));
        attacks.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(0, 1));
        newScheme.scheme.Add(new Vector2(-1, 0));
        attacks.Add(newScheme);
        //newScheme = new Scheme();
        //newScheme.scheme.Add(new Vector2(0, 1));
        //newScheme.scheme.Add(new Vector2(1, 2));
        //newScheme.scheme.Add(new Vector2(-1, 2));
        //attacks.Add(newScheme);

        //newScheme = new Scheme();
        //newScheme.scheme.Add(new Vector2(1, 0));
        //newScheme.scheme.Add(new Vector2(2, 1));
        //newScheme.scheme.Add(new Vector2(2, -1));
        //attacks.Add(newScheme);

        //newScheme = new Scheme();
        //newScheme.scheme.Add(new Vector2(0, -1));
        //newScheme.scheme.Add(new Vector2(1, -2));
        //newScheme.scheme.Add(new Vector2(-1, -2));
        //attacks.Add(newScheme);

        //newScheme = new Scheme();
        //newScheme.scheme.Add(new Vector2(-1, 0));
        //newScheme.scheme.Add(new Vector2(-2, 1));
        //newScheme.scheme.Add(new Vector2(-2, -1));
        //attacks.Add(newScheme);
    }
}
