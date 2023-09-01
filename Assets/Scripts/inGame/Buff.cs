using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    //데이터
    public InGameData data;
    private List<Dictionary<string, string>> textData;
    private List<int> buffNum = new List<int>(); //각 버프의 번호 저장.

    //UI 동작
    public Transform canvas; //소속된 Canvas
    public GameObject Box; //UI창
    public Transform[] position; //생성 위치
    public Sprite[] images;
    public GameObject[] effect;

    public GameObject my;

    //선택지
    private int[] num; //선택된 번호
    private int mineNum = 0;

    //선택된
    public Transform[] mine;

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

        textData = ExelReader.Read("Data/inGame/BuffTable"); //버프 데이터 받아오기
    }

    public void Start()
    {
        Box.SetActive(false);
        my.SetActive(false);

        //buffNum를 textData 행 번호(배열 인덱스)로 초기화
        for (int i = 0; i < textData.Count; i++)
        {
            buffNum.Add(i);
        }

        //play();
    }

    //선택지 on
    public void play()
    {
        Box.SetActive(true);

        //1. 랜덤으로 셋 뽑기
        num = new int[] { -1, -1, -1 };

        for (int i = 0; i < num.Length; i++)
        {
            //선택
            num[i] = UnityEngine.Random.Range(0, buffNum.Count);

            //중복 검사
            for (int j = 0; j < i; j++)
            {
                if (num[j] == num[i])
                {
                    i--;
                    break;
                }
            }
        }

        //2. 카드 UI 세팅
        for (int i = 0; i < num.Length; i++)
        {
            Transform tf = position[i];

            cardSetting(tf, textData[num[i]]);
        }
    }

    //세팅할 카드, 세팅할 정보
    private void cardSetting(Transform tf, Dictionary<string, string> target)
    {
        //카드 프레임
        if (target["Type"].Equals("NatureBless")) //버프 카드
        {
            tf.GetChild(1).GetComponent<Image>().sprite = images[0];
            GameObject game = Instantiate(effect[0], tf.GetChild(4).position, Quaternion.identity, tf);

        }
        else if (target["Type"].Equals("DarknessCurse")) //디버프 카드
        {
            tf.GetChild(1).GetComponent<Image>().sprite = images[1];
            GameObject game = Instantiate(effect[1], tf.GetChild(4).position, Quaternion.identity, tf);
        }
        else //리워드 카드
        {
            tf.GetChild(1).GetComponent<Image>().sprite = images[2];
            GameObject game = Instantiate(effect[2], tf.GetChild(4).position, Quaternion.identity, tf);
        }

        tf.GetChild(2).GetComponent<Text>().text = target["Name"]; //Title
        tf.GetChild(3).GetComponent<Text>().text = target["Description"]; //Content
        tf.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(target["Directory"]);
    }

    //선택 후 처리
    public void choice(int n) //카드 선택 시 시행될 메소드
    {
        Box.SetActive(false); //선택 창 제거
        buffNum.RemoveAt(num[n]); //버프 넘에서 선택 번호 제외. -> 다음에 뽑히지 않도록 함.
        Dictionary<string, string> choice = textData[num[n]]; //선택된 행
        cardSetting(mine[mineNum], choice);


        //내부 버프 효과 **************************값 변경 미적용***********************
        if (!choice["NatureBlessTargetTower"].Equals("null")) //버프 대상이 존재하면
        {
            Debug.Log("버프가 적용됩니다: 추후 적용 예정");
        }

        if (!choice["DarknessCurseTargetTower"].Equals("null")) //디버프 대상이 존재하면
        {
            Debug.Log("디버프가 적용됩니다: 추후 적용 예정");
        }

        if (!choice["RewardTarget"].Equals("null")) //리워드 대상이 존재하면
        {
            Debug.Log("리워드가 적용됩니다: 추후 적용 예정");
        }

        //data.BuffATKS(1.06f); 예시문
    }

    public void watchChoice()
    {
        my.SetActive(!my.activeSelf);
    }
}