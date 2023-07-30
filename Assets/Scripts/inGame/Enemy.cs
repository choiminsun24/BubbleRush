using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using PathCreation;
using TMPro;

public class Enemy : MonoBehaviour
{
    public TextMesh hpText;

    private int hp;
    private float speed;

    private void Start()
    {
        updateTextHP();
        GetComponent<PathFollower>().pathCreator = GameObject.Find("Path").GetComponent<PathCreator>();
    }

    public void setEnemy(int hp, float speed)
    {
        this.hp = hp;
        this.speed = speed;
    }

    //공격에 맞은 경우
    public void takeDamage(float ATK)
    {
        if (hp <= 0)
            return;

        int damage = (int)ATK;  //타워의 공격력을 넘겨주면 데미지를 연산.

        hp -= damage;

        if (hp <= 0)
        {
            Death(5);
        }

        updateTextHP();
    }

    public void updateTextHP()
    {
        hpText.text = hp.ToString();
        //Debug.Log(hp);
    }

    private void Death(int _coin) 
    {
        if(_coin != 0) //타워한테 죽은 경우만 시행
        {
            GameManager.Instance.Coin(_coin); //코인 획득
            GameManager.Instance.bubblePop.Play(); //효과음 재생
        }
        GameManager.Instance.RemoveEnemy(this); //배열에서 제거
        Destroy(gameObject); //오브젝트 제거
    }

    //맵 밖으로 벗어나면 GameManager AddHeart(-1) 호출.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Finish")
        {
            print("END POINT");
            GameManager.Instance.AddHeart(-1);
            // 코인 증가 없이 소멸
            Death(0);
        }
    }
}
