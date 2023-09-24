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
    private int effectNum = 0;

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

    public void check()
    {
        int n = 0;
        for (int i = 0; i < buffNum.Count;i++)
        {
            n += buffNum[i];
        }

        Debug.Log(n);
    }

    //������ on
    public void play()
    {
        check();
        Box.SetActive(true);
        ui.Blind();

        //1. �������� �� �̱�
        num = new int[] { -1, -1, -1 };

        for (int i = 0; i < num.Length; i++)
        {
            //����
            num[i] = buffNum[UnityEngine.Random.Range(0, buffNum.Count)];

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

    //���� �� ó��
    public void choice(int n) //ī�� ���� �� ����� �޼ҵ�
    {
        Debug.Log("ī�尡 ���õ�: " + n);
        Debug.Log(mine[mineNum]);

        Box.SetActive(false); //���� â ����
        ui.Blind();

        buffNum.RemoveAt(num[n]); //���� �ѿ��� ���� ��ȣ ����. -> ������ ������ �ʵ��� ��.
        Dictionary<string, string> choice = textData[num[n]]; //���õ� ��
        mine[mineNum].cardSetting(choice);
        mineNum++;


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

        if (GameManager.Instance.getAuto())
        {
            GameManager.Instance.StartRound();
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