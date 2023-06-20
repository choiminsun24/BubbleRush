using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float ATK;

    public void setATK(float ATK)
    {
        this.ATK = ATK;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
            transform.position = Vector2.MoveTowards(transform.position, enemy.position, 0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Bubble>().takeDamage(ATK);
            Destroy(gameObject);
        }
    }
}
