using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using PathCreation;
using TMPro;

public class Enemy : MonoBehaviour
{
    //퀘스트 
    static int killMad = 0;
    static int killSmile = 0;
    static int killExpressionless = 0;

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
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private GameObject[] effectNabi;
    private int rand;

    private void Start()
    {
        prevVel = transform.position;
        anim = GetComponent<Animator>();
        effect.SetActive(false);
        updateTextHP();
        GetComponent<PathFollower>().pathCreator = GameObject.Find("Path").GetComponent<PathCreator>();

        rand = Random.Range(0, effectNabi.Length);
    }

    public void setEnemy(int hp, int speed)
    {
        this.hp = hp;
        this.speed = speed;
    }

    //공격에 맞은 경우
    public void takeDamage(float ATK, int towerEx, GameObject bullet, int towerCategory)
    {
        if(bullet!=null)
        {
            Destroy(bullet);
        }
        if (hp <= 0)
            return;
        if (towerCategory == 0)
        {
            damageEffect.SetActive(false);
            damageEffect.SetActive(true);
            SoundManager.Instance.EffectPlay(SoundManager.Instance.Daebak_Attack[UnityEngine.Random.Range(0, SoundManager.Instance.Daebak_Attack.Length)]);
        }
        else if (towerCategory == 1)
        {
            effectNabi[rand].SetActive(false);
            effectNabi[rand].SetActive(true);

            SoundManager.Instance.EffectPlay(SoundManager.Instance.Nabi_Attack);
        }
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
        else if (expression == Expression.EXPRESSIONLESS)
        {
            if (tower == Expression.MAD) //화난 친구 -> 무표정 친구 : 약함
                damage *= 0.8f;
            else if (tower == Expression.MAD) //웃는 친구 -> 무표정 친구 : 강함
                damage *= 1.2f;

            Debug.Log(damage);
            damage *= InGameData.Instance.getBuffExpressionlessEnemyDamaged();
            Debug.Log(damage);
        }
        else
            Debug.Log("그런 타입 없습니다.");

        hp -= Mathf.RoundToInt(damage);

        if (hp <= 0)
        {

            Death(10);
        }

        updateTextHP();
    }

    public void updateTextHP()
    {
        hpText.text = hp.ToString();
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

            //퀘스트 카운트
            if (expression == Expression.MAD)
                killMad++;
            else if (expression == Expression.SMILE)
                Quest.Instance.checkSmile(++killSmile);
            else if (expression == Expression.EXPRESSIONLESS)
                Quest.Instance.checkExpressionless(++killExpressionless);
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

    private Animator anim;
    private Vector2 prevVel;
    private void Update()
    {
        // x 변화량이 더 크면 가로, y 변화량이 더 크면 세로
        if(Mathf.Abs(transform.position.x - prevVel.x) > Mathf.Abs(transform.position.y - prevVel.y))
        {
            anim.SetBool("isHeight", false);
            prevVel = transform.position;
        }
        else
        {
            anim.SetBool("isHeight", true);
            prevVel = transform.position;
        }
    }
}
