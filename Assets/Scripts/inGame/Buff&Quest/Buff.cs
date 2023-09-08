using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    //������
    public InGameData data;
    private List<Dictionary<string, string>> textData;
    private List<int> buffNum = new List<int>(); //�� ������ ��ȣ ����.

    //UI ����
    public GameObject Box; //UIâ
    public BuffCard[] position; //���� ��ġ
    public Sprite[] images;
    public GameObject[] effect;

    public UIManager ui;

    public GameObject my;

    //������
    private int[] num; //���õ� ��ȣ
    private int mineNum = 0;

    //���õ�
    public BuffCard[] mine;

    //����Ʈ
    private GameObject[] effec;
    int effectNum = 0;

    //���α׷�
    public void Awake()
    {
        textData = ExelReader.Read("Data/inGame/BuffTable"); //���� ������ �޾ƿ���

        Box.SetActive(false);
        my.SetActive(false);

        //buffNum�� textData �� ��ȣ(�迭 �ε���)�� �ʱ�ȭ
        for (int i = 0; i < textData.Count; i++)
        {
            buffNum.Add(i);
        }
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
            num[i] = UnityEngine.Random.Range(0, buffNum.Count);

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

    ////������ ī��, ������ ����
    //private void cardSetting(Transform tf, Dictionary<string, string> target)
    //{
    //    //ī�� ������
    //    if (target["Type"].Equals("NatureBless")) //���� ī��
    //    {
    //        tf.GetChild(1).GetComponent<Image>().sprite = images[0];
    //        effect[effectNum] = Instantiate(effect[0], tf.GetChild(3).position, Quaternion.identity);

    //    }
    //    else if (target["Type"].Equals("DarknessCurse")) //����� ī��
    //    {
    //        tf.GetChild(1).GetComponent<Image>().sprite = images[1];
    //        GameObject game = Instantiate(effect[1], tf.GetChild(3).position, Quaternion.identity);
    //    }
    //    else //������ ī��
    //    {
    //        tf.GetChild(1).GetComponent<Image>().sprite = images[2];
    //        GameObject game = Instantiate(effect[2], tf.GetChild(3).position, Quaternion.identity);
    //    }

    //    tf.GetChild(2).GetComponent<Text>().text = target["Name"]; //Title
    //    tf.GetChild(3).GetComponent<Text>().text = target["Description"]; //Content
    //    tf.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(target["Directory"]);
    //}

    //���� �� ó��
    public void choice(int n) //ī�� ���� �� ����� �޼ҵ�
    {
        Debug.Log("ī�尡 ���õ�");
        Box.SetActive(false); //���� â ����
        ui.Blind();

        buffNum.RemoveAt(num[n]); //���� �ѿ��� ���� ��ȣ ����. -> ������ ������ �ʵ��� ��.
        Dictionary<string, string> choice = textData[num[n]]; //���õ� ��
        mine[mineNum%3].cardSetting(choice);
        mineNum++;
        Debug.Log("mineNum: " + mineNum);


        //���� ���� ȿ�� **************************�� ���� ������***********************
        if (!choice["NatureBlessTargetTower"].Equals("null")) //���� ����� �����ϸ�
        {
            Debug.Log("������ ����˴ϴ�: ���� ���� ����");
        }

        if (!choice["DarknessCurseTargetTower"].Equals("null")) //����� ����� �����ϸ�
        {
            Debug.Log("������� ����˴ϴ�: ���� ���� ����");
        }

        if (!choice["RewardTarget"].Equals("null")) //������ ����� �����ϸ�
        {
            Debug.Log("�����尡 ����˴ϴ�: ���� ���� ����");
        }

        //data.BuffATKS(1.06f); ���ù�
    }

    public void watchBuff()
    {
        if (my.activeSelf == true)
            SoundManager.Instance.popCloseSound();

        ui.Blind();
        my.SetActive(!my.activeSelf);
    }
}