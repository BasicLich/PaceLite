using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public string type;
    public Tile position;

    public void Pick()
    {
        position.pickup = null;
        GetComponent<Animator>().Play("Applied_Pickup");
        InvokeRepeating("Invoke_Pickup", 0.5f, 0.1f);
    }
    private void Invoke_Pickup()
    {
        GameObject.Destroy(gameObject);
    }
}
