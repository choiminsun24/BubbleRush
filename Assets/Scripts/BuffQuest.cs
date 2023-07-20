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

        Debug.Log("전투 자극제");
    }

    public void ArtificialEyes()
    {
        data.BuffATKR(1.2f);

        Debug.Log("인공눈 이식");
    }

    public void BloodFlower()
    {
        data.BuffATK(1.1f);

        Debug.Log("피바라기");
    }

    public void PainShadow()
    {
        data.BuffATKS(0.7f);

        Debug.Log("고통의 그림자");
    }

    public void Weak()
    {
        data.BuffATK(0.7f);

        Debug.Log("쇠약");
    }
}
