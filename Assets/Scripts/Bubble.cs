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
    }

    //맵 밖으로 벗어나면 GameManager AddHeart(-1) 호출.

    //타워 공격에 맞으면 체력 감소

}
