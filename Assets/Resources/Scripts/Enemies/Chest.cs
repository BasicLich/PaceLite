using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Entity
{
    public bool advancedChest;
    public bool startingChest = false;
    public override void Die(string way = "Regular")
    {
        if (startingChest)
        {
            Camera.main.GetComponent<CameraHandler>().ChangeMusic("Game");
            GameObject.Find("Arena").GetComponent<Logic>().tutorial = false;
            position.Left().pickup = Instantiate(Resources.Load("Prefabs/Pickups/Pickup_Move") as GameObject, GameObject.Find("Arena/Pickups").transform).GetComponent<Pickup>();
            position.Left().pickup.transform.position = position.Left().transform.position;
            position.Left().pickup.position = position;
            position.Right().pickup = Instantiate(Resources.Load("Prefabs/Pickups/Pickup_Attack") as GameObject, GameObject.Find("Arena/Pickups").transform).GetComponent<Pickup>();
            position.Right().pickup.transform.position = position.Right().transform.position;
            position.Right().pickup.position = position;
            base.Die(way);
            return;
        }
        if (way.Equals("Regular"))
        {
            string type = "HP";
            if (!advancedChest)
            {
                if (Random.Range(0, 1f) > 0.5f)
                {
                    type = "MaxHP";
                }
                else
                {
                    type = "HP";
                }
            }
            else
            {
                if(Random.Range(0, 1f) > 0.5f)
                {
                    type = "Move";
                }
                else
                {
                    type = "Attack";
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
