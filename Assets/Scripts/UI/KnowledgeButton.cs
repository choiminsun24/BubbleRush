using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnowledgeButton : MonoBehaviour
{
    Transform tf;
    Button btn;

    void Start()
    {
        tf = GetComponent<Transform>();
        btn = GetComponent<Button>();
    }

    void Update()
    {
        if (btn.interactable == true)
        {
            tf.GetChild(0).gameObject.SetActive(false);
        }
    }
}
