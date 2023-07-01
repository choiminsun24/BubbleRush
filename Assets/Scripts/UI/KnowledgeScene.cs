using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KnowledgeScene : MonoBehaviour
{
    public GameObject fade;

    // Start is called before the first frame update
    void Start()
    {
        if (fade.activeSelf == false) //왜 하는거지
        {
            fade.SetActive(true);
        }

        fade.GetComponent<Fade>().SceneIn();
    }

    //Fade 없이 나갈 때
    //public void SceneOut()
    //{
    //    SceneManager.LoadScene("Lobby");
    //}
}
