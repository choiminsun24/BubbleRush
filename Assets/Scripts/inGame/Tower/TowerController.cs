using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerController : MonoBehaviour
{
    public bool isInstantiated {get; set;} = false;
    private Tower data;
    BulletController bullCtr;

    private int level = 1;
    [SerializeField] private Sprite[] otherImgs;

    private float time = 0f;

    public List<GameObject> enemies;
    [SerializeField] private Object bullet;

    private float angle;
    private GameObject bull;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //sprite = GetComponent<SpriteRenderer>();

        data = new Tower();
        data.hp = 10;
        data.attack = 5;
        data.range = 3;
        data.time = 1f;
    }

    // 타워 근처 적 감지 범위
    public void DetectEnemies(GameObject enemy)
    {
        if (enemies.IndexOf(enemy) == -1)
        {
            enemies.Add(enemy);
        }
    }

    // 적이 감지 범위에서 벗어났을 때
    public void RemoveEnemies(GameObject enemy)
    {
        if (enemies.IndexOf(enemy) != -1)
        {
            GameObject temp = enemies.Find(element => element == enemy);
            enemies.Remove(temp);
        }
    }

    
    // 일정한 주기로 공격
    void Update()
    {

        time += Time.deltaTime;
        if (time >= data.time)
        {
            time = 0f;
            Attack();
        }

        
    }


    public void Attack()
    {
        if(enemies.Count==0)
        {
            return;
        }

        // 감지된 적 있을 때 바라보며 공격하기
        angle = Mathf.Atan2(enemies[0].transform.position.y - transform.position.y,
                            enemies[0].transform.position.x - transform.position.x)
              * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 270);

        // Bullet 생성하여 적을 향해 이동시키기
        foreach (GameObject enemy in enemies)
        {
            bull = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
            bullCtr = bull.GetComponent<BulletController>();
            bullCtr.setATK(data.attack);
            bullCtr.TriggerMove(enemy.transform);
        }
    }

    public void LevelUp()
    {
        Debug.Log("Level UP");
        spriteRenderer.sprite = otherImgs[level++];
        if(level >= otherImgs.Length)
        {
            level = 0;
        }
    }
}
