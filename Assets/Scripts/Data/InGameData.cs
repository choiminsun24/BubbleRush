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

    void Awake() //DataManager를 통해 기본 능력치 세팅
    {
        manager = GameObject.Find("DataManager").GetComponent<DataManager>();

        //기본 능력치
        ATK = manager.Atk;
        ATKS = manager.AtkSpeed;
        ATKR = manager.AtkRange;

        //딕셔너리 초기화 - 동물 지식 (행이 속성, 열이 각각 공격력, 공격속도, 공격범위인 행렬)
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

    //업그레이드 확인용
    public void check()
    {
        Debug.Log("ATK: " + ATK);
        Debug.Log("ATKS: " + ATKS);
        Debug.Log("ATKR: " + ATKR);
    }

    //능력치 변화
//앞에 있는 if 저거 지우기

    public void BuffATK(string type, int change)
    {
        if (!atkData.ContainsKey(type))
        {
            Debug.Log("해당 키는 아직 없어요");
            return;
        }
        Debug.Log(atkData[type]["ATK"] + "에서");

        atkData[type]["ATK"] += 0.01f * change;

        Debug.Log(atkData[type]["ATK"] + "로");
    }

    public void BuffATKS(string type, float change)
    {
        if (!atkData.ContainsKey(type))
        {
            Debug.Log("해당 키는 아직 없어요");
            return;
        }

        Debug.Log(atkData[type]["ATKS"] + "에서");

        atkData[type]["ATKS"] += 0.01f * change;

        Debug.Log(atkData[type]["ATKS"] + "로");
    }

    public void BuffATKR(string type, float change)
    {
        if (!atkData.ContainsKey(type))
        {
            Debug.Log("해당 키는 아직 없어요");
            return;
        }
        Debug.Log(atkData[type]["ATKR"] + "에서");

        atkData[type]["ATKR"] += 0.01f * change;

        Debug.Log(atkData[type]["ATKR"] + "로");
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
