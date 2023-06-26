using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStage : MonoBehaviour
{
    //world 
    public Material nonBlur;
    public Material blur;
    public SpriteRenderer background;

    public Animator anim;

    public void SelectWorld(int num)
    {
        switch (num) //선택 월드에 따른 애니메이션
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
        Invoke("OpenSelectStage", 0.8f);
    }

    public void OpenSelectStage()
    {
        background.material = blur;
    }
}
