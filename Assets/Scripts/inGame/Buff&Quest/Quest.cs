using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    //������
    public InGameData data;
    private List<Dictionary<string, string>> textData;

    //UI ����
    public GameObject Box; //UIâ
    public QuestCard[] position; //���� ��ġ
    public GameObject my; //���õ� ī�� Panel
    public QuestCard mine; //���õ� ī��

    public UIManager ui;

    //������
    private int[] num; //���õ� ��ȣ

    //���α׷�
    private static Quest _instance;

    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static Quest Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
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
        // �ν��Ͻ��� �����ϴ� ��� ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            Destroy(this);
        }

        textData = ExelReader.Read("Data/inGame/QuestTest"); //����Ʈ ������ �޾ƿ���

        //GameManager Start���� ������
        Box.SetActive(false);
        my.SetActive(false);
    }

    //������ on
    public void play()
    {
        Box.SetActive(true);
        ui.Blind(true);
        GameManager.Instance.playingCardOn(); //ī�� ���� ��

        //1. �������� �� �̱�
        num = new int[] { -1, -1, -1 };

        for (int i = 0; i < num.Length; i++)
        {
            //����
            num[i] = UnityEngine.Random.Range(0, textData.Count);

            //�ߺ� �˻�
            for (int j = 0; j < i; j++)
            {
                if (num[j] == num[i])
                {
                    i--;
                    break;
                }
            }
        }

        //2. ī�� UI ����
        for (int i = 0; i < num.Length; i++)
        {
            position[i].cardSetting(textData[num[i]]);
        }
    }

    //������ ī��, ������ ����
    private void cardSetting(Transform tf, Dictionary<string, string> target)
    {
        tf.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(target["Directory"]);
        tf.GetChild(2).GetComponent<Text>().text = target["Name"]; //Title
        tf.GetChild(3).GetComponent<Text>().text = target["QuestDescription"]; //Content
        tf.GetChild(4).GetComponent<Text>().text = target["RewardDescription"]; //Content
    }


    Dictionary<string, string> mainCard;

    //���� �� ó��
    public void choice(int n) //ī�� ���� �� ����� �޼ҵ�
    {
        Box.SetActive(false); //���� â ����
        ui.Blind(false);
        GameManager.Instance.playingCardOff(); //ī�� ���� ����

        mainCard = textData[num[n]]; //���õ� ��
        mine.cardSetting(mainCard);


        //���� ���� ȿ�� **************************�� ���� ������***********************
        if (!mainCard["QuestTarget"].Equals("null")) //����Ʈ ��� ����
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
                            Debug.Log("�߰����� ���� ����Ʈ Ÿ��: " + mainCard["QuestTarget"]);
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
                    Debug.Log("�߰����� ���� ����Ʈ: " + mainCard["QuestCount"]);
                    break;
            }
            Debug.Log(state + "�� ���� ����Ʈ�� ����˴ϴ�.");
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

    //����Ʈ �޼�
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
        //�̹� �Ϸ������� ����
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
            Debug.Log("state ����");
            return;
        }

        if (possession >= possessionQuest)
            giveReward();

        ui.UpdateQuestUI(state == QuestState.COMPLETE, possessionQuest, possession);
    }

    private void giveReward()
    {
        state = QuestState.COMPLETE;

        if (!mainCard["RewardTarget"].Equals("null")) //����� ����� �����ϸ�
        {
            switch (mainCard["RewardAbility"])
            {
                case "Gold":
                    GameManager.Instance.Coin(int.Parse(mainCard["RewardValue"]));
                    break;
                case "CoolTime":
                    Debug.Log("��ų�� ���� ������ ��带 ��� �帮��");
                    GameManager.Instance.Coin(10000);
                    break;
                case "Damaged":
                    InGameData.Instance.BuffExpressionlessEnemyDamaged(int.Parse(mainCard["RewardValue"]));
                    break;
                case "AttackSpeed":
                    Debug.Log("�ϴ� �ϰ� ����"); //���߿� �ڵ� ���ϸ鼭 �߼����� Ÿ���� ��Ƽ� ���� ����
                    break;
                default:
                    Debug.Log("���� �߰����� ���� ���� ����: " + mainCard["RewardAbility"]);
                    break;
            }
        }
        Debug.Log(mainCard["RewardTarget"] + "����");
    }
}
