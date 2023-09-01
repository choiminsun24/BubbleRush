using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    [SerializeField] private TowerController tc;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Enemy>().takeDamage(10, 1, null);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
