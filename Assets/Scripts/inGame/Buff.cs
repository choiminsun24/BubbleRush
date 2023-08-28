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

        textData = ExelReader.Read("BuffTest");

        //Ȯ�ο� - textData Ű, �� �� O
    }

    public void Start()
    {
        Box.SetActive(false);

        //*******************buffNum�� textData �� ��ȣ(�迭 �ε���)�� �ʱ�ȭ
        for (int i = 0; i < textData.Count; i++)
        {
            buffNum.Add(i);
        }

        play();

        //Ȯ�ο� - BuffNum O

    }

    //������ on
    public void play()
    {
        Box.SetActive(true);

        //1. �������� �� �̰�
        num = new int[] { -1, -1, -1 }; //�������� �� ������ �� ������ȣ

        for (int i = 0; i < num.Length; i++) //3�� ����
        {
            //��÷
            int r = UnityEngine.Random.Range(0, buffNum.Count);
            //num[i] = buffNum[r];
            num[i] = r;

            //�ߺ� �˻�
            for (int j = 0; j < i; j++)
            {
                if (num[j] == r)
                {
                    i--;
                    break;
                }
            }
        }

        //2. ī�� ����
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
            //tf.gameObject.GetComponent<Image>
            tf.GetChild(0).GetComponent<Text>().text = textData[num[i]]["Name"]; //Title
            tf.GetChild(1).GetComponent<Text>().text = textData[num[i]]["Description"]; //Content
            tf.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(textData[i]["Directory"]);
        }
    }

    //���� �� ó��
    public void choice(int n) //ī�� ���� �� ����� �޼ҵ�
    {
        Box.SetActive(false);

        //buffNum���� �ش� ������ȣ ����
        buffNum.RemoveAt(n);
        
        //���� ���� ȿ�� **************************�� ���� ������***********************
        Dictionary<string, string> choice = textData[num[n]]; //���ù��� ���� ��ȣ

        if (!choice["NatureBlessTargetTower"].Equals("null")) //���� ����� �����ϸ�
        {
            Debug.Log("������ ����˴ϴ�: ���� ���� ����");
        }

        if (!choice["DarknessCurseTargetTower"].Equals("null")) //����� ����� �����ϸ�
        {
            Debug.Log("������� ����˴ϴ�: ���� ���� ����");
        }

        if (!choice["RewardTarget"].Equals ("null")) //������ ����� �����ϸ�
        {
            Debug.Log("�����尡 ����˴ϴ�: ���� ���� ����");
        }
    }

    //**************************�� ���� -> ����, csv ���ϰ� CSV Reader���� ����.
    //private bool doMedicine = false;
    //public void Medicine()
    //{
    //    if (doMedicine == false) //���� ����
    //    {
    //        doMedicine = true;
    //        choice();
    //    }
    //    else
    //    {
    //        buffNum.Remove(buffNum[2]);
    //        data.BuffATKS(2f);
    //    }
    //}

    //private bool energy = false;
    //public void Energy()
    //{
    //    if (energy == false) // ���� ����
    //    {
    //        energy = true;
    //        choice();
    //    }
    //    else //�̹� ���õ�.
    //    {
    //        data.BuffATKS(1.06f);
    //        Debug.Log("�̹� ���õǾ� ���õ� �� ���� ��ư: ����Ʈ���� ���ŵ��� ���� ������ ������.");
    //    }
    //}

    //private bool doBlood = false;
    //public void BloodFlower()
    //{
    //    if (doBlood == false) // ���� ����
    //    {
    //        doBlood = true;
    //        choice();
    //    }
    //    else
    //    {
    //        data.BuffATK(1.1f);
    //    }
    //}

    //private bool doPain = false;
    //public void PainShadow()
    //{
    //    if (doPain == false) // ���� ����
    //    {
    //        doPain = true;
    //        choice();
    //    }
    //    else
    //    {
    //        data.BuffATKS(0.7f);
    //    }
    //}

    //private bool doWeak = false;
    //public void Weak()
    //{
    //    if (doWeak == false) // ���� ����
    //    {
    //        doWeak = true;
    //        choice();
    //    }
    //    else
    //    {
    //        data.BuffATK(0.7f);
    //    }
    //}
}
