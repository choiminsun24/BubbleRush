using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerController : MonoBehaviour
{
    public int expression = 1; //임시 표정 MAD

    public bool isInstantiated {get; set;} = false;
    private Tower data;
    BulletController bullCtr;

    public int level = 1;
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
        spriteRenderer = GetComponent<SpriteRenderer>();

        data = new Tower();
        data.hp = 10;
        data.attack = 10;
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
        if(!isInstantiated)
        {
            spriteRenderer.enabled = false;
            return;
        }
        else
        {
            spriteRenderer.enabled = true;
        }
        time += Time.deltaTime;
        Attack();
    }

    public void Attack()
    {
        if(enemies.Count==0)
        {
            return;
        }

        if(time >= data.time)
        {
            // 감지된 적 있을 때 바라보기
            angle = Mathf.Atan2(enemies[0].transform.position.y - transform.position.y,
                                enemies[0].transform.position.x - transform.position.x)
                  * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 270);
            time = 0f;

            // Bullet 생성하여 적을 향해 이동시키기
            foreach (GameObject enemy in enemies)
            {
                bull = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
                bullCtr = bull.GetComponent<BulletController>();
                bullCtr.setBullet(data.attack, expression);
                bullCtr.TriggerMove(enemy.transform);
            }
        }

        
    }

    [SerializeField]private GameObject effect;
    [SerializeField] private SpriteRenderer aura;
    private Color[] auraColor = {Color.white, Color.cyan, Color.gray, Color.magenta, Color.green, Color.blue};
    public void LevelUp()
    {
        Debug.Log("Level UP");
        if(aura)
        {
            aura.enabled = true;
            aura.color = auraColor[level - 1];
        }
        if(effect)
        {
            effect.SetActive(true);
            Invoke("TurnOffEffect", 1.0f);
        }
        spriteRenderer.sprite = otherImgs[level++];
        if (level >= otherImgs.Length)
        {
            level = 0;
        }
        
        // 공격력 +5
        data.attack += 5;
    }
    private void TurnOffEffect()
    {
        effect.SetActive(false);
    }
}
