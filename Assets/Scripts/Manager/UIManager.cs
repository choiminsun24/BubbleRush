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

    private int point; //hearts의 top

    public void plusHeart()
    {
        //3개 미만이면 충전
        if (point < hearts.Length - 1)
        {
            hearts[point + 1].GetComponent<Animator>().SetBool("live", true);
            //hearts[point + 1].gameObject.SetActive(true);
        }

        point++;
    }

    public void minusHeart()
    {
        //하트가 있으면 제거
        if (point >= 0)
        {
            hearts[point].GetComponent<Animator>().SetBool("live", false);
            //hearts[point].gameObject.SetActive(false);
        }

        point--;
    }

    // 스테이지 코인 관리
    public Text Coin;

    public void UpdateStageCoin(int coin)
    {
        Coin.text = coin.ToString();
    }

    //// 배경음악 볼륨 조절
    //[SerializeField] private AudioMixer mixer;
    //[SerializeField] private Slider slider;
    //public void SetBGMLevel(float sliderValue)
    //{
    //    mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue)*20);
    //    PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    //}
    
    //// 효과음 볼륨 조절
    //[SerializeField] private Slider effectSlider;
    //public void SetEffectLevel(float sliderValue)
    //{
    //    mixer.SetFloat("EffectVolume", Mathf.Log10(sliderValue)*20);
    //    PlayerPrefs.SetFloat("EffectVolume", sliderValue);
    //}

    // Start is called before the first frame update
    void Start()
    {
        point = hearts.Length - 1;
        Blind(false);
        //slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        //effectSlider.value = PlayerPrefs.GetFloat("EffectVolume", 0.75f);
    }

    //// 진동 조절
    //[SerializeField] private Text onOff;
    //public void SetVibration()
    //{
    //    switch(onOff.text)
    //    {
    //        case "ON":
    //            onOff.text = "OFF";

    //        break;
    //        case "OFF":
    //            onOff.text = "ON";
    //        break;
    //    }
    //}

    public GameObject blind;

    public void Blind(bool state)
    {
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

    public void UpdateQuestUI(bool complete)
    {
        Debug.Log(complete);

        if (!complete)
        {
            //퀘스트 진행중이면 이렇게
        }
        else
        {
            //퀘스트 완료했으면 이렇게
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
}
