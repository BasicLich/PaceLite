using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Entity
{
    public override void SetSchemes()
    {
        //Basic Movement
        Scheme newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(1, 0));
        newScheme.scheme.Add(new Vector2(1, 0));
        moves.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(0, 1));
        newScheme.scheme.Add(new Vector2(0, 1));
        moves.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(-1, 0));
        newScheme.scheme.Add(new Vector2(-1, 0));
        moves.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(0, -1));
        newScheme.scheme.Add(new Vector2(0, -1));
        moves.Add(newScheme);

        //Basic Attack
        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(0, 1));
        attacks.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(1, 0));
        attacks.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(0, -1));
        attacks.Add(newScheme);

        newScheme = new Scheme();
        newScheme.scheme.Add(new Vector2(-1, 0));
        attacks.Add(newScheme);
    }
}
