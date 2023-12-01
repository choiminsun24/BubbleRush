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


    Dictionary<string, string> mainCard;

    //선택 후 처리
    public void choice(int n) //카드 선택 시 시행될 메소드
    {
        Box.SetActive(false); //선택 창 제거
        ui.Blind(false);
        GameManager.Instance.playingCardOff(); //카드 선택 종료

        mainCard = textData[num[n]]; //선택된 행
        mine.cardSetting(mainCard);


        //내부 버프 효과 **************************값 변경 미적용***********************
        if (!mainCard["QuestTarget"].Equals("null")) //퀘스트 대상 존재
        {
            switch(mainCard["QuestType"])
            {
                case "Fusion":
                    state = QuestState.FUSION;
                    fusionQuest = int.Parse(mainCard["QuestCount"]);
                    ui.UpdateQuestUI(state == QuestState.COMPLETE, fusionQuest, 0);
                    break;
                case "Kill":
                    switch(mainCard["QuestTarget"])
                    {
                        case "ExpressionlessBubble":
                            state= QuestState.EXPRESSIONLESS;
                            expressionlessQuest = int.Parse(mainCard["QuestCount"]);
                            ui.UpdateQuestUI(state == QuestState.COMPLETE, expressionlessQuest, 0);
                            break;
                        case "Smile":
                            state = QuestState.SMILE;
                            smileQuest = int.Parse(mainCard["QuestCount"]);
                            ui.UpdateQuestUI(state == QuestState.COMPLETE, smileQuest, 0);
                            break;
                        default:
                            Debug.Log("추가되지 않은 퀘스트 타겟: " + mainCard["QuestTarget"]);
                            break;
                    }
                    break;
                case "Skill":
                    state = QuestState.SKILL;
                    skillQuest = int.Parse(mainCard["QuestCount"]);
                    ui.UpdateQuestUI(state == QuestState.COMPLETE, skillQuest, 0);
                    break;
                case "Possession":
                    state = QuestState.POSSESSION;
                    possessionQuest = int.Parse(mainCard["QuestCount"]);
                    ui.UpdateQuestUI(state == QuestState.COMPLETE, possessionQuest, 0);
                    break;
                default:
                    Debug.Log("추가되지 않은 퀘스트: " + mainCard["QuestCount"]);
                    break;
            }
            Debug.Log(state + "에 대한 퀘스트가 진행됩니다.");
        }
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
    public enum QuestState
    {
        FUSION = 0,
        EXIST = 1,
        SMILE = 2,
        EXPRESSIONLESS = 3,
        SKILL = 4,
        POSSESSION = 5,
        COMPLETE = 6
    };

    private QuestState state;

    private int fusionQuest;
    private int existQuest;
    private int smileQuest;
    private int expressionlessQuest;
    private int skillQuest;
    private int possessionQuest;

    public void CheckQuest(int mission)
    {
        Debug.Log(state);
        Debug.Log(mission);

        if (state == QuestState.FUSION)
        {
            checkFusion(mission);
        }
        else if (state == QuestState.EXIST)
        {
            checkExist(mission);
        }
        else if (state == QuestState.SMILE)
        {
            checkSmile(mission);
        }
        else if (state == QuestState.EXPRESSIONLESS)
        {
            checkExpressionless(mission);
        }
        else if (state == QuestState.SKILL)
        {
            checkSkill(mission);
        }
        else if (state == QuestState.POSSESSION)
        {
            checkPossession(mission);
        }
    }

    public void checkFusion(int fusion)
    {
        //이미 완료했으면 말고
        if (state != QuestState.FUSION)
        {
            return;
        }

        if (fusion >= fusionQuest)
            giveReward();

        ui.UpdateQuestUI(state == QuestState.COMPLETE, fusionQuest, fusion);
    }

    public void checkExist(int exist)
    {
        if (state != QuestState.EXIST)
        {
            return;
        }

        if (exist >= existQuest)
            giveReward();

        ui.UpdateQuestUI(state == QuestState.COMPLETE, existQuest, exist);
    }

    public void checkSmile(int smile)
    {
        if (state != QuestState.SMILE)
        {
            return;
        }

        if (smile >= smileQuest)
            giveReward();

        ui.UpdateQuestUI(state == QuestState.COMPLETE, smileQuest, smile);
    }

    public void checkExpressionless(int expressionless)
    {
        Debug.Log("check Expressionless");
        if (state != QuestState.EXPRESSIONLESS)
        {
            return;
        }
        Debug.Log(expressionless);
        if (expressionless >= expressionlessQuest)
            giveReward();
        Debug.Log(state == QuestState.COMPLETE);
        Debug.Log(expressionlessQuest);
        Debug.Log(expressionless);
        ui.UpdateQuestUI(state == QuestState.COMPLETE, expressionlessQuest, expressionless);
    }

    public void checkSkill(int skill)
    {
        if (state != QuestState.SKILL)
        {
            return;
        }

        if (skill >= skillQuest)
            giveReward();

        ui.UpdateQuestUI(state == QuestState.COMPLETE, skillQuest, skill);
    }

    public void checkPossession(int possession)
    {
        Debug.Log("checkPossession: " + possession);
        Debug.Log(possessionQuest);
        if (state != QuestState.POSSESSION)
        {
            Debug.Log("state 오류");
            return;
        }

        if (possession >= possessionQuest)
            giveReward();

        ui.UpdateQuestUI(state == QuestState.COMPLETE, possessionQuest, possession);
    }

    private void giveReward()
    {
        state = QuestState.COMPLETE;

        if (!mainCard["RewardTarget"].Equals("null")) //디버프 대상이 존재하면
        {
            switch (mainCard["RewardAbility"])
            {
                case "Gold":
                    GameManager.Instance.Coin(int.Parse(mainCard["RewardValue"]));
                    break;
                case "CoolTime":
                    Debug.Log("스킬이 아직 없으니 골드를 대신 드리죠");
                    GameManager.Instance.Coin(10000);
                    break;
                case "Damaged":
                    InGameData.Instance.BuffExpressionlessEnemyDamaged(int.Parse(mainCard["RewardValue"]));
                    break;
                case "AttackSpeed":
                    Debug.Log("일단 일괄 적용"); //나중에 코드 합하면서 야성발현 타워만 모아서 버프 주죠
                    break;
                default:
                    Debug.Log("아직 추가되지 않은 보상 종류: " + mainCard["RewardAbility"]);
                    break;
            }
        }
        Debug.Log(mainCard["RewardTarget"] + "지급");
    }
}
