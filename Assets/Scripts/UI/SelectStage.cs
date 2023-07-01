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
        if (stagePanel.activeSelf == false) //���� ����â�Դϴ�. Home���� ���ư���
        {
            //fadeOut���� �� �̵�
            if (fadePanel.activeSelf == false)
            {
                fadePanel.SetActive(true);
            }
            fadePanel.GetComponent<Fade>().SceneOut("Home");
        }
        else if (levelPanel.activeSelf == false) //�������� ����â�Դϴ�. ���� �������� ���ư���
        {
            anim.SetInteger("num", 0);
            Invoke("CloseSelectStage", 0.8f);
            worldPanel.SetActive(true);
        }
        else if (levelPanel.activeSelf == true) //���̵� ����â�Դϴ�. �������� �������� ���ư���.
        {
            levelPanel.SetActive(false);
        }
    }

    public void SelectWorld(int num) //�� �������� �� ���� �ִϸ��̼�.
    {
        //���� ���� ����
        worldNum = num;
        worldPanel.SetActive(false);

        //�������� �������� ȭ�� ��ȯ
        anim.SetInteger("num", worldNum);
        Invoke("OpenSelectStage", 0.8f);

        //switch (worldNum) //���� ���忡 ���� �������� ����â
        //{
        //    case 1:
        //        anim.SetInteger("num", 1);
        //        stage[0].SetActive(true);
        //        Invoke("OpenSelectStage", 0.8f);
        //        break;
        //    case 2:
        //        anim.SetInteger("num", 2);
        //        stage[1].SetActive(true);
        //        Invoke("OpenSelectStage", 0.8f);
        //        break;
        //    case 3:
        //        anim.SetInteger("num", 3);
        //        stage[2].SetActive(true);
        //        Invoke("OpenSelectStage", 0.8f);
        //        break;
        //    case 4:
        //        anim.SetInteger("num", 4);
        //        stage[3].SetActive(true);
        //        Invoke("OpenSelectStage", 0.8f);
        //        break;
        //}
    }

    private void OpenSelectStage()
    {
        background.material = blur;
        stagePanel.SetActive(true);
        stage[worldNum - 1].SetActive(true);
    }

    private void CloseSelectStage()
    {
        background.material = nonBlur;
        stagePanel.SetActive(false);

        for (int i = 0; i < stage.Length; i++)
        {
            stage[i].SetActive(false);
        }
    }

    public void selectStage(int num)
    {
        //�������� ���� ����
        stageNum = num;
        CloseSelectStage();

        //���̵� �������� ȭ�� ��ȯ
        levelPanel.SetActive(true);
    }

    public void SelectLevel()
    {

    }
}
