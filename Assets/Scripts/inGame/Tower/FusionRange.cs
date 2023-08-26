using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionRange : MonoBehaviour
{
    public bool canFuse = false;
    public GameObject targetTower;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Tower")
        {
            canFuse = true;
            targetTower = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Tower")
        {
            canFuse = false;
            targetTower = null;
        }
    }
}
