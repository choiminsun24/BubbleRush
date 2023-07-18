using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStage : MonoBehaviour
{
    //background
    public Material nonBlur;
    public Material blur;
    public SpriteRenderer background;

    //SelectPanel
    public GameObject worldPanel;
    public GameObject stagePanel;
    public GameObject[] stage = new GameObject[4];
    public GameObject levelPanel;

    //Data
    int worldNum;
    int stageNum;
    int levelNum;

    //GameObject
    public GameObject fadePanel;
    public Animator anim;

    void Start()
    {
        stagePanel.SetActive(false);
        levelPanel.SetActive(false);

        if (fadePanel.activeSelf == false) //���������� �Ѽ� fadeó��
        {
            fadePanel.SetActive(true);
        }

        fadePanel.GetComponent<Fade>().SceneIn();
    }

    //���� ��ư
    public void BackButton()
    {
        if (stagePanel.activeSelf == false && levelPanel.activeSelf == false) //���� ����â�Դϴ�. Home���� ���ư���
        {
            //fadeOut���� �� �̵�
            if (fadePanel.activeSelf == false)
            {
                fadePanel.SetActive(true);
            }
            fadePanel.GetComponent<Fade>().SceneOut("Home");
        }
        else if (stagePanel.activeSelf == true) //�������� ����â�Դϴ�. ���� �������� ���ư���
        {
            stage[worldNum - 1].SetActive(false);
            anim.SetInteger("num", 0);
            Invoke("CloseSelectStage", 0.8f);
            
        }
        else if (levelPanel.activeSelf == true) //���̵� ����â�Դϴ�. �������� �������� ���ư���.
        {
            levelPanel.SetActive(false);
            OpenSelectStage();
        }
    }

    public void SelectWorld(int num) //�� �������� �� ���� �ִϸ��̼�.
    {
        //���� ���� ����
        worldNum = num;
        worldPanel.SetActive(false);

        //�������� �������� ȭ�� ��ȯ
        anim.SetInteger("num", worldNum);
        Invoke("OpenSelectStage", 0.8f); //�ִϸ��̼� �ӵ� ����
    }

    private void OpenSelectStage()
    {
        background.material = blur;
        stagePanel.SetActive(true);
        stage[worldNum - 1].SetActive(true);
    }

    private void CloseSelectStage()
    {
        //for (int i = 0; i < stage.Length; i++)
        //{
        //    stage[i].SetActive(false);
        //}
        background.material = nonBlur;
        stagePanel.SetActive(false);
        worldPanel.SetActive(true);
    }

    public void selectStage(int num)
    {
        //�������� ���� ����
        stageNum = num;
        CloseSelectStage();

        //���̵� �������� ȭ�� ��ȯ
        levelPanel.SetActive(true);
    }

    public void SelectLevel(int num)
    {
        levelNum = num;

        ///////////////////////  FOR TEST /////////////////////

        //Debug.Log(worldNum);

        switch (worldNum)
        {
            case 1:
                Debug.Log("�ʿ�");
                break;
            case 2:
                Debug.Log("ȭ��");
                break;
            case 3:
                Debug.Log("�縷");
                break;
            case 4:
                Debug.Log("����");
                break;
        }

        Debug.Log(stageNum + " stage");

        //Debug.Log(levelNum);

        switch (worldNum)
        {
            case 1:
                Debug.Log("����");
                break;
            case 2:
                Debug.Log("����");
                break;
            case 3:
                Debug.Log("�����");
                break;
            case 4:
                Debug.Log("����");
                break;
        }

        Debug.Log("�� �̵��մϴ�.");

        //////////////////////////////////////////////////////

        /////////////////////// FOR RUN ////////////////////

        //fadeOut���� �� �̵�
        if (fadePanel.activeSelf == false)
        {
            fadePanel.SetActive(true);
        }
        fadePanel.GetComponent<Fade>().SceneOut("map test");

        //////////////////////////////////////////////////////
    }
}
