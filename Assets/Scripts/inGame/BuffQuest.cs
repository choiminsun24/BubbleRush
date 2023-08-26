using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffQuest : MonoBehaviour
{
    //������
    public InGameData data;

    //������
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
        instance = this; //�������� ������ �ϳ��� ���.
    }

    public void Start()
    {
        Box.SetActive(false);
    }

    public void play()
    {
        Box.SetActive(true);

        //1. �������� �� �̰�
            int[] num = new int[] {-1, -1, -1};

            for (int i = 0; i < 3; i++)
            {
                int r = Random.Range(0, button.Count);
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

        //2. Canvas�� ��ġ
        for (int i = 0; i < 3; i++)
        {
            GameObject game = Instantiate(button[num[i]], position[i].position, Quaternion.identity, canvas);
        }

        data.check(); //���ο� Log ��� �޼ҵ�
    }

    private void choice()
    {
        Box.SetActive(false);
    }

    private bool doMedicine = false;
    public void Medicine()
    {
        if (doMedicine == false) //����
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
        if (doEyse == false) // ���õ�.
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
        if (doBlood == false) // ���õ�.
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
        if (doPain == false) // ���õ�.
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
        if (doWeak == false) // ���õ�.
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
