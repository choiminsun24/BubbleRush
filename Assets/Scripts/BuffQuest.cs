using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffQuest : MonoBehaviour
{
    public InGameData data;

    public void play()
    {

    }

    public void Medicine()
    {
        data.BuffATKS(1.1f);

        Debug.Log("���� �ڱ���");
    }

    public void ArtificialEyes()
    {
        data.BuffATKR(1.2f);

        Debug.Log("�ΰ��� �̽�");
    }

    public void BloodFlower()
    {
        data.BuffATK(1.1f);

        Debug.Log("�ǹٶ��");
    }

    public void PainShadow()
    {
        data.BuffATKS(0.7f);

        Debug.Log("������ �׸���");
    }

    public void Weak()
    {
        data.BuffATK(0.7f);

        Debug.Log("���");
    }
}
