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

    //������ on
    public void play()
    {
        Box.SetActive(true);
        ui.Blind(true);
        GameManager.Instance.playingCardOn(); //ī�� ���� ��

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
        Box.SetActive(false); //���� â ����
        ui.Blind(false);
        GameManager.Instance.playingCardOff(); //ī�� ���� ��

        buffNum.RemoveAt(num[n]); //���� �ѿ��� ���� ��ȣ ����. -> ������ ������ �ʵ��� ��.
        Dictionary<string, string> choice = textData[num[n]]; //���õ� ��
        mine[mineNum].cardSetting(choice);
        mineNum++;

        //����
        //BuffTarget = ["Attack", "AttackSpeed", "Range", "Cooltime"]
        //BuffTargetTower = ["Ground", "Water", "Wind", "Single", "Multi", "Range"]
        string[] target;
        int value;
        string targetT;

        if (!choice["NatureBlessTargetTower"].Equals("null")) //���� ����� �����ϸ�
        {
            target = choice["NatureBlessTargetTower"].Split(" ");
            value = int.Parse(choice["NatureBlessValue[%]"]);
            targetT = choice["NatureBlessTarget"];

            Targetting(targetT, target, value);
        }

        if (!choice["DarknessCurseTargetTower"].Equals("null")) //����� ����� �����ϸ�
        {
            target = choice["DarknessCurseTargetTower"].Split(" ");
            value = int.Parse(choice["DarknessCurseValue[%]"]);
            targetT = choice["DarknessCurseTarget"];

            Targetting(targetT, target, value);
        }

        //������
        if (!choice["RewardTarget"].Equals("null")) //������ ����� �����ϸ�
        {
            Debug.Log("�����尡 ����˴ϴ�: ���� ���� ����");
        }

        if (GameManager.Instance.getAuto())
        {
            GameManager.Instance.StartRound();
        }
    }

    private void Targetting(string targetT, string[] target, int value)
    {
        if (targetT.Equals("Attack"))
        {
            foreach (string s in target) //���� ó��
                data.BuffATK(s, value);
        }
        else if (targetT.Equals("AttackSpeed"))
        {
            foreach (string s in target) //���� ó��
                data.BuffATKS(s, value);
        }
        else if (targetT.Equals("Range"))
        {
            foreach (string s in target) //���� ó��
                data.BuffATKR(s, value);
        }
        else if (targetT.Equals("Cooltime"))
        {
            Debug.Log("��Ÿ�� ����: ���� ����");
        }
        else
            Debug.Log(targetT + " ���� ����: �ش� Ÿ�꿡 ���� ó���� �����ϴ�.");

        Debug.Log(targetT + "�Ӽ��� " + value + "��ŭ �����Ǿ����");
    }

    public void watchBuff()
    {
        if (my.activeSelf == true)
        {
            SoundManager.Instance.popCloseSound();
            ui.Blind(false);
        }
        else
            ui.Blind(true);

        my.SetActive(!my.activeSelf);
    }
}