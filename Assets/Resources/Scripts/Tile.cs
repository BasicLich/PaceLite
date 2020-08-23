using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile: MonoBehaviour
{
    public Entity contester;
    public Pickup pickup;
    public int x;
    public int y;

    public bool obstructed;

    void Start()
    {
        transform.Find("Water").GetComponent<SpriteRenderer>().flipX = Random.Range(0, 1f) > 0.5f;
        transform.Find("Water").GetComponent<SpriteRenderer>().flipY = Random.Range(0, 1f) > 0.5f;
    }

    public Tile Neighbour(int xDir, int yDir)
    {
        if (x+xDir <= GameObject.Find("Arena").GetComponent<Logic>().mapSize - 1 && x+xDir >= 0 && y+yDir <= GameObject.Find("Arena").GetComponent<Logic>().mapSize - 1 && y+yDir >= 0 && (xDir!=0 || yDir!=0))
        {
            return GameObject.Find("Arena").GetComponent<Logic>().grid[x + xDir, y + yDir];
        }
        return null;
    }

    public Tile Left()
    {
        if (x > 0) 
            return GameObject.Find("Arena").GetComponent<Logic>().grid[x - 1, y];
        return null;
    }
    public Tile Right()
    {
        if (x < GameObject.Find("Arena").GetComponent<Logic>().mapSize-1)
            return GameObject.Find("Arena").GetComponent<Logic>().grid[x + 1, y];
        return null;
    }
    public Tile Top()
    {
        if (y < GameObject.Find("Arena").GetComponent<Logic>().mapSize - 1)
            return GameObject.Find("Arena").GetComponent<Logic>().grid[x, y+1];
        return null;
    }
    public Tile Bottom()
    {
        if (y>0)
            return GameObject.Find("Arena").GetComponent<Logic>().grid[x, y-1];
        return null;
    }

    public void Splatter(float sidesplatters = 0.5f, float strength = 0.65f)
    {
        if (gameObject.tag != "Submerged")
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, Color.red, strength);

            if (Left() && Random.Range(0, 1f) < sidesplatters)
            {
                Left().Splatter(0f, strength / 2);
            }
            if (Right() && Random.Range(0, 1f) < sidesplatters)
            {
                Right().Splatter(0f, strength / 2);
            }
            if (Top() && Random.Range(0, 1f) < sidesplatters)
            {
                Top().Splatter(0f, strength / 2);
            }
            if (Bottom() && Random.Range(0, 1f) < sidesplatters)
            {
                Bottom().Splatter(0f, strength / 2);
            }
        }
    }
    public void Submerge(float progress)
    {
        if (pickup)
        {
            pickup.Pick();
        }

        if (!GameObject.Find("Arena").GetComponent<Logic>().tutorial)
        {
            Color newColor = transform.Find("Water").GetComponent<SpriteRenderer>().color;
            newColor.a = progress;
            transform.Find("Water").GetComponent<SpriteRenderer>().color = newColor;
        }
    }
}
