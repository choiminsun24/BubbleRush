using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using PathCreation;

public class Enemy : MonoBehaviour
{
    public Transform hpT;
    private float barWidth;

    private int hp;
    private float speed;

    private void Start()
    {
        GetComponent<PathFollower>().pathCreator = GameObject.Find("Path").GetComponent<PathCreator>();
    }

    public void setEnemy(int hp, float speed)
    {
        this.hp = hp;
        this.speed = speed;

        barWidth = hpT.localScale.x / this.hp;
    }

    public void takeDamage(float ATK)
    {
        int damage = (int)ATK;  //타워의 공격력을 넘겨주면 데미지를 연산.

        hp -= damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }

        updateTextHP();
    }

    public void updateTextHP()
    {
        hpT.localScale = new Vector3(hp * barWidth, hpT.localScale.y, 0f);
        Debug.Log(hp);
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
