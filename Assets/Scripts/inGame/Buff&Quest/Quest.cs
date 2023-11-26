using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    //데이터
    public InGameData data;
    private List<Dictionary<string, string>> textData;

    //UI 동작
    public GameObject Box; //UI창
    public QuestCard[] position; //생성 위치
    public GameObject my; //선택된 카드 Panel
    public QuestCard mine; //선택된 카드

    public UIManager ui;

    //선택지
    private int[] num; //선택된 번호

    //프로그램

    public void Awake()
    {
        textData = ExelReader.Read("Data/inGame/QuestTest"); //퀘스트 데이터 받아오기

        //GameManager Start보다 빠르게
        Box.SetActive(false);
        my.SetActive(false);
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
            num[i] = UnityEngine.Random.Range(0, textData.Count);

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

    //세팅할 카드, 세팅할 정보
    private void cardSetting(Transform tf, Dictionary<string, string> target)
    {
        tf.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(target["Directory"]);
        tf.GetChild(2).GetComponent<Text>().text = target["Name"]; //Title
        tf.GetChild(3).GetComponent<Text>().text = target["QuestDescription"]; //Content
        tf.GetChild(4).GetComponent<Text>().text = target["RewardDescription"]; //Content
    }

    //선택 후 처리
    public void choice(int n) //카드 선택 시 시행될 메소드
    {
        Box.SetActive(false); //선택 창 제거
        ui.Blind(false);
        GameManager.Instance.playingCardOff(); //카드 선택 종료

        Dictionary<string, string> choice = textData[num[n]]; //선택된 행
        mine.cardSetting(choice);


        //내부 버프 효과 **************************값 변경 미적용***********************
        if (!choice["QuestTarget"].Equals("null")) //퀘스트 대상 존재
        {
            Debug.Log(choice["QuestTarget"] + "에 대한 퀘스트가 진행됩니다: 추후 적용 예정");
        }

        if (!choice["RewardTarget"].Equals("null")) //디버프 대상이 존재하면
        {
            Debug.Log(choice["RewardTarget"] + "에 대한 퀘스트가 진행됩니다: 추후 적용 예정");
        }

        //data.BuffATKS(1.06f); 예시문
    }

    public void watchQuest()
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

    //퀘스트 달성
    private int fusion;
    private int exist;
    private int smile;
    private int expressionless;
    private int skill;
    private int procession;


    public void countFusion()
    {

    }

    public void countExist()
    {

    }

    public void countKillSmile()
    {

    }

    public void countExpressionless()
    {

    }

    public void countSkill()
    {

    }

    public void countProcession()
    {

    }
}
