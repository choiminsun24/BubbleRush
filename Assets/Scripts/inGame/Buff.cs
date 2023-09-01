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
    public Transform canvas; //�Ҽӵ� Canvas
    public GameObject Box; //UIâ
    public Transform[] position; //���� ��ġ
    public Sprite[] images;
    public GameObject[] effect;

    public GameObject my;

    //������
    private int[] num; //���õ� ��ȣ
    private int mineNum = 0;

    //���õ�
    public Transform[] mine;

    //���α׷�
    private static Buff instance;

    public static Buff Instance
    {
        get
        {
            if (!instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void Awake()
    {
        instance = this; //�������� ������ �ϳ��� ���.

        textData = ExelReader.Read("Data/inGame/BuffTable"); //���� ������ �޾ƿ���
    }

    public void Start()
    {
        Box.SetActive(false);
        my.SetActive(false);

        //buffNum�� textData �� ��ȣ(�迭 �ε���)�� �ʱ�ȭ
        for (int i = 0; i < textData.Count; i++)
        {
            buffNum.Add(i);
        }

        //play();
    }

    //������ on
    public void play()
    {
        Box.SetActive(true);

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
            Transform tf = position[i];

            cardSetting(tf, textData[num[i]]);
        }
    }

    //������ ī��, ������ ����
    private void cardSetting(Transform tf, Dictionary<string, string> target)
    {
        //ī�� ������
        if (target["Type"].Equals("NatureBless")) //���� ī��
        {
            tf.GetChild(1).GetComponent<Image>().sprite = images[0];
            GameObject game = Instantiate(effect[0], tf.GetChild(4).position, Quaternion.identity, tf);

        }
        else if (target["Type"].Equals("DarknessCurse")) //����� ī��
        {
            tf.GetChild(1).GetComponent<Image>().sprite = images[1];
            GameObject game = Instantiate(effect[1], tf.GetChild(4).position, Quaternion.identity, tf);
        }
        else //������ ī��
        {
            tf.GetChild(1).GetComponent<Image>().sprite = images[2];
            GameObject game = Instantiate(effect[2], tf.GetChild(4).position, Quaternion.identity, tf);
        }

        tf.GetChild(2).GetComponent<Text>().text = target["Name"]; //Title
        tf.GetChild(3).GetComponent<Text>().text = target["Description"]; //Content
        tf.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(target["Directory"]);
    }

    //���� �� ó��
    public void choice(int n) //ī�� ���� �� ����� �޼ҵ�
    {
        Box.SetActive(false); //���� â ����
        buffNum.RemoveAt(num[n]); //���� �ѿ��� ���� ��ȣ ����. -> ������ ������ �ʵ��� ��.
        Dictionary<string, string> choice = textData[num[n]]; //���õ� ��
        cardSetting(mine[mineNum], choice);


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

    public void watchChoice()
    {
        my.SetActive(!my.activeSelf);
    }
}