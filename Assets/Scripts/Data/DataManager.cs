using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�׽�Ʈ
public class DataManager : MonoBehaviour
{
    int count;

    /////////////////////* Data */////////////
    //���ݷ�
    float atk;
    public float Atk { get => atk; }

    float atkSpeed;
    public float AtkSpeed { get => atkSpeed; }

    float atkRange;
    public float AtkRange { get => atkRange; }

    //knowledge
    private int knowledgeCoin;
    public int KnowledgeCoin { get => knowledgeCoin; }
    //�Ҹ� Ȥ�� ȹ�� �޼ҵ� �߰�.

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

    //�ν��Ͻ�
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
        //�̱��� �׽�Ʈ 
        count = Random.Range(0, 10);

        //�̱���
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

    void Start() //data Load. ���� ȭ�鿡�� Ȩ ȭ������ �Ѿ �� �ε��ϸ� ���� ������
    {
        atk = DataSample.ATK;
        atkSpeed = DataSample.ATKS;
        atkRange = DataSample.ATKR;

        knowledgeCoin = DataSample.KnowledgeCoin;
        knowATK = DataSample.KnowATK;
        knowATKS = DataSample.KnowATKS;
        knowATKR = DataSample.KnowATKR;

    }

    public void getCount()
    {
        Debug.Log(count);
    }
}
