using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStage : MonoBehaviour
{
    //world 
    public Material nonBlur;
    public Material blur;
    public SpriteRenderer background;
    public GameObject stagePanel;

    public Animator anim;

    public void SelectWorld(int num)
    {
        switch (num) //선택 월드에 따른 애니메이션
        {
            case 1:
                anim.SetInteger("num", 1);
                Invoke("OpenSelectStage", 0.8f);
                break;
            case 2:
                anim.SetInteger("num", 2);
                Invoke("OpenSelectStage", 0.8f);
                break;
            case 3:
                anim.SetInteger("num", 3);
                Invoke("OpenSelectStage", 0.8f);
                break;
            case 4:
                anim.SetInteger("num", 4);
                Invoke("OpenSelectStage", 0.8f);
                break;
            default:
                anim.SetInteger("num", 0);
                Invoke("CloseSelectStage", 0.8f);
                break;
        }
    }

    public void OpenSelectStage()
    {
        stagePanel.SetActive(true);
        background.material = blur;
    }

    public void CloseSelectStage()
    {
        stagePanel.SetActive(false);
        background.material = nonBlur;
    }
}
