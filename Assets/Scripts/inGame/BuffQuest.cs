using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffQuest : MonoBehaviour
{
    public InGameData data;

    public GameObject[] button;
    public Transform[] position;
    public Transform canvas;

    public GameObject[] choice;

    public void play()
    {
        //1. 랜덤으로 셋 뽑고
            int[] num = new int[] {-1, -1, -1};

            for (int i = 0; i < 3; i++)
            {
                int r = Random.Range(0, button.Length);
                num[i] = r;

                for (int j = 0 ; j < i; j++) //중복 검사
                {
                    if (num[j] == r) //중복이면 다시 뽑기
                    {
                        i--;
                        break;
                    }
                }
            }

            Debug.Log(num[0] + " " + num[1] + " " + num[2]); //확인해따


        //2. Canvas에 설치
        for (int i = 0; i < 3; i++)
        {
            GameObject game = Instantiate(button[num[i]], position[i].position, Quaternion.identity, canvas);
        }

        data.check();
    }

    public void Medicine()
    {
        data.BuffATKS(2f);

        Debug.Log("전투 자극제");
        gameObject.SetActive(false);
    }

    public void ArtificialEyes()
    {
        data.BuffATKR(1.2f);

        Debug.Log("인공눈 이식");
        gameObject.SetActive(false);
    }

    public void BloodFlower()
    {
        data.BuffATK(1.1f);

        Debug.Log("피바라기");
        gameObject.SetActive(false);
    }

    public void PainShadow()
    {
        data.BuffATKS(0.7f);

        Debug.Log("고통의 그림자");
        gameObject.SetActive(false);
    }

    public void Weak()
    {
        data.BuffATK(0.7f);

        Debug.Log("쇠약");
        gameObject.SetActive(false);
    }
}
