using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 하트 개수 업데이트
    [SerializeField] private GameObject[] hearts;

    Animator anim;

    private int heartTopIndex; //hearts의 top
    public void plusHeart()
    {
        //3개 미만이면 충전
        if (heartTopIndex < hearts.Length - 1)
        {
            hearts[heartTopIndex + 1].GetComponent<Animator>().SetBool("live", true);
        }

        heartTopIndex++;
    }

    public void minusHeart()
    {
        //하트가 있으면 제거
        if (heartTopIndex >= 0)
        {
            hearts[heartTopIndex].GetComponent<Animator>().SetBool("live", false);
        }

        heartTopIndex--;
    }

    // 스테이지 코인 관리
    public Text Coin;

    public void UpdateStageCoin(int coin)
    {
        Coin.text = coin.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        heartTopIndex = hearts.Length - 1;
        Blind(false);
        QuestComplete.SetActive(false);
        QuestProgress.SetActive(false);

        //Invoke("test", 3.0f);
        
    }

    public void QuestUItest()
    {
        UpdateQuestUI(true, 0, 0);
    }

    public GameObject blind;

    public void Blind(bool state)
    {
        if (!blind.activeSelf || !state)
            blind.SetActive(state);


    }

    public GameObject WinPanel;

    public void Win()
    {
        WinPanel.SetActive(true);
        SoundManager.Instance.EffectPlay(SoundManager.Instance.win);
    }

    public GameObject gameOverWindow;

    // 게임 오버에서 홈 화면
    public void GoToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Home");
    }
    // 게임 오버에서 Retry
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //GameManager.Instance.StartCore();
    }

    [SerializeField] private Text curNum;
    // 라운드 관리
    public void UpdateRound(int _num)
    {
        curNum.text = (_num).ToString();
    }
    public GameObject nextRoundBtn;
    public GameObject fastButton;
    public Image fastImage;
    public Sprite fast1;
    public Sprite fast2;

    public void onFast()
    {
        nextRoundBtn.SetActive(false);
        fastButton.SetActive(true);
    }

    public void offFast()
    {
        nextRoundBtn.SetActive(true);
        fastButton.SetActive(false);
    }

    public void updateFast(int level)
    {
        if (level == 1)
        {
            fastImage.sprite = fast1;
        }
        else
        {
            fastImage.sprite = fast2;
        }
    }

    public GameObject QuestProgress;
    public GameObject QuestComplete;
    public Image questIcon;

    public Text goal;
    public Text now;

    public void UpdateQuestUI(bool complete, int goal, int now)
    {
        if (!complete)
        {
            QuestProgress.SetActive(true);
            QuestComplete.SetActive(false);

            this.now.text = now.ToString();
            this.goal.text = goal.ToString();
        }
        else
        {
            QuestProgress.SetActive(false);
            colorDown(questIcon);
            QuestComplete.SetActive(true);
        }
    }


    // 설정 화면 바깥 터치 시 종료
    [SerializeField] private GameObject settingWindow;

    private void TouchOutside(GameObject _window)
    {
        if (Input.GetMouseButton(0) && _window.activeSelf)
        {
            print("Touch Outside with " + _window.name);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);            
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPos, Camera.main.transform.forward);
            if (hitInformation.collider != null)
            {
                //We should have hit something with a 2D Physics collider!
                GameObject touchedObject = hitInformation.transform.gameObject;
                //touchedObject should be the object someone touched.
                if(touchedObject == _window)
                {
                    _window.SetActive(false);
                }
            }
        }
    }

    private void colorDown(Image target)
    {
        float b = 0.6f;
        Color dark = new Color(b, b, b, 1.0f);

        target.color = dark;

        target.raycastTarget = false;
    }
}
