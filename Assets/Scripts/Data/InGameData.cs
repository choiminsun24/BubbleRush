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
    private Dictionary<string, Dictionary<string, float>> enemyBuffData;

    //�̱���
    private static InGameData _instance;

    public static InGameData Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(InGameData)) as InGameData;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    void Awake() //DataManager�� ���� �⺻ �ɷ�ġ ����
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // �ν��Ͻ��� �����ϴ� ��� ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            Destroy(this);
        }

        manager = GameObject.Find("DataManager").GetComponent<DataManager>();

        //�⺻ �ɷ�ġ
        ATK = manager.Atk;
        ATKS = manager.AtkSpeed;
        ATKR = manager.AtkRange;

        //Ÿ�� �⺻ �ɷ�ġ �ʱ�ȭ - ���� ���� (���� �Ӽ�, ���� ���� ���ݷ�, ���ݼӵ�, ���ݹ����� ���)
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

        //���� ���� ������
        enemyBuffData = new Dictionary<string, Dictionary<string, float>>();

        enemyBuffData["Mad"] = new Dictionary<string, float>();
        enemyBuffData["Smile"] = new Dictionary<string, float>();
        enemyBuffData["Expressionless"] = new Dictionary<string, float>();

        enemyBuffData["Mad"]["Damaged"] = 1;
        enemyBuffData["Smile"]["Damaged"] = 1;
        enemyBuffData["Expressionless"]["Damaged"] = 1;
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

    public void BuffExpressionlessEnemyDamaged(int per)
    {
        enemyBuffData["Expressionless"]["Damaged"] += per * 0.01f;
    }

    public float getBuffExpressionlessEnemyDamaged()
    {
        return enemyBuffData["Expressionless"]["Damaged"];
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

        Quest.Instance.checkPossession(stageCoin);
    }
}
