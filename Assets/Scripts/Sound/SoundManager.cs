using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;

    static public SoundManager Instance
    {
        get
        {
            if (instance == null)
                return null;

            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        effectSlider.value = PlayerPrefs.GetFloat("EffectVolume", 0.75f);
        Screen.SetResolution(1920, 1080,FullScreenMode.MaximizedWindow);
    }

    //????
    [Header("Sound Manager")]
    public AudioClip bubblePop;
    public AudioClip lobby;
    public AudioClip inGame;
    public AudioClip inStage;
    public AudioClip[] towerInstall, tower1Install, tower2Install, skillBite;
    public AudioClip card; //0:Ïπ¥Îìú ?Ñ†?ÉùÏ∞? 1:Î≤ÑÌîÑ   2:?îîÎ≤ÑÌîÑ   3:Î¶¨Ïõå?ìú
    public AudioClip die;
    public AudioClip[] kill;
    public AudioClip popUpClose;
    public AudioClip win;

    //???? ????
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;
    [SerializeField] private Slider effectSlider;

    public void popCloseSound()
    {
        effect.clip = popUpClose;
        effect.Play();
    }

    public AudioClip selectKillSound()
    {
        int r = Random.Range(0, kill.Length);
        return kill[r];
    }

    public void SetBGMLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void SetEffectLevel(float sliderValue)
    {
        mixer.SetFloat("EffectVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("EffectVolume", sliderValue);
    }

    //???? ????
    [SerializeField] private Text onOff;
    public void SetVibration()
    {
        switch (onOff.text)
        {
            case "ON":
                onOff.text = "OFF";

                break;
            case "OFF":
                onOff.text = "ON";
                break;
        }
    }

    //???? ????
    public AudioSource bgm;
    public AudioSource effect;

    public void BGMStop()
    {
        bgm.Stop();
    }

    public void BGMToLobby()
    {
        bgm.clip = lobby;
        bgm.Play();
    }

    public void BGMToInGame()
    {
        bgm.clip = inGame;
        bgm.Play();
    }

    public void BGMPlay(AudioClip audio)
    {
        bgm.clip = audio;
        bgm.Play();
    }
    public void EffectPlay(AudioClip audio)
    {
        effect.clip = audio;
        effect.Play();
    }
}
