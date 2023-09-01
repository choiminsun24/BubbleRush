using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower0Guage : MonoBehaviour
{
    [SerializeField] private Transform origin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(origin.rotation.x, origin.rotation.y, -origin.rotation.z);
    }
}
