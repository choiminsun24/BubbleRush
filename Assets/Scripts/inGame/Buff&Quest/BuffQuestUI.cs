using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffQuestUI : MonoBehaviour
{
    //ÂüÁ¶
    public GameObject ui;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonDown()
    {
        ui.SetActive(true);
    }

    public void ButtonUp()
    {
        ui.SetActive(false);
    }
}
