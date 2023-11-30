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
    public GameObject Box; //UI창
    public BuffCard[] position; //생성 위치
    public Sprite[] images;
    public GameObject[] effect;

    public UIManager ui;

    public GameObject my;

    //선택지
    private int[] num; //선택된 번호
    private int mineNum = 0;

    //선택된
    public BuffCard[] mine;

    //이펙트
    private GameObject[] effec;
    private int effectNum = 0;

    //프로그램
    public void Awake()
    {
        textData = ExelReader.Read("Data/inGame/BuffTable"); //버프 데이터 받아오기

        Box.SetActive(false);
        my.SetActive(false);

        //buffNum를 textData 행 번호(배열 인덱스)로 초기화
        for (int i = 0; i < textData.Count; i++)
        {
            buffNum.Add(i);
        }
    }

    //선택지 on
    public void play()
    {
        Box.SetActive(true);
        ui.Blind(true);
        GameManager.Instance.playingCardOn(); //카드 선택 중

        //1. 랜덤으로 셋 뽑기
        num = new int[] { -1, -1, -1 };

        for (int i = 0; i < num.Length; i++)
        {
            //선택
            num[i] = buffNum[UnityEngine.Random.Range(0, buffNum.Count)];

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
            position[i].cardSetting(textData[num[i]]);
        }
    }

    //선택 후 처리
    public void choice(int n) //카드 선택 시 시행될 메소드
    {
        Box.SetActive(false); //선택 창 제거
        ui.Blind(false);
        GameManager.Instance.playingCardOff(); //카드 선택 중

        buffNum.RemoveAt(num[n]); //버프 넘에서 선택 번호 제외. -> 다음에 뽑히지 않도록 함.
        Dictionary<string, string> choice = textData[num[n]]; //선택된 행
        mine[mineNum].cardSetting(choice);
        mineNum++;

        //버프
        //BuffTarget = ["Attack", "AttackSpeed", "Range", "Cooltime"]
        //BuffTargetTower = ["Ground", "Water", "Wind", "Single", "Multi", "Range"]
        string[] target;
        int value;
        string targetT;

        if (!choice["NatureBlessTargetTower"].Equals("null")) //버프 대상이 존재하면
        {
            target = choice["NatureBlessTargetTower"].Split(" ");
            value = int.Parse(choice["NatureBlessValue[%]"]);
            targetT = choice["NatureBlessTarget"];

            Targetting(targetT, target, value);
        }

        if (!choice["DarknessCurseTargetTower"].Equals("null")) //디버프 대상이 존재하면
        {
            target = choice["DarknessCurseTargetTower"].Split(" ");
            value = int.Parse(choice["DarknessCurseValue[%]"]);
            targetT = choice["DarknessCurseTarget"];

            Targetting(targetT, target, value);
        }

        //리워드
        if (!choice["RewardTarget"].Equals("null")) //리워드 대상이 존재하면
        {
            Debug.Log("리워드가 적용됩니다: 추후 적용 예정");
        }

        if (GameManager.Instance.getAuto())
        {
            GameManager.Instance.StartRound();
        }
    }

    private void Targetting(string targetT, string[] target, int value)
    {
        if (targetT.Equals("Attack"))
        {
            foreach (string s in target) //버프 처리
                data.BuffATK(s, value);
        }
        else if (targetT.Equals("AttackSpeed"))
        {
            foreach (string s in target) //버프 처리
                data.BuffATKS(s, value);
        }
        else if (targetT.Equals("Range"))
        {
            foreach (string s in target) //버프 처리
                data.BuffATKR(s, value);
        }
        else if (targetT.Equals("Cooltime"))
        {
            Debug.Log("쿨타임 버프: 추후 적용");
        }
        else
            Debug.Log(targetT + " 예외 오류: 해당 타깃에 대한 처리가 없습니다.");

        Debug.Log(targetT + "속성이 " + value + "만큼 버프되었어요");
    }

    public void watchBuff()
    {
        if (my.activeSelf == true)
        {
            SoundManager.Instance.popCloseSound();
            ui.Blind(false);
        }
        else
            ui.Blind(true);

        my.SetActive(!my.activeSelf);
    }
}