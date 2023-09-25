using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameData : MonoBehaviour
{
    private DataManager manager;

    private float ATK;
    private float ATKS;
    private float ATKR;
    private Dictionary<string,Dictionary<string,float>> atkData;

    void Awake() //DataManager�� ���� �⺻ �ɷ�ġ ����
    {
        manager = GameObject.Find("DataManager").GetComponent<DataManager>();

        //�⺻ �ɷ�ġ
        ATK = manager.Atk;
        ATKS = manager.AtkSpeed;
        ATKR = manager.AtkRange;

        //��ųʸ� �ʱ�ȭ - ���� ���� (���� �Ӽ�, ���� ���� ���ݷ�, ���ݼӵ�, ���ݹ����� ���)
        atkData = new Dictionary<string, Dictionary<string,float>>();

        atkData["Ground"] = new Dictionary<string, float>();
        atkData["Water"] = new Dictionary<string, float>();
        atkData["Wind"] = new Dictionary<string, float>();

        atkData["Ground"]["ATK"] = GameData.GetKnowATK(manager.KnowATK);
        atkData["Ground"]["ATKS"] = GameData.GetKnowATKS(manager.KnowATKS);
        atkData["Ground"]["ATKR"] = GameData.GetKnowATKR(manager.KnowATKR);

        atkData["Water"]["ATK"] = GameData.GetKnowATK(manager.KnowATK);
        atkData["Water"]["ATKS"] = GameData.GetKnowATKS(manager.KnowATKS);
        atkData["Water"]["ATKR"] = GameData.GetKnowATKR(manager.KnowATKR);

        atkData["Wind"]["ATK"] = GameData.GetKnowATK(manager.KnowATK);
        atkData["Wind"]["ATKS"] = GameData.GetKnowATKS(manager.KnowATKS);
        atkData["Wind"]["ATKR"] = GameData.GetKnowATKR(manager.KnowATKR);
    }

    //���׷��̵� Ȯ�ο�
    public void check()
    {
        Debug.Log("ATK: " + ATK);
        Debug.Log("ATKS: " + ATKS);
        Debug.Log("ATKR: " + ATKR);
    }

    //�ɷ�ġ ��ȭ
//�տ� �ִ� if ���� �����

    public void BuffATK(string type, int change)
    {
        if (!atkData.ContainsKey(type))
        {
            Debug.Log("�ش� Ű�� ���� �����");
            return;
        }
        Debug.Log(atkData[type]["ATK"] + "����");

        atkData[type]["ATK"] += 0.01f * change;

        Debug.Log(atkData[type]["ATK"] + "��");
    }

    public void BuffATKS(string type, float change)
    {
        if (!atkData.ContainsKey(type))
        {
            Debug.Log("�ش� Ű�� ���� �����");
            return;
        }

        Debug.Log(atkData[type]["ATKS"] + "����");

        atkData[type]["ATKS"] += 0.01f * change;

        Debug.Log(atkData[type]["ATKS"] + "��");
    }

    public void BuffATKR(string type, float change)
    {
        if (!atkData.ContainsKey(type))
        {
            Debug.Log("�ش� Ű�� ���� �����");
            return;
        }
        Debug.Log(atkData[type]["ATKR"] + "����");

        atkData[type]["ATKR"] += 0.01f * change;

        Debug.Log(atkData[type]["ATKR"] + "��");
    }

    //��ȭ ����
    private int stageCoin = 100;

    public int GetStageCoin()
    {
        return stageCoin;
    }

    public void AddStageCoin(int coin)
    {
        if (coin < 0) //when we use the coin
        {
            coin *= -1; //absolution 

            if (stageCoin >= coin) //adequate
            {
                stageCoin -= coin;
            }
            else //inadequate
                return;
        }
        else //when we got the coin
        {
            stageCoin += coin;
        }
    }
}
