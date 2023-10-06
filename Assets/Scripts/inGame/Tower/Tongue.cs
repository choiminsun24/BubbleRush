using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    int atk;
    int expression = 1;

    private void atkLoad()
    {
        atk = 10;
    }

    [SerializeField] private TowerController tc;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            atkLoad();
            other.gameObject.GetComponent<Enemy>().takeDamage(atk, expression, null);
        }
    }
}
