using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Buff : MonoBehaviour
{
    /*
     * Name : Title
     * BuffTargetTower : ���� ��� Ÿ�� -> BuffTarget
     * BuffTarget : ���� ���� -> 
     * Bu
     * 
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

    //������
    private int[] num; //���õǾ��� ��ȣ

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

        //Ȯ�ο� - ��� Ű ���
        foreach(string key in textData[0].Keys)
        {
            Debug.Log(key);
        }
    }

    public void Start()
    {
        Box.SetActive(false);

        //*******************buffNum�� ������ �� ���� ��ȣ�� �ʱ�ȭ
        foreach(Dictionary<string, string> column in textData)
        {
            print(column["Description"]);
            try
            {
                buffNum.Add(int.Parse(column["Index"]));
            }
            catch(FormatException ex)
            {
                Debug.Log("Integer���� ��ȯ �Ұ��� �׸��Դϴ�.");
            }
        }

        //Ȯ�ο�
        for (int i = 0; i < buffNum.Count; i++)
        {
            Debug.Log(buffNum[i]);
        }
    }

    //������ on
    public void play()
    {
        Box.SetActive(true);

        //1. �������� �� �̰�
        num = new int[] {-1, -1, -1}; //�������� �� ������ �� ������ȣ

        for (int i = 0; i < num.Length; i++) //3�� ����
        {
            //��÷
            int r = UnityEngine.Random.Range(0, buffNum.Count);
            num[i] = buffNum[r];
                
            //�ߺ� �˻�
            for (int j = 0 ; j < i; j++)
            {
                if (num[j] == r)
                {
                    i--;
                    break;
                }
            }
        }

        //2. ī�� ����
        for(int i = 0; i < num.Length;i++)
        {
            //position[i] = 
        }

        //********************2. Canvas�� ��ġ -> ������� �� ������ȣ�� ���� ���� �ε����� ����.
        //for (int i = 0; i < 3; i++)
        //{
        //    GameObject game = Instantiate(buffNum[num[i]], position[i].transform.position, Quaternion.identity, canvas);
        //}
    }

    //���� �� ó��
    public void choice(int n) //ī�� ���� �� ����� �޼ҵ�
    {
        Box.SetActive(false);
        //buffNum���� �ش� ������ȣ ����
        //buffNum���� num[n] ����
        
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
