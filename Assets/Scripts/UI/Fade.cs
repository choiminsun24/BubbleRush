using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    private int setBtnIdx = 0;
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
        Image settingBtn = SoundManager.Instance.transform.Find("SettingCanvas").Find("SettingBtn").GetComponent<Image>();
        setBtnIdx = setBtnIdx == 0 ? 1 : 0;
        settingBtn.sprite = SoundManager.Instance.setBtnImg[setBtnIdx];

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
