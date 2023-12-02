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


    //public List<GameObject> enemies;
    
    public bool canTongue {get; set;} = false;
    public bool isAttacking {get; set;} = false;
    public int towerCategory = -1;
    [SerializeField] private UnityEngine.Object bullet;
    [SerializeField] private GameObject nabiEffect;

    private float angle;
    private GameObject bull;

    private SpriteRenderer spriteRenderer;

    public bool canAnimate = true;
    public bool canCreateBullet = true;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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

        // 합성 중이 아닐 때, 게임 시작했을 때만 공격
        if (!isFusioning && towerCategory != 4)
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
    [SerializeField] private float offset = 0.5f;
    private Transform SelectEnemy()
    {
        Debug.Log(offset);
        Transform prevEnemy = nearestEnemy;
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

    public float attackTime = 0f;
    public void Attack()
    {

        attackTime += Time.deltaTime;

        // 공격 속도 지났을 때, 다음 공격 위해 초기화
        if (attackTime >= 1 / (data.attackSpeed * 0.001f))
        {
            
            attackTime = 0f;

            anim.SetBool("isAttack", false);
            aura?.GetComponent<Animator>().SetBool("isAttack", false);
            isAttacking = false;

            canAnimate = true;
            canCreateBullet = true;

            return;
            
        }

        // 게임 시작 여부
        if(!GameManager.Instance.isStarted)
        {
            return;
        }

        // 사거리 안에 적이 없을 때, 애니메이션 off
        if (!SelectEnemy())
        {
            {
                //attackTime = 0f;

                // canAnimate = true;
                // canCreateBullet = true;
                // isAttacking = false;
            }
            return;
        }

        // 사거리 안에 적 있을 때, 공격 시작 세팅
        if (canCreateBullet)
        {
            attackTime = 0f;
            Invoke("Shoot", 1 / (data.attackSpeed * 0.001f) * 0.2f);
            canCreateBullet = false;            
        }

        // 사거리 안에 적 있을 때, 공격 중일 때만 바라보기
        if (isAttacking == false)
        {
            
        }
        isAttacking = true;


        if (canAnimate)
        {
            // 공격 전에 바라보기
            angle = Mathf.Atan2(nearestEnemy.transform.position.y - transform.position.y,
                                nearestEnemy.transform.position.x - transform.position.x)
                    * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 270);

            isAttacking = true;
            if (towerCategory == 0)
            {
                SoundManager.Instance.EffectPlay(SoundManager.Instance.Daebak_Swish[UnityEngine.Random.Range(0, SoundManager.Instance.Daebak_Swish.Length)]);
            }
            // 일반 공격
            anim.speed = data.attackSpeed * 0.001f;
            anim.SetBool("isUp", false);
            anim.SetBool("isAttack", true);

            aura?.GetComponent<Animator>().SetBool("isAttack", true);

            canAnimate = false;
        }
        canAnimate = false;

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
        
        // 공격력 +=
        data.attack += data.attack;
        Debug.Log(data.attack);
    }
    private void TurnOffEffect()
    {
        effect.SetActive(false);
    }

    private void Shoot()
    {
        // 나비 이펙트
        if(towerCategory == 1)
        {
            nabiEffect?.SetActive(false);
            nabiEffect?.SetActive(true);
        }
        // Bullet 생성하여 적을 향해 이동시키기
        bull = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        bullCtr = bull.GetComponent<BulletController>();
        bullCtr.towerCategory = towerCategory;
        bullCtr.setBullet(data.attack, expression);
        bullCtr.TriggerMove(nearestEnemy?.transform);
    }

    // public void TurnOnCanBullet()
    // {
    //     canCreateBullet = true;
    // }
    // public void TurnOnWeaponColl()
    // {
    //     Debug.Log("TurnOnWeaponCollider");
    //     attackCollider.enabled = true;
    //     Invoke("TurnOffWeaponCollider", 0.3f);
    // }
    // private void TurnOffWeaponCollider()
    // {
    //     attackCollider.enabled = true;
    // }
}
