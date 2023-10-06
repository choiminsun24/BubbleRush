using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerController : MonoBehaviour
{
    public int expression = 1; //임시 표정 MAD

    public bool isInstantiated {get; set;} = false;
    public bool isFusioning {get; set;} = false;
    public Animator anim { get; set; }

    private Tower data;
    BulletController bullCtr;

    public int level = 1;
    [SerializeField] private Sprite[] otherImgs;

    private float time = 0f;

    //public List<GameObject> enemies;
    
    public bool canTongue {get; set;} = false;
    public bool isAttacking {get; set;} = false;
    [SerializeField] private int towerCategory = 0;
    [SerializeField] private UnityEngine.Object bullet;

    private float angle;
    private GameObject bull;

    private SpriteRenderer spriteRenderer;

    private bool canAnimate = true;


    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        data = new Tower();
        data.hp = 10;
        data.attack = 20;
        data.range = 3;
        data.time = 1f;
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

        // 합성 중이 아닐 때, 게임 시작했을 때만 공격
        if(!isFusioning && GameManager.Instance.isStarted)
        {
            Attack();
        }
        else
        {
            isAttacking = false;
        }
    }

    private Transform nearestEnemy = null;
    [SerializeField] private float offset =2f;
    private Transform SelectEnemy()
    {
        if (!nearestEnemy || Vector3.Distance(nearestEnemy.position, transform.position) >= offset)
        {
            float min_distance = offset;
            foreach (Enemy enemy in GameManager.Instance.enemies)
            {
                if(Vector3.Distance(enemy.transform.position, transform.position) < offset)
                {
                    nearestEnemy = enemy.transform;
                }
            }
        }
        
        if(nearestEnemy && Vector3.Distance(nearestEnemy.position, transform.position) >= offset)
        {
            nearestEnemy = null;
        }

        return nearestEnemy;
    }

    public void DestroySelected(GameObject destroyedEnemy)
    {
        if(!nearestEnemy)
        {
            return;
        }
        if (nearestEnemy.gameObject == destroyedEnemy)
        {
            nearestEnemy = null;
        }
    }

    public void Attack()
    {
        if (!SelectEnemy())
        {
            if(towerCategory == 1 && canTongue)
            {
                canTongue = false;
            }
            else
            {
                anim.SetBool("isAttack", false);
                canAnimate = true;
            }
            return;
        }

        if (towerCategory == 1)
        {
            if(!nearestEnemy)
            {
                canTongue = false;
            }
            else
            {
                canTongue = true;
            }
        }

        isAttacking = true;
        // 감지된 적 있을 때 바라보기
        angle = Mathf.Atan2(nearestEnemy.transform.position.y - transform.position.y,
                            nearestEnemy.transform.position.x - transform.position.x)
              * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 270);

        if (time >= data.time)
        {
            time = 0f;

            
            // Bullet 생성하여 적을 향해 이동시키기
            bull = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
            bullCtr = bull.GetComponent<BulletController>();
            bullCtr.setBullet(data.attack, expression);
            bullCtr.TriggerMove(nearestEnemy.transform);
            if (towerCategory == 1)
            {
                return;
            }
            if (canAnimate)
            {
                // 일반 공격
                anim.SetBool("isUp", false);
                anim.SetBool("isAttack", true);
                canAnimate = false;
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
