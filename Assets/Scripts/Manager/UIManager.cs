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

    //애니메이션 넣느라 아래 메소드로 바꿨습니다!!
    //public void UpdateHearts(int heart)
    //{
    //    for (int i = 0; i < _hearts; i++)
    //    {
    //        hearts[i].gameObject.SetActive(true);
    //    }
    //    for (int i = _hearts; i < hearts.Length; i++)
    //    {
    //        hearts[i].gameObject.SetActive(false);
    //    }
    //}

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

    public void Blind()
    {
        blind.SetActive(!blind.activeSelf);
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
        curNum.text = (_num+1).ToString();
    }
    public GameObject nextRoundBtn;


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
