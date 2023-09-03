using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffCard : MonoBehaviour
{
    public Image image;
    public Image frame;
    public Text Name;
    public Text Content;
    public Sprite[] frameList;
    public GameObject[] effectList;

    //세팅할 카드, 세팅할 정보
    public void cardSetting(Dictionary<string, string> target)
    {
        //카드 프레임
        if (target["Type"].Equals("NatureBless")) //버프 카드
        {
            frame.sprite = frameList[0];
            effectList[0].SetActive(true);

        }
        else if (target["Type"].Equals("DarknessCurse")) //디버프 카드
        {
            frame.sprite = frameList[1];
            effectList[1].SetActive(true);
        }
        else //리워드 카드
        {
            frame.sprite = frameList[2];
            effectList[2].SetActive(true);
        }

        Name.text = target["Name"]; //Title
        Content.text = target["Description"]; //Content
        image.sprite = Resources.Load<Sprite>(target["Directory"]);
    }

    public void cardReset()
    {
        for (int i = 0; i < effectList.Length; i++)
        {
            effectList[i].SetActive(false);
        }
    }
}
