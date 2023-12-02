using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int towerCategory = -1;
    private float speed = 250f;
    public enum Expression
    {
        MAD = 0,
        SMILE = 1,
        EXPRESSIONLESS = 2
    };

    private float ATK;
    private Expression expression;

    public void setBullet(float ATK, int expression)
    {
        this.ATK = ATK;
        this.expression = (Expression)expression;
        
    }
    private Transform enemy = null;
    public void TriggerMove(Transform _enemy)
    {
        enemy = _enemy;
    }
    // Update is called once per frame
    void Update()
    {
        if(enemy)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemy.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Enemy bubble = other.gameObject.GetComponent<Enemy>();
            if(bubble)
            {
                bubble.takeDamage(ATK, (int)bubble.expression, gameObject, towerCategory);
            }
        }
    }
}
