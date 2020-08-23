using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : Entity
{
    private bool damaged = false;
    public int abilityCooldown = 7;
    public int abilityStrength = 2;
    public int abilityVariation = 3;
    private int abilityRefreshesIn = 0;

    public string anim_ability;

    public override void TakeDamage(int dmg)
    {
        damaged = true;
        base.TakeDamage(dmg);
    }

    public override void TickAttacks()
    {
        abilityRefreshesIn--;
        if (!alive)
        {
            return;
        }
        actionPerformed = false;
        if (!damaged && abilityRefreshesIn <= 0)
        {
            RaiseDead();
            actionPerformed = true;
            abilityRefreshesIn = abilityCooldown-Random.Range(0, abilityVariation);
        }
        else
        {
            damaged = false;
        }
    }

    public override void Start()
    {
        base.Start();
        abilityRefreshesIn = Random.Range(0, abilityVariation);
    }

    public void RaiseDead()
    {
        int spawned = 0;
        int shift = Random.Range(0, 3);
        for(int x = 0; x<3; x++)
        {
            for(int y = 0; y<3; y++)
            {
                int actualX = ((x + shift) % 3) - 1;
                int actualY = ((y + shift) % 3) - 1;
                if (position.Neighbour(actualX, actualY) && !position.Neighbour(actualX, actualY).obstructed && !position.Neighbour(actualX, actualY).contester)
                {
                    position.Neighbour(actualX, actualY).contester = Instantiate(Resources.Load("Prefabs/Enemies/Skeleton") as GameObject, GameObject.Find("Arena/Entities").transform).GetComponent<Entity>();
                    position.Neighbour(actualX, actualY).contester.actionPerformed = true;
                    position.Neighbour(actualX, actualY).contester.position = position.Neighbour(actualX, actualY);
                    position.Neighbour(actualX, actualY).contester.transform.position = position.Neighbour(actualX, actualY).transform.position;
                    spawned++;
                    if (spawned >= abilityStrength)
                    {
                        GetComponent<Animator>().Play(anim_ability);
                        return;
                    }
                }
            }
        }
        if (spawned > 0)
        {
            GetComponent<Animator>().Play(anim_ability);
        }
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

        ////Basic Movement
        //newScheme = new Scheme();
        //newScheme.scheme.Add(new Vector2(1, 1));
        //attacks.Add(newScheme);

        //newScheme = new Scheme();
        //newScheme.scheme.Add(new Vector2(-1, 1));
        //attacks.Add(newScheme);

        //newScheme = new Scheme();
        //newScheme.scheme.Add(new Vector2(-1, -1));
        //attacks.Add(newScheme);

        //newScheme = new Scheme();
        //newScheme.scheme.Add(new Vector2(1, -1));
        //attacks.Add(newScheme);
    }
}
