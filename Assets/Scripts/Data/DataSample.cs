using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//저장될 데이터. 서버 구축하면 서버에 저장, local이면 뭘로할지 조금 더 고민 해보기(Json, Text, Xml 등) -> 일단 임시데이터
public class DataSample : MonoBehaviour
{
    //테스트용 데이터
    //재화
    static private int stageCoin = 100;
    static private int knowledgeCoin = 100;

    public static int StageCoin { get => stageCoin;}
    public static int KnowledgeCoin { get => knowledgeCoin; set => knowledgeCoin = value; }

    //attack
    private static float atk = 1f;
    private static float atkS = 1f;
    private static float atkR = 1f;


    public static float ATK { get => atk; set => atk = value; }
    public static float ATKS { get => atkS; set => atkS = value; }
    public static float ATKR { get => atkR; set => atkR = value; }


    //knowledge
    private static int knowATK = 0;
    private static int knowATKS = 0;
    private static int knowATKR = 0;

    
    public static int KnowATK { get => knowATK; set => knowATK = value; }
    public static int KnowATKS { get => knowATKS; set => knowATKS = value; }
    public static int KnowATKR { get => knowATKR; set => knowATKR = value; }


    static public int GetStageCoin()
    {
        return stageCoin;
    }
}
