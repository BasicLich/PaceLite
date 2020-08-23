using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : Entity
{
    public override void SetSchemes()
    {
        //Basic Movement
        Scheme newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(1, 1));
        moves.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(-1, -1));
        moves.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(-1, 1));
        moves.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(1, -1));
        moves.Add(newScheme);

        //Basic Attack
        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(-1, 1));
        newScheme.scheme.Add(new Vector2(-1, 1));
        attacks.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(1, -1));
        newScheme.scheme.Add(new Vector2(1, 1));
        attacks.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(-1, -1));
        newScheme.scheme.Add(new Vector2(1, -1));
        attacks.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(-1, -1));
        newScheme.scheme.Add(new Vector2(-1, 1));
        attacks.Add(newScheme);
    }
}
