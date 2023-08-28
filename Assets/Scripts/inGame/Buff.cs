using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    /*
     * �ʿ��� �� 
     * UI
     *  - ���� ���� <> �׵θ�
     *  - ���� �̹��� <> �̹���
     *  - Name <> ���� Ÿ��Ʋ
     *  - ���� Content <> ���� content
     *  
     *���� ó��
     *  - ���� ���� <> ���� �޼���
     *  - ���� ��ġ <> ���� value
     *  - ���� ��� <>
     */

    //������
    public InGameData data;
    private List<Dictionary<string, string>> textData;
    private List<int> buffNum = new List<int>(); //�� ������ ��ȣ ����.

    //UI ����
    public Transform canvas; //�Ҽӵ� Canvas
    public GameObject Box; //UIâ
    public GameObject[] position; //���� ��ġ
    public Sprite[] images;

    //������
    private int[] num; //���õ� ��ȣ

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

        textData = ExelReader.Read("BuffTest"); //���� ������ �޾ƿ���
    }

    public void Start()
    {
        Box.SetActive(false);

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
            Transform tf = position[i].GetComponent<Transform>();

            //ī�� ������
            if (textData[num[i]]["Type"].Equals("NatureBless")) //���� ī��
            {
                tf.gameObject.GetComponent<Image>().sprite = images[0];
            }
            else if (textData[num[i]]["Type"].Equals("DarknessCurse")) //����� ī��
            {
                tf.gameObject.GetComponent<Image>().sprite = images[1];
            }
            else //������ ī��
            {
                tf.gameObject.GetComponent<Image>().sprite = images[2];
            }

            tf.GetChild(0).GetComponent<Text>().text = textData[num[i]]["Name"]; //Title
            tf.GetChild(1).GetComponent<Text>().text = textData[num[i]]["Description"]; //Content
            tf.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(textData[i]["Directory"]);
        }
    }

    //���� �� ó��
    public void choice(int n) //ī�� ���� �� ����� �޼ҵ�
    {
        Box.SetActive(false); //���� â ����
        buffNum.RemoveAt(num[n]); //���� �ѿ��� ���� ��ȣ ����. -> ������ ������ �ʵ��� ��.
        Dictionary<string, string> choice = textData[num[n]]; //���õ� ��


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

        //data.BuffATKS(1.06f); �̷������� �ٲٸ� �ȴ�
    }
}