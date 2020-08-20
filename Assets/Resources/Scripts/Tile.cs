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
        if (x < GameObject.Find("Arena").GetComponent<Logic>().mapSize - 1)
            return GameObject.Find("Arena").GetComponent<Logic>().grid[x, y+1];
        return null;
    }
    public Tile Bottom()
    {
        if (y>0)
            return GameObject.Find("Arena").GetComponent<Logic>().grid[x, y-1];
        return null;
    }
}
