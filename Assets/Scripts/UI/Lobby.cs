using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    public GameObject fade;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("두 번째 로드엔 안되나");

        if (fade.activeSelf == false) //왜 하는거지
        {
            fade.SetActive(true);
        }

        fade.GetComponent<Fade>().SceneIn();
    }

    public void KnowledgeScene()
    {

    }


}
