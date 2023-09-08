using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    //������
    public InGameData data;
    private List<Dictionary<string, string>> textData;

    //UI ����
    public GameObject Box; //UIâ
    public QuestCard[] position; //���� ��ġ
    public GameObject my; //���õ� ī�� Panel
    public QuestCard mine; //���õ� ī��

    public UIManager ui;

    //������
    private int[] num; //���õ� ��ȣ

    //���α׷�

    public void Awake()
    {
        textData = ExelReader.Read("Data/inGame/QuestTest"); //���� ������ �޾ƿ���

        //GameManager Start���� ������
        Box.SetActive(false);
        my.SetActive(false);
    }

    //������ on
    public void play()
    {
        Box.SetActive(true);
        ui.Blind();

        //1. �������� �� �̱�
        num = new int[] { -1, -1, -1 };

        for (int i = 0; i < num.Length; i++)
        {
            //����
            num[i] = UnityEngine.Random.Range(0, textData.Count);

            //�ߺ� �˻�
            for (int j = 0; j < i; j++)
            {
                if (num[j] == num[i])
                {
                    i--;
                    break;
                }
            }
        }

        //2. ī�� UI ����
        for (int i = 0; i < num.Length; i++)
        {
            position[i].cardSetting(textData[num[i]]);
        }
    }

    //������ ī��, ������ ����
    private void cardSetting(Transform tf, Dictionary<string, string> target)
    {
        tf.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(target["Directory"]);
        tf.GetChild(2).GetComponent<Text>().text = target["Name"]; //Title
        tf.GetChild(3).GetComponent<Text>().text = target["QuestDescription"]; //Content
        tf.GetChild(4).GetComponent<Text>().text = target["RewardDescription"]; //Content
    }

    //���� �� ó��
    public void choice(int n) //ī�� ���� �� ����� �޼ҵ�
    {
        Box.SetActive(false); //���� â ����
        ui.Blind();

        Dictionary<string, string> choice = textData[num[n]]; //���õ� ��
        mine.cardSetting(choice);


        //���� ���� ȿ�� **************************�� ���� ������***********************
        if (!choice["QuestTarget"].Equals("null")) //����Ʈ ��� ����
        {
            Debug.Log(choice["QuestTarget"] + "�� ���� ����Ʈ�� ����˴ϴ�: ���� ���� ����");
        }

        if (!choice["RewardTarget"].Equals("null")) //����� ����� �����ϸ�
        {
            Debug.Log(choice["RewardTarget"] + "�� ���� ����Ʈ�� ����˴ϴ�: ���� ���� ����");
        }

        //data.BuffATKS(1.06f); ���ù�
    }

    public void watchQuest()
    {
        if (my.activeSelf == true)
            SoundManager.Instance.popCloseSound();

        ui.Blind();
        my.SetActive(!my.activeSelf);
    }

}
