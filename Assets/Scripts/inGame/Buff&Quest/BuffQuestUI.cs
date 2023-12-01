using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffQuestUI : MonoBehaviour
{
    //ÂüÁ¶
    public GameObject ui;
    public UIManager uiManager;
    public GameObject blindButton;
    private Image image;
    private Outline line;

    void Start()
    {
        blindButton.SetActive(false);
    }

    public void ButtonDown()
    {
        ui.SetActive(true);
        Time.timeScale = 0.5f;
        uiManager.Blind(true);
        blindButton.SetActive(true);
        GameManager.Instance.watchingCardOn();
    }

    public void ButtonUp()
    {
        ui.SetActive(false);
        GameManager.Instance.ReleaseGame();
        uiManager.Blind(false);
        blindButton.SetActive(false);
        GameManager.Instance.watchingCardOff();
    }
}
