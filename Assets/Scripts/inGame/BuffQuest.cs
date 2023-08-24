using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffQuest : MonoBehaviour
{
    //데이터
    public InGameData data;

    //선택지
    public GameObject Box;
    public List<GameObject> button;
    public Transform[] position;
    public Transform canvas;

    private static BuffQuest instance;

    public static BuffQuest Instance
    {
        get
        {
            if (!instance)
            {
                return null;
            }
            return instance;
        }
    }

    void Awkae()
    {
        instance = this; //마지막에 생성된 하나만 사용.
    }

    public void Start()
    {
        Box.SetActive(false);
    }

    public void play()
    {
        Box.SetActive(true);

        //1. 랜덤으로 셋 뽑고
            int[] num = new int[] {-1, -1, -1};

            for (int i = 0; i < 3; i++)
            {
                int r = Random.Range(0, button.Count);
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

        //2. Canvas에 설치
        for (int i = 0; i < 3; i++)
        {
            GameObject game = Instantiate(button[num[i]], position[i].position, Quaternion.identity, canvas);
        }

        data.check(); //학인용 Log 출력 메소드
    }

    private void choice()
    {
        Box.SetActive(false);
    }

    private bool doMedicine = false;
    public void Medicine()
    {
        if (doMedicine == false) //선택
        {
            doMedicine = true;
            choice();
        }
        else
        {
            button.Remove(button[2]);
            data.BuffATKS(2f);
        }
    }

    private bool doEyse = false;
    public void ArtificialEyes()
    {
        if (doEyse == false) // 선택됨.
        {
            doEyse = true;
            choice();
        }
        else
        {
            data.BuffATKR(1.2f);
        }
    }

    private bool doBlood = false;
    public void BloodFlower()
    {
        if (doBlood == false) // 선택됨.
        {
            doBlood = true;
            choice();
        }
        else
        {
            data.BuffATK(1.1f);
        }
    }

    private bool doPain = false;
    public void PainShadow()
    {
        if (doPain == false) // 선택됨.
        {
            doPain = true;
            choice();
        }
        else
        {
            data.BuffATKS(0.7f);
        }
    }

    private bool doWeak = false;
    public void Weak()
    {
        if (doWeak == false) // 선택됨.
        {
            doWeak = true;
            choice();
        }
        else
        {
            data.BuffATK(0.7f);
        }
    }
}
