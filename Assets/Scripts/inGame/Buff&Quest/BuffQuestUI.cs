using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffQuestUI : MonoBehaviour
{
    //ÂüÁ¶
    public GameObject ui;
    public GameObject blind;
    public GameObject blindButton;
    private Image image;
    private Outline line;

    void Start()
    {
        //image = gameObject.GetComponent<image>();
        //line = gameObject.GetComponent<Outline>();
        //line.enabled = false;
        blind.SetActive(false);
        blindButton.SetActive(false);
    }

    public void ButtonDown()
    {
        ui.SetActive(true);
        Time.timeScale = 0.5f;
        //line.enabled = true;
        blind.SetActive(true);
        blindButton?.SetActive(true);

        //image
    }

    public void ButtonUp()
    {
        ui.SetActive(false);
        Time.timeScale = 1f;
        //line.enabled = false;
        blind.SetActive(false);
        blindButton.SetActive(false);
    }
}
