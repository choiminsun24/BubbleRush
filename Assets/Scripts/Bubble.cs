using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    float hp;
    float speed;

    public void takeDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            //GamaManager.AddHeart(-1); //�̱��� ���� �� �߰�.
        }
    }

    
    //�� ������ ����� GameManager AddHeart(-1) ȣ��.

    //Ÿ�� ���ݿ� ������ ü�� ����

}
