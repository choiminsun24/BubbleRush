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
        if (fade.activeSelf == false) //�� �ϴ°���
        {
            fade.SetActive(true);
        }

        fade.GetComponent<Fade>().SceneIn();
    }

    //Fade ���� ���� ��
    //public void SceneOut()
    //{
    //    SceneManager.LoadScene("Lobby");
    //}
}
