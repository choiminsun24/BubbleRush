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
    public bool isFused {get; set;} = false;
    public Animator anim { get; set; }

    public Tower data {get;set;}
    BulletController bullCtr;

    public int level = 1;
    [SerializeField] private Sprite[] otherImgs;

    private float time = 0f;

    //public List<GameObject> enemies;
    
    public bool canTongue {get; set;} = false;
    public bool isAttacking {get; set;} = false;
    public int towerCategory = -1;
    [SerializeField] private UnityEngine.Object bullet;
    [SerializeField] private Collider2D attackCollider;

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
        Debug.Log("Tower Category: " + towerCategory + " attack: " + data.attack);
    }
    
    // 일정한 주기로 공격
    void Update()
    {
        if(!isInstantiated)
        {
            spriteRenderer.color = new Color(1,1,1,0.5f);
            return;
        }
        else
        {
            spriteRenderer.color = new Color(1,1,1,1f);
        }
        time += Time.deltaTime;

        // 합성 중이 아닐 때, 게임 시작했을 때만 공격
        if (!isFusioning && GameManager.Instance.isStarted)
        {
            isAttacking = false;
            Attack();
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }

    public Transform nearestEnemy = null;
    [SerializeField] private float offset = 2f;
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
            if (towerCategory == 0 && canTongue)
            {
                canTongue = false;
            }
            else if(towerCategory != 0)
            {
                anim.SetBool("isAttack", false);
                aura?.GetComponent<Animator>().SetBool("isAttack", false);
                canAnimate = true;
            }
            return;
        }

        // 감지된 적 있을 때, 공격 전에만 바라보기
        if (isAttacking == false)
        {
            angle = Mathf.Atan2(nearestEnemy.transform.position.y - transform.position.y,
                                nearestEnemy.transform.position.x - transform.position.x)
                    * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 270);
        }

        if (towerCategory == 0)
        {
            if(!nearestEnemy)
            {
                canTongue = false;
            }
            else
            {
                canTongue = true;
            }
            return;
        }


        if (time >= data.skillCoolTime/4000)
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
                
                aura?.GetComponent<Animator>().SetBool("isAttack", true);
                
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

    public void TurnOnWeaponColl()
    {
        Debug.Log("TurnOnWeaponCollider");
        attackCollider.enabled = true;
        Invoke("TurnOffWeaponCollider", 0.3f);
    }
    private void TurnOffWeaponCollider()
    {
        attackCollider.enabled = true;
    }
}
