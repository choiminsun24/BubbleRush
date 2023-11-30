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
    private bool complete = false;

    //���α׷�

    public void Awake()
    {
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

    //���� �� ó��
    public void choice(int n) //ī�� ���� �� ����� �޼ҵ�
    {
        Box.SetActive(false); //���� â ����
        ui.Blind(false);
        GameManager.Instance.playingCardOff(); //ī�� ���� ����

        Dictionary<string, string> choice = textData[num[n]]; //���õ� ��
        mine.cardSetting(choice);


        //���� ���� ȿ�� **************************�� ���� ������***********************
        if (!choice["QuestTarget"].Equals("null")) //����Ʈ ��� ����
        {
            switch(choice["QuestType"])
            {
                case "Fusion":
                    fusionQuest = int.Parse(choice["QuestCount"]);
                    break;
                case "Kill":
                    switch(choice["QuestTarget"])
                    {
                        case "ExpressionlessBubble":
                            expressionlessQuest = int.Parse(choice["QuestCount"]);
                            break;
                        case "Smile":
                            smileQuest = int.Parse(choice["QuestCount"]);
                            break;
                        default:
                            Debug.Log("�߰����� ���� ����Ʈ Ÿ��: " + choice["QuestTarget"]);
                            break;
                    }
                    break;
                case "Skill":
                    skillQuest = int.Parse(choice["QuestCount"]);
                    break;
                case "Possession":
                    possessionQuest = int.Parse(choice["QuestCount"]);
                    break;
                default:
                    Debug.Log("�߰����� ���� ����Ʈ: " + choice["QuestCount"]);
                    break;
            }
            Debug.Log(choice["QuestType"] + "�� ���� ����Ʈ�� ����˴ϴ�.");
        }

        if (!choice["RewardTarget"].Equals("null")) //����� ����� �����ϸ�
        {
            Debug.Log(choice["RewardTarget"] + "�� ���� ����Ʈ�� ����˴ϴ�: ���� ���� ����");
        }

        //data.BuffATKS(1.06f); ���ù�
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
        //�̹� �Ϸ������� ����
        if (complete)
        {
            return;
        }

        if (fusion >= fusionQuest)
            complete = true;

        ui.UpdateQuestUI(complete);
    }

    public void checkExist(int exist)
    {
        //�̹� �Ϸ������� ����
        if (complete)
        {
            return;
        }

        if (exist >= existQuest)
            complete = true;

        ui.UpdateQuestUI(complete);
    }

    public void checkSmile(int smile)
    {
        //�̹� �Ϸ������� ����
        if (complete)
        {
            return;
        }

        if (smile >= smileQuest)
            complete= true;

        ui.UpdateQuestUI(complete);
    }

    public void checkExpressionless(int expressionless)
    {
        //�̹� �Ϸ������� ����
        if (complete)
        {
            return;
        }

        if (expressionless >= expressionlessQuest)
            complete = true;

        ui.UpdateQuestUI(complete);
    }

    public void checkSkill(int skill)
    {
        //�̹� �Ϸ������� ����
        if (complete)
        {
            return;
        }

        if (skill >= skillQuest)
            complete = true;

        ui.UpdateQuestUI(complete);
    }

    public void checkPossession(int possession)
    {
        //�̹� �Ϸ������� ����
        if (complete)
        {
            return;
        }

        if (possession >= possessionQuest)
            complete = true;

        ui.UpdateQuestUI(complete);
    }
}
