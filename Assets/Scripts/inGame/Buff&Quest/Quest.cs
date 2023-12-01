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
    private bool complete = false;

    //프로그램
    private static Quest _instance;

    // 인스턴스에 접근하기 위한 프로퍼티
    public static Quest Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(Quest)) as Quest;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // 인스턴스가 존재하는 경우 기존 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(this);
        }

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
            switch(choice["QuestType"])
            {
                case "Fusion":
                    fusionQuest = int.Parse(choice["QuestCount"]);
                    ui.UpdateQuestUI(complete, fusionQuest, 0);
                    break;
                case "Kill":
                    switch(choice["QuestTarget"])
                    {
                        case "ExpressionlessBubble":
                            expressionlessQuest = int.Parse(choice["QuestCount"]);
                            ui.UpdateQuestUI(complete, expressionlessQuest, 0);
                            break;
                        case "Smile":
                            smileQuest = int.Parse(choice["QuestCount"]);
                            ui.UpdateQuestUI(complete, smileQuest, 0);
                            break;
                        default:
                            Debug.Log("추가되지 않은 퀘스트 타겟: " + choice["QuestTarget"]);
                            break;
                    }
                    break;
                case "Skill":
                    skillQuest = int.Parse(choice["QuestCount"]);
                    ui.UpdateQuestUI(complete, skillQuest, 0);
                    break;
                case "Possession":
                    possessionQuest = int.Parse(choice["QuestCount"]);
                    ui.UpdateQuestUI(complete, possessionQuest, 0);
                    break;
                default:
                    Debug.Log("추가되지 않은 퀘스트: " + choice["QuestCount"]);
                    break;
            }
            Debug.Log(choice["QuestType"] + "에 대한 퀘스트가 진행됩니다.");
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
    private int fusionQuest;
    private int existQuest;
    private int smileQuest;
    private int expressionlessQuest;
    private int skillQuest;
    private int possessionQuest;

    public void CheckQuest(int mission)
    {
        if (fusionQuest > 0)
        {
            checkFusion(mission);
        }
        else if (existQuest > 0)
        {
            checkExist(mission);
        }
        else if (smileQuest > 0)
        {
            checkSmile(mission);
        }
        else if (expressionlessQuest > 0)
        {
            checkExpressionless(mission);
        }
        else if (skillQuest > 0)
        {
            checkSkill(mission);
        }
        else if (possessionQuest > 0)
        {
            checkPossession(mission);
        }
    }

    public void checkFusion(int fusion)
    {
        //이미 완료했으면 말고
        if (complete)
        {
            return;
        }

        if (fusion >= fusionQuest)
            complete = true;

        ui.UpdateQuestUI(complete, fusionQuest, fusion);
    }

    public void checkExist(int exist)
    {
        if (complete)
        {
            return;
        }

        if (exist >= existQuest)
            complete = true;

        ui.UpdateQuestUI(complete, existQuest, exist);
    }

    public void checkSmile(int smile)
    {
        if (complete)
        {
            return;
        }

        if (smile >= smileQuest)
            complete= true;

        ui.UpdateQuestUI(complete, smileQuest, smile);
    }

    public void checkExpressionless(int expressionless)
    {
        if (complete)
        {
            return;
        }

        if (expressionless >= expressionlessQuest)
            complete = true;

        ui.UpdateQuestUI(complete, expressionlessQuest, expressionless);
    }

    public void checkSkill(int skill)
    {
        if (complete)
        {
            return;
        }

        if (skill >= skillQuest)
            complete = true;

        ui.UpdateQuestUI(complete, skillQuest, skill);
    }

    public void checkPossession(int possession)
    {
        if (complete)
        {
            return;
        }

        if (possession >= possessionQuest)
            complete = true;

        ui.UpdateQuestUI(complete, possessionQuest, possession);
    }
}
