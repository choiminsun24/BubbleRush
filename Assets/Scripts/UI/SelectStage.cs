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
        if (stagePanel.activeSelf == false && levelPanel.activeSelf == false) //월드 선택창입니다. Home으로 돌아가요
        {
            //fadeOut으로 씬 이동
            if (fadePanel.activeSelf == false)
            {
                fadePanel.SetActive(true);
            }
            fadePanel.GetComponent<Fade>().SceneOut("Home");
        }
        else if (stagePanel.activeSelf == true) //스테이지 선택창입니다. 월드 선택으로 돌아가요
        {
            stage[worldNum - 1].SetActive(false);
            anim.SetInteger("num", 0);
            Invoke("CloseSelectStage", 0.8f);
            
        }
        else if (levelPanel.activeSelf == true) //난이도 선택창입니다. 스테이지 선택으로 돌아가요.
        {
            levelPanel.SetActive(false);
            OpenSelectStage();
        }
    }

    public void SelectWorld(int num) //각 스테이지 판 열고 애니메이션.
    {
        //월드 정보 세팅
        worldNum = num;
        worldPanel.SetActive(false);

        //스테이지 선택으로 화면 전환
        anim.SetInteger("num", worldNum);
        Invoke("OpenSelectStage", 0.8f); //애니메이션 속도 맞춤
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
        //스테이지 정보 세팅
        stageNum = num;
        CloseSelectStage();

        //난이도 선택으로 화면 전환
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
                Debug.Log("초원");
                break;
            case 2:
                Debug.Log("화산");
                break;
            case 3:
                Debug.Log("사막");
                break;
            case 4:
                Debug.Log("빙하");
                break;
        }

        Debug.Log(stageNum + " stage");

        //Debug.Log(levelNum);

        switch (worldNum)
        {
            case 1:
                Debug.Log("쉬움");
                break;
            case 2:
                Debug.Log("보통");
                break;
            case 3:
                Debug.Log("어려움");
                break;
            case 4:
                Debug.Log("지옥");
                break;
        }

        Debug.Log("씬 이동합니다.");

        //////////////////////////////////////////////////////

        /////////////////////// FOR RUN ////////////////////

        //fadeOut으로 씬 이동
        if (fadePanel.activeSelf == false)
        {
            fadePanel.SetActive(true);
        }
        fadePanel.GetComponent<Fade>().SceneOut("map test");

        //////////////////////////////////////////////////////
    }
}
