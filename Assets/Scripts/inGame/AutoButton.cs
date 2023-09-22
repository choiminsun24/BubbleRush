using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoButton : MonoBehaviour
{
    public Sprite onButton;
    public Sprite onText;
    public Sprite offButton;
    public Sprite offText;

    private Animator anim;
    private Image img;
    public Image txt;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        img = gameObject.GetComponent<Image>();
    }

    public void changeAuto()
    {
        Debug.Log("auto: " + GameManager.Instance.getAuto());

        if (GameManager.Instance.getAuto() == true)
        {
            autoOff();
            GameManager.Instance.autoOff();
        }
        else
        {
            autoOn();
            GameManager.Instance.autoOn();
        }
    }

    private void autoOff()
    {
        anim.SetBool("off", true);
        img.sprite = offButton;
        txt.sprite = offText;
    }

    private void autoOn()
    {
        anim.SetBool("off", false);
        img.sprite = onButton;
        txt.sprite = onText;
    }
}
