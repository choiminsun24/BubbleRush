using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using PathCreation;

public class Bubble : MonoBehaviour
{
    float hp;
    float speed;

    public void takeDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            //GamaManager.AddHeart(-1); //싱글톤 구현 뒤 추가.
        }
    }

    private void Start()
    {
        GetComponent<PathFollower>().pathCreator = 
        GameObject.Find("Path").GetComponent<PathCreator>();
    }

    //맵 밖으로 벗어나면 GameManager AddHeart(-1) 호출.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Finish")
        {
            print("END POINT");
            GameManager.Instance.AddHeart(-1);
            Destroy(gameObject);
        }
    }

    //타워 공격에 맞으면 체력 감소

}
