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
        //1. �������� �� �̰�
            int[] num = new int[] {-1, -1, -1};

            for (int i = 0; i < 3; i++)
            {
                int r = Random.Range(0, button.Length);
                num[i] = r;

                for (int j = 0 ; j < i; j++) //�ߺ� �˻�
                {
                    if (num[j] == r) //�ߺ��̸� �ٽ� �̱�
                    {
                        i--;
                        break;
                    }
                }
            }

            Debug.Log(num[0] + " " + num[1] + " " + num[2]); //Ȯ���ص�


        //2. Canvas�� ��ġ
        for (int i = 0; i < 3; i++)
        {
            GameObject game = Instantiate(button[num[i]], position[i].position, Quaternion.identity, canvas);
        }

        data.check();
    }

    public void Medicine()
    {
        data.BuffATKS(2f);

        Debug.Log("���� �ڱ���");
        gameObject.SetActive(false);
    }

    public void ArtificialEyes()
    {
        data.BuffATKR(1.2f);

        Debug.Log("�ΰ��� �̽�");
        gameObject.SetActive(false);
    }

    public void BloodFlower()
    {
        data.BuffATK(1.1f);

        Debug.Log("�ǹٶ��");
        gameObject.SetActive(false);
    }

    public void PainShadow()
    {
        data.BuffATKS(0.7f);

        Debug.Log("������ �׸���");
        gameObject.SetActive(false);
    }

    public void Weak()
    {
        data.BuffATK(0.7f);

        Debug.Log("���");
        gameObject.SetActive(false);
    }
}
