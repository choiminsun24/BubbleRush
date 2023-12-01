using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using PathCreation;
using TMPro;

public class Enemy : MonoBehaviour
{
    //UI
    public TextMesh hpText;
    public GameObject effect;

    //객체값 - 체력, 속력
    private int hp;
    private int speed;

    //버블 종류 선택 - 속성, 표정
    [System.Serializable]
    public enum Expression
    {
        MAD = 0,
        SMILE = 1,
        EXPRESSIONLESS = 2
    };

    public enum Property
    {
        GROUND = 0,
        WATER = 1,
        WIND = 2
    };

    [SerializeField]
    public Expression expression;
    public Property property;


    private void Start()
    {
        effect.SetActive(false);
        updateTextHP();
        GetComponent<PathFollower>().pathCreator = GameObject.Find("Path").GetComponent<PathCreator>();
    }

    public void setEnemy(int hp, int speed)
    {
        this.hp = hp;
        this.speed = speed;
    }

    //공격에 맞은 경우
    public void takeDamage(float ATK, int towerEx, GameObject bullet)
    {
        if(bullet!=null)
        {
            Destroy(bullet);
        }
        if (hp <= 0)
            return;

        //타워의 공격력으로 데미지 연산
        float damage = ATK;
        Expression tower = (Expression)towerEx;
        Debug.Log("타워 속성: " + tower);

        if (expression == Expression.MAD)
        {
            if (tower == Expression.SMILE) //웃는 친구 -> 화난 친구 : 약함
                damage *= 0.8f;
            else if (tower == Expression.EXPRESSIONLESS) //무표정 친구 -> 화난 친구 : 강함
                damage *= 1.2f;
        }
        else if (expression == Expression.SMILE)
        {
            if (tower == Expression.EXPRESSIONLESS) //무표정 친구 -> 웃는 친구 : 약함
                damage *= 0.8f;
            else if (tower == Expression.MAD) //화난 친구 -> 웃는 친구 : 강함
                damage *= 1.2f;
        }
        else //expression == Expression.Expressionless
        {
            if (tower == Expression.MAD) //화난 친구 -> 무표정 친구 : 약함
                damage *= 0.8f;
            else if (tower == Expression.MAD) //웃는 친구 -> 무표정 친구 : 강함
                damage *= 1.2f;
        }

        hp -= Mathf.RoundToInt(damage);

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
        if (_coin != 0) //타워한테 죽은 경우만 시행
        {
            GameManager.Instance.Coin(_coin); //코인 획득
            SoundManager.Instance.EffectPlay(SoundManager.Instance.selectKillSound()); //효과음 재생
            effect.SetActive(true); //이펙트 재생
            hpText.gameObject.SetActive(false); //체력바 제거
            gameObject.GetComponent<SpriteRenderer>().enabled = false; //안 보이게 처리
        }
        GameManager.Instance.RemoveEnemy(this); //배열에서 제거
        for (int i=0; i<6; ++i)
        {
            foreach (var tower in TowerManager.Instance.GetTowerList(i))
            {
                tower.GetComponent<TowerController>().DestroySelected(gameObject);
            }
        }
        
        Invoke("destroy", 0.2f); //오브젝트 제거
    }

    private void destroy()
    {
        Destroy(gameObject);
    }

    private bool live = true;

    //맵 밖으로 벗어나면 GameManager AddHeart(-1) 호출.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Finish")
        {
            if (live)
            {
                print("END POINT");
                GameManager.Instance.AddHeart(-1);
                // 코인 증가 없이 소멸
                live = false;
                Death(0);
            }
        }
    }
}
