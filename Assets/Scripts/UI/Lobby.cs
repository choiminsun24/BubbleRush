using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    public GameObject fade;

    // Start is called before the first frame update
    void Start()
    {
        if (fade.activeSelf == false) //�� �ϴ°���
        {
            fade.SetActive(true);
        }

        fade.GetComponent<Fade>().SceneIn();
    }

    public void KnowledgeScene()
    {

    }


}
