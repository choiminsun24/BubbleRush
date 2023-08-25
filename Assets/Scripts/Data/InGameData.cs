using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameData : MonoBehaviour
{
    private DataManager manager;

    private float ATK;
    private float ATKS;
    private float ATKR;


    void Awake() //DataManager�� ���� �⺻ �ɷ�ġ ����
    {
        manager = GameObject.Find("DataManager").GetComponent<DataManager>();

        //�⺻ �ɷ�ġ * knowledge �ɷ�ġ
        ATK = manager.Atk * GameData.GetKnowATK(manager.KnowATK);
        ATKS = manager.AtkSpeed * GameData.GetKnowATKS(manager.KnowATKS);
        ATKR = manager.AtkRange * GameData.GetKnowATKR(manager.KnowATKR);

    }

    //���׷��̵� Ȯ�ο�
    public void check()
    {
        Debug.Log("ATK: " + ATK);
        Debug.Log("ATKS: " + ATKS);
        Debug.Log("ATKR: " + ATKR);
    }

    //�ɷ�ġ ��ȭ
    public void BuffATK(float change)
    {
        ATK *= change;
    }

    public void BuffATKS(float change)
    {
        ATKS *= change;
    }

    public void BuffATKR(float change)
    {
        ATKR *= change;
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
