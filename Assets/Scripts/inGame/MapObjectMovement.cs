using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectMovement : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = Random.Range(0.8f, 1.2f);
    }
}
