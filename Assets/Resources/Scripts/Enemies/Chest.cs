using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Entity
{
    public bool advancedChest;
    public override void Die(string way = "Regular")
    {
        if (way.Equals("Regular"))
        {
            string type = "HP";
            if (!advancedChest)
            {
                switch ((int)Random.Range(0, 2f)*2)
                {
                    case 0:
                        type = "MaxHP";
                        break;
                    default:
                        type = "HP";
                        break;
                }
            }
            else
            {
                switch ((int)Random.Range(0, 2f)*2)
                {
                    case 0:
                        type = "STR";
                        break;
                    case 1:
                        type = "Move";
                        break;
                    default:
                        type = "Attack";
                        break;
                }
            }
            position.pickup = Instantiate(Resources.Load("Prefabs/Pickups/Pickup_" + type) as GameObject, GameObject.Find("Arena/Pickups").transform).GetComponent<Pickup>();
            position.pickup.transform.position = position.transform.position; //I love this line of code so much
            position.pickup.position = position; //this one is fun too
        }

        base.Die(way);
    }

    public override void TickAttacks()
    {

    }
    public override void TickMoves()
    {

    }
}
