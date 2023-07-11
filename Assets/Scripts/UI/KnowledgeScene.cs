using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KnowledgeScene : MonoBehaviour
{
    public GameObject fade;

    private DataManager dataManager;

    public Text money;

    public Button[] ATKUp;
    public Button[] ATKSUp;
    public Button[] ATKRUp;

    // Start is called before the first frame update
    void Awake() //���׷��̵� ��ư Ȱ��ȭ
    {
        dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();

        //��ȭ ����
        money.text = dataManager.KnowledgeCoin.ToString();

        //��ư ���� - ���׷��̵� �Ұ� : ��Ȱ��ȭ
        //          - ���׷��̵� �Ϸ� + ���� : Ȱ��ȭ 
        for (int i = 0; i <= dataManager.KnowATK; i++)
        {
            ATKUp[i].interactable = true;
        }

        for (int i = 0; i <= dataManager.KnowATKS; i++)
        {
            ATKSUp[i].interactable = true;
        }

        for (int i = 0; i <= dataManager.KnowATKR; i++)
        {
            ATKRUp[i].interactable = true;
        }
    }

    void Start()
    {
        //���̵� ��
        if (fade.activeSelf == false) //�� �ϴ°���
        {
            fade.SetActive(true);
        }

        fade.GetComponent<Fade>().SceneIn();
    }

    //���׷��̵� �Ϸ� ��ư: �ȳ� �˾� / ���׷��̵� : ���׷��̵�
    public void UpgradeATK(int i)
    {
        //���׷��̵� ����
        if (i < dataManager.KnowATK) //�̹� ���׷��̵� �� ��ư
        {
            Debug.Log("�̹� �Ϸ��"); ;
        }
        else if (dataManager.KnowATK == i) //���׷��̵� �� ����
        {
            dataManager.UpgradeKnowATK();
            try
            {
                ATKUp[i + 1].interactable = true;
            }
            catch (System.IndexOutOfRangeException ex) //������ ������Ʈ ��ư�� ���� ���
            {
                ;
            }
        }
        else
        {
            Debug.Log("����: ���׷��̵� ���� ���°� �ƴϹǷ� �ش� ��ư�� ���� �� �����.");
        }
    }

    public void UpgradeATKS(int i)
    {
        //���׷��̵� ����
        if (i < dataManager.KnowATKS) //�̹� ���׷��̵� �� ��ư
        {
            Debug.Log("�̹� �Ϸ��"); ;
        }
        else if (dataManager.KnowATKS == i) //���׷��̵� �� ����
        {
            dataManager.UpgradeKnowATKS();
            try
            {
                ATKSUp[i + 1].interactable = true;
            }
            catch (System.IndexOutOfRangeException ex) //������ ������Ʈ ��ư�� ���� ���
            {
                ;
            }
        }
        else
        {
            Debug.Log("����: ���׷��̵� ���� ���°� �ƴϹǷ� �ش� ��ư�� ���� �� �����.");
        }
    }

    public void UpgrageATKR(int i)
    {
        //���׷��̵� ����
        if (i < dataManager.KnowATKR) //�̹� ���׷��̵� �� ��ư
        {
            Debug.Log("�̹� �Ϸ��");
        }
        else if (dataManager.KnowATKR == i) //���׷��̵� �� ����
        {
            dataManager.UpgradeKnowATKR();
            try
            {
                ATKRUp[i + 1].interactable = true;
            }
            catch (System.IndexOutOfRangeException ex) //������ ������Ʈ ��ư�� ���� ���
            {
                ;
            }
        }
        else
        {
            Debug.Log("����: ���׷��̵� ���� ���°� �ƴϹǷ� �ش� ��ư�� ���� �� �����.");
        }
    }

    public void TestButton()
    {
        Debug.Log(dataManager.KnowATK);
        Debug.Log(dataManager.KnowATKS);
        Debug.Log(dataManager.KnowATKR);
    }

    

    //Fade ���� ���� ��
    //public void SceneOut()
    //{
    //    SceneManager.LoadScene("Lobby");
    //}
}
