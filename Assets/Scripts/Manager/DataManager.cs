using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//테스트
public class DataManager : MonoBehaviour
{
    int count;

    /////////////////////* Data */////////////
    //공격력
    float atk;
    public float Atk { get => atk; }

    float atkSpeed;
    public float AtkSpeed { get => atkSpeed; }

    float atkRange;
    public float AtkRange { get => atkRange; }

    //knowledge
    int knowATK;
    public int KnowATK { get => knowATK; }
    public void UpgradeKnowATK()
    {
        knowATK++;
        DataSample.KnowATK = knowATK;
    }

    int knowATKS;
    public int KnowATKS { get => knowATKS; }
    public void UpgradeKnowATKS()
    {
        knowATKS++;
        DataSample.KnowATKS = knowATKS;
    }

    int knowATKR;
    public int KnowATKR { get => knowATKR; }
    public void UpgradeKnowATKR()
    {
        knowATKR++;
        DataSample.KnowATKR = knowATKR;
    }
    ///////////////////////////////////////////

    //인스턴스
    static public DataManager instance;

    static public DataManager Instance
    {
        get
        {
            if (instance == null)
                return null;

            return instance;
        }
    }

    void Awake()
    {
        //싱글톤 테스트 
        count = Random.Range(0, 10);
        Debug.Log(count);

        //싱글톤
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start() //data Load. 실행 화면에서 홈 화면으로 넘어갈 때 로딩하면 되지 않을까
    {
        atk = DataSample.ATK;
        atkSpeed = DataSample.ATKS;
        atkRange = DataSample.ATKR;

        knowATK = DataSample.KnowATK;
        knowATKS = DataSample.KnowATKS;
        knowATKR = DataSample.KnowATKR;
    }

    public void getCount()
    {
        Debug.Log(count);
    }
}
