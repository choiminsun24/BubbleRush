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

    //������ ī��, ������ ����
    public void cardSetting(Dictionary<string, string> target)
    {
        //ī�� ������
        if (target["Type"].Equals("NatureBless")) //���� ī��
        {
            frame.sprite = frameList[0];
            effectList[0].SetActive(true);

        }
        else if (target["Type"].Equals("DarknessCurse")) //����� ī��
        {
            frame.sprite = frameList[1];
            effectList[1].SetActive(true);
        }
        else //������ ī��
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
