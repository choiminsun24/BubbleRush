using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] private TowerController tc;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(tc.nearestEnemy != other.transform)
            {
                return;
            }
            
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                //enemy.takeDamage(tc.data.attack, (int)enemy.expression, null);
            }
        }
    }
}
