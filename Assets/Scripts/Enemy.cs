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
        int damage = (int)ATK;  //타워의 공격력을 넘겨주면 데미지를 연산.

        hp -= damage;

        if (hp <= 0)
        {
            Death();
        }

        updateTextHP();
    }

    public void updateTextHP()
    {
        hpText.text = hp.ToString();
        Debug.Log(hp);
    }

    private void Death()
    {
        GameManager.Instance.AddStageCoin(5);
        GameManager.Instance.bubblePop.Play();
        Destroy(gameObject);
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
}
