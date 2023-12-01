using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    public Image blackPanel;
    Color color;

    public void SceneOut(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    public void SceneIn()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut(string sceneName)
    {
        color = blackPanel.color;

        while (color.a < 1f)
        {
            color.a += 1.5f * Time.deltaTime; //fade 속도 조절
            blackPanel.color = color;

            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeIn()
    {
        color = blackPanel.color;

        while (color.a > 0f)
        {
            color.a -= 1.5f * Time.deltaTime; //fade 속도 조절
            blackPanel.color = color;

            yield return null;
        }

        blackPanel.gameObject.SetActive(false);
    }
}
