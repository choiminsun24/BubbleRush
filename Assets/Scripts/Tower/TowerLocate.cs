using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLocate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Tower" && other.GetComponent<DragTower>() != null)
        {
            other.GetComponent<DragTower>().nextLocation = transform;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Tower" && other.GetComponent<DragTower>() != null)
        {
            other.GetComponent<DragTower>().nextLocation = null;
        }
    }
}
