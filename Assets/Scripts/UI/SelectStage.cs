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

        if (fadePanel.activeSelf == false) //꺼져있으면 켜서 fade처리
        {
            fadePanel.SetActive(true);
        }

        fadePanel.GetComponent<Fade>().SceneIn();
    }

    //이전 버튼
    public void BackButton()
    {
        if (stagePanel.activeSelf == false) //월드 선택창입니다. Home으로 돌아가요
        {
            //fadeOut으로 씬 이동
            if (fadePanel.activeSelf == false)
            {
                fadePanel.SetActive(true);
            }
            fadePanel.GetComponent<Fade>().SceneOut("Home");
        }
        else if (levelPanel.activeSelf == false) //스테이지 선택창입니다. 월드 선택으로 돌아가요
        {
            anim.SetInteger("num", 0);
            Invoke("CloseSelectStage", 0.8f);
            worldPanel.SetActive(true);
        }
        else if (levelPanel.activeSelf == true) //난이도 선택창입니다. 스테이지 선택으로 돌아가요.
        {
            levelPanel.SetActive(false);
        }
    }

    public void SelectWorld(int num) //각 스테이지 판 열고 애니메이션.
    {
        //월드 정보 세팅
        worldNum = num;
        worldPanel.SetActive(false);

        //스테이지 선택으로 화면 전환
        anim.SetInteger("num", worldNum);
        Invoke("OpenSelectStage", 0.8f);

        //switch (worldNum) //선택 월드에 따른 스테이지 선택창
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
        //스테이지 정보 세팅
        stageNum = num;
        CloseSelectStage();

        //난이도 선택으로 화면 전환
        levelPanel.SetActive(true);
    }

    public void SelectLevel()
    {

    }
}
