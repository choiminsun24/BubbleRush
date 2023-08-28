using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    /*
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
    public Sprite[] images;

    //선택지
    private int[] num; //선택된 번호

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

        //확인용 - textData 키, 행 수 O
    }

    public void Start()
    {
        Box.SetActive(false);

        //*******************buffNum를 textData 행 번호(배열 인덱스)로 초기화
        for (int i = 0; i < textData.Count; i++)
        {
            buffNum.Add(i);
        }

        play();

        //확인용 - BuffNum O

    }

    //선택지 on
    public void play()
    {
        Box.SetActive(true);

        //1. 랜덤으로 셋 뽑고
        num = new int[] { -1, -1, -1 }; //선택지에 들어갈 버프의 각 고유번호

        for (int i = 0; i < num.Length; i++) //3개 선택
        {
            //추첨
            int r = UnityEngine.Random.Range(0, buffNum.Count);
            //num[i] = buffNum[r];
            num[i] = r;

            //중복 검사
            for (int j = 0; j < i; j++)
            {
                if (num[j] == r)
                {
                    i--;
                    break;
                }
            }
        }

        //2. 카드 세팅
        for (int i = 0; i < num.Length; i++)
        {
            Transform tf = position[i].GetComponent<Transform>();

            //카드 프레임
            if (textData[num[i]]["Type"].Equals("NatureBless")) //버프 카드
            {
                tf.gameObject.GetComponent<Image>().sprite = images[0];
            }
            else if (textData[num[i]]["Type"].Equals("DarknessCurse")) //디버프 카드
            {
                tf.gameObject.GetComponent<Image>().sprite = images[1];
            }
            else //리워드 카드
            {
                tf.gameObject.GetComponent<Image>().sprite = images[2];
            }
            //tf.gameObject.GetComponent<Image>
            tf.GetChild(0).GetComponent<Text>().text = textData[num[i]]["Name"]; //Title
            tf.GetChild(1).GetComponent<Text>().text = textData[num[i]]["Description"]; //Content
            tf.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(textData[i]["Directory"]);
        }
    }

    //선택 후 처리
    public void choice(int n) //카드 선택 시 시행될 메소드
    {
        Box.SetActive(false);

        //buffNum에서 해당 고유번호 삭제
        buffNum.RemoveAt(n);
        
        //내부 버프 효과 **************************값 변경 미적용***********************
        Dictionary<string, string> choice = textData[num[n]]; //선택받은 고유 번호

        if (!choice["NatureBlessTargetTower"].Equals("null")) //버프 대상이 존재하면
        {
            Debug.Log("버프가 적용됩니다: 추후 적용 예정");
        }

        if (!choice["DarknessCurseTargetTower"].Equals("null")) //디버프 대상이 존재하면
        {
            Debug.Log("디버프가 적용됩니다: 추후 적용 예정");
        }

        if (!choice["RewardTarget"].Equals ("null")) //리워드 대상이 존재하면
        {
            Debug.Log("리워드가 적용됩니다: 추후 적용 예정");
        }
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
