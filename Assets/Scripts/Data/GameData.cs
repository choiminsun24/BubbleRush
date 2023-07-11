using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����� ������. ���� �����ϸ� ������ ����, local�̸� �̱������� �ϸ� �Ƿ���
public class GameData : MonoBehaviour
{
    static public float[] atkUp = { 1f, 1.01f, 1.02f, 1.03f };
    static public float[] atkTime = { 1, 0.99f, 0.98f, 0.97f };
    static public float[] atkRange = { 1f, 1.01f, 1.02f, 1.03f };

    static public float GetKnowATK(int i)
    {
        return atkUp[i];
    }

    static public float GetKnowATKS(int i)
    {
        return atkTime[i];
    }

    static public float GetKnowATKR(int i)
    {
        return atkRange[i];
    }
}
