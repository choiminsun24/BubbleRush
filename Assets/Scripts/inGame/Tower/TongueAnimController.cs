using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueAnimController : MonoBehaviour
{
    [SerializeField] private float skillTime = 1f;
    private float time = 0f;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        time+=Time.deltaTime;
        if(time >= 1f)
        {
            anim.SetTrigger("isAttack_0");
            time = 0f;
        }
        // if(time >= 2.5f)
        // {
        //     anim.SetBool("isAttack", false);
        //     time = 0f;
        // }
    }
}
