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
    }

    //����
    [Header("Sound Manager")]
    public AudioSource bubblePop;
    public AudioClip lobby;
    public AudioClip inGame;

    //���� ����
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;
    [SerializeField] private Slider effectSlider;

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

    //���� ����
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

    //���� ����
    public AudioSource bgm;

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
}