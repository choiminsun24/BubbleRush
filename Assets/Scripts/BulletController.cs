using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
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
        if(Vector2.Distance(transform.position, enemy.position) < 0.1f)
        {
            Destroy(this.gameObject);
        }
    }
}
