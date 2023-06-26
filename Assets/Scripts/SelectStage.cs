using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStage : MonoBehaviour
{
    //world 
    public Animator anim;

    public void SelectWorld(int num)
    {
        switch (num)
        {
            case 1:
                anim.SetInteger("num", 1);
                break;
            case 2:
                anim.SetInteger("num", 2);
                break;
            case 3:
                anim.SetInteger("num", 3);
                break;
            case 4:
                anim.SetInteger("num", 4);
                break;
            default:
                anim.SetInteger("num", 0);
                break;
        }
    }
}
