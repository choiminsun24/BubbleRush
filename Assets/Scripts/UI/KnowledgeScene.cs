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
    void Awake() //업그레이드 버튼 활성화
    {
        dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();

        //재화 세팅
        money.text = dataManager.KnowledgeCoin.ToString();

        //버튼 세팅 - 업그레이드 불가 : 비활성화
        //          - 업그레이드 완료 + 가능 : 활성화 
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
        //페이드 인
        if (fade.activeSelf == false) //왜 하는거지
        {
            fade.SetActive(true);
        }

        fade.GetComponent<Fade>().SceneIn();
    }

    //업그레이드 완료 버튼: 안내 팝업 / 업그레이드 : 업그레이드
    public void UpgradeATK(int i)
    {
        //업그레이드 상태
        if (i < dataManager.KnowATK) //이미 업그레이드 된 버튼
        {
            Debug.Log("이미 완료됨"); ;
        }
        else if (dataManager.KnowATK == i) //업그레이드 될 차례
        {
            dataManager.UpgradeKnowATK();
            try
            {
                ATKUp[i + 1].interactable = true;
            }
            catch (System.IndexOutOfRangeException ex) //마지막 업데이트 버튼이 눌릴 경우
            {
                ;
            }
        }
        else
        {
            Debug.Log("오류: 업그레이드 가능 상태가 아니므로 해당 버튼은 눌릴 수 없어요.");
        }
    }

    public void UpgradeATKS(int i)
    {
        //업그레이드 상태
        if (i < dataManager.KnowATKS) //이미 업그레이드 된 버튼
        {
            Debug.Log("이미 완료됨"); ;
        }
        else if (dataManager.KnowATKS == i) //업그레이드 될 차례
        {
            dataManager.UpgradeKnowATKS();
            try
            {
                ATKSUp[i + 1].interactable = true;
            }
            catch (System.IndexOutOfRangeException ex) //마지막 업데이트 버튼이 눌릴 경우
            {
                ;
            }
        }
        else
        {
            Debug.Log("오류: 업그레이드 가능 상태가 아니므로 해당 버튼은 눌릴 수 없어요.");
        }
    }

    public void UpgrageATKR(int i)
    {
        //업그레이드 상태
        if (i < dataManager.KnowATKR) //이미 업그레이드 된 버튼
        {
            Debug.Log("이미 완료됨");
        }
        else if (dataManager.KnowATKR == i) //업그레이드 될 차례
        {
            dataManager.UpgradeKnowATKR();
            try
            {
                ATKRUp[i + 1].interactable = true;
            }
            catch (System.IndexOutOfRangeException ex) //마지막 업데이트 버튼이 눌릴 경우
            {
                ;
            }
        }
        else
        {
            Debug.Log("오류: 업그레이드 가능 상태가 아니므로 해당 버튼은 눌릴 수 없어요.");
        }
    }

    public void TestButton()
    {
        Debug.Log(dataManager.KnowATK);
        Debug.Log(dataManager.KnowATKS);
        Debug.Log(dataManager.KnowATKR);
    }

    

    //Fade 없이 나갈 때
    //public void SceneOut()
    //{
    //    SceneManager.LoadScene("Lobby");
    //}
}
