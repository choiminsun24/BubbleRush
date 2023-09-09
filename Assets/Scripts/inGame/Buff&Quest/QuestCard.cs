using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestCard : MonoBehaviour
{
    public Image image;
    public Text name;
    public Text quest;
    public Text reward;

    //������ ī��, ������ ����
    public void cardSetting(Dictionary<string, string> target)
    {
        image.sprite = Resources.Load<Sprite>(target["Directory"]);
        name.text = target["Name"]; //Title
        quest.text = target["QuestDescription"]; //Content
        reward.text = target["RewardDescription"]; //Content
    }
}
