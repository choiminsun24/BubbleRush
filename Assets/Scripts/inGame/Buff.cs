using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Buff : MonoBehaviour
{
    /*
     * Name : Title
     * BuffTargetTower : 버프 대상 타워 -> BuffTarget
     * BuffTarget : 버프 종류 -> 
     * Bu
     * 
     * 필요한 것 
     * UI
     *  - 버프 종류 <> 테두리
     *  - 버프 이미지 <> 이미지
     *  - Name <> 버프 타이틀
     *  - 버프 Content <> 버프 content
     *  
     *내부 처리
     *  - 버프 종류 <> 버프 메서드
     *  - 버프 수치 <> 버프 value
     *  - 버프 대상 <>
     */

    //데이터
    public InGameData data;
    private List<Dictionary<string, string>> textData;
    private List<int> buffNum = new List<int>(); //각 버프의 번호 저장.

    //UI 동작
    public Transform canvas; //소속된 Canvas
    public GameObject Box; //UI창
    public GameObject[] position; //생성 위치

    //선택지
    private int[] num; //선택되었던 번호

    //프로그램
    private static Buff instance;

    public static Buff Instance
    {
        get
        {
            if (!instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void Awake()
    {
        instance = this; //마지막에 생성된 하나만 사용.

        textData = ExelReader.Read("BuffTest");

        //확인용 - 모든 키 출력
        foreach(string key in textData[0].Keys)
        {
            Debug.Log(key);
        }
    }

    public void Start()
    {
        Box.SetActive(false);

        //*******************buffNum의 내용을 각 고유 번호로 초기화
        foreach(Dictionary<string, string> column in textData)
        {
            print(column["Description"]);
            try
            {
                buffNum.Add(int.Parse(column["Index"]));
            }
            catch(FormatException ex)
            {
                Debug.Log("Integer으로 변환 불가한 항목입니다.");
            }
        }

        //확인용
        for (int i = 0; i < buffNum.Count; i++)
        {
            Debug.Log(buffNum[i]);
        }
    }

    //선택지 on
    public void play()
    {
        Box.SetActive(true);

        //1. 랜덤으로 셋 뽑고
        num = new int[] {-1, -1, -1}; //선택지에 들어갈 버프의 각 고유번호

        for (int i = 0; i < num.Length; i++) //3개 선택
        {
            //추첨
            int r = UnityEngine.Random.Range(0, buffNum.Count);
            num[i] = buffNum[r];
                
            //중복 검사
            for (int j = 0 ; j < i; j++)
            {
                if (num[j] == r)
                {
                    i--;
                    break;
                }
            }
        }

        //2. 카드 세팅
        for(int i = 0; i < num.Length;i++)
        {
            //position[i] = 
        }

        //********************2. Canvas에 설치 -> 사라지고 각 고유번호에 대한 정보 로딩으로 변경.
        //for (int i = 0; i < 3; i++)
        //{
        //    GameObject game = Instantiate(buffNum[num[i]], position[i].transform.position, Quaternion.identity, canvas);
        //}
    }

    //선택 후 처리
    public void choice(int n) //카드 선택 시 시행될 메소드
    {
        Box.SetActive(false);
        //buffNum에서 해당 고유번호 삭제
        //buffNum에서 num[n] 삭제
        
    }

    //**************************각 버프 -> 삭제, csv 파일과 CSV Reader에서 정의.
    //private bool doMedicine = false;
    //public void Medicine()
    //{
    //    if (doMedicine == false) //선택 가능
    //    {
    //        doMedicine = true;
    //        choice();
    //    }
    //    else
    //    {
    //        buffNum.Remove(buffNum[2]);
    //        data.BuffATKS(2f);
    //    }
    //}

    //private bool energy = false;
    //public void Energy()
    //{
    //    if (energy == false) // 선택 가능
    //    {
    //        energy = true;
    //        choice();
    //    }
    //    else //이미 선택됨.
    //    {
    //        data.BuffATKS(1.06f);
    //        Debug.Log("이미 선택되어 선택될 수 없는 버튼: 리스트에서 제거되지 않은 것으로 추정됨.");
    //    }
    //}

    //private bool doBlood = false;
    //public void BloodFlower()
    //{
    //    if (doBlood == false) // 선택 가능
    //    {
    //        doBlood = true;
    //        choice();
    //    }
    //    else
    //    {
    //        data.BuffATK(1.1f);
    //    }
    //}

    //private bool doPain = false;
    //public void PainShadow()
    //{
    //    if (doPain == false) // 선택 가능
    //    {
    //        doPain = true;
    //        choice();
    //    }
    //    else
    //    {
    //        data.BuffATKS(0.7f);
    //    }
    //}

    //private bool doWeak = false;
    //public void Weak()
    //{
    //    if (doWeak == false) // 선택 가능
    //    {
    //        doWeak = true;
    //        choice();
    //    }
    //    else
    //    {
    //        data.BuffATK(0.7f);
    //    }
    //}
}
