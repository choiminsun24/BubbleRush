using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameData : MonoBehaviour
{
    private DataManager manager;

    private float ATK;
    private float ATKS;
    private float ATKR;


    void Awake() //DataManager를 통해 기본 능력치 세팅
    {
        manager = GameObject.Find("DataManager").GetComponent<DataManager>();

        ATK = manager.Atk * GameData.GetKnowATK(manager.KnowATK);
        ATKS = manager.AtkSpeed * GameData.GetKnowATKS(manager.KnowATKS);
        ATKR = manager.AtkRange * GameData.GetKnowATKR(manager.KnowATKR);
    }

    //능력치 변화
    public void BuffATK(float change)
    {
        ATK *= change;
    }

    public void BuffATKS(float change)
    {
        ATK *= change;
    }

    public void BuffATKT(float change)
    {
        ATKR *= change;
    }

    //재화 관리
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
