using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerController : MonoBehaviour
{
    private Tower data;
    BulletController bullCtr;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        data = new Tower();
        data.hp = 10;
        data.attack = 5;
        data.range = 3;
        data.time = 1f;
    }
    public List<GameObject> enemies;
    [SerializeField] private Object bullet;
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

    
    private bool dragging = false;
    private Touch touch;
    private Vector3 initPos;
    private Vector2 vec, offset;
    private SpriteRenderer sprite;

    private int towerCategory = 1;
    private TowerController targetLevel;
    [SerializeField] private SpriteRenderer grayMap;
    [SerializeField] private FusionRange fusionRange;
    // 일정한 주기로 공격
    private float time = 0f;
    void Update()
    {

        time += Time.deltaTime;
        if (time >= data.time)
        {
            time = 0f;
            Attack();
        }
        /*
                // 다중 터치 시 스킵
                if (Input.touchCount != 1)
                {
                    dragging = false;
                    return;
                }

                // 입력된 터치 수
                touch = Input.touches[0];
                initPos = touch.position;

                if (touch.phase == TouchPhase.Began)
                {
                    sprite.color = new Color(1, 1, 1, 0.5f);
                    // 터치 좌표를 월드 좌표로 계산
                    vec = new Vector2(initPos.x, initPos.y);
                    vec = Camera.main.ScreenToWorldPoint(vec);
                    // 타워와 터치 위치 차이
                    offset = (Vector2)transform.position - vec;

                    fusionRange.gameObject.SetActive(true);
                    if (!grayMap)
                    {
                        grayMap = GameManager.Instance.grayMap;
                    }
                    grayMap.gameObject.SetActive(true);
                    //grayMap.sortingLayerID = towerCategory - 1;
                    dragging = true;
                }

                if (dragging && touch.phase == TouchPhase.Moved)
                {
                    // dragging
                    vec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    vec = Camera.main.ScreenToWorldPoint(vec);
                    transform.position = vec + offset;
                }

                if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                {
                    sprite.color = new Color(1, 1, 1, 1f);
                    if (dragging == true)
                    {
                        if (fusionRange.canFuse == true)
                        {
                            targetLevel = fusionRange.targetTower.GetComponent<TowerController>();
                            if (targetLevel)
                            {
                                targetLevel.LevelUp();
                            }
                            else
                            {
                                return;
                            }
                            Destroy(this.gameObject, 0.5f);
                        }
                    }
                    dragging = false;
                }
                */

    }

    private float angle;
    private GameObject bull;
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
/*
    [SerializeField] private SpriteRenderer grayMap;
    private int towerCategory=1;
    public void OnBeginDrag(PointerEventData eventData)
    {
        fusionRange.gameObject.SetActive(true);
        if(!grayMap)
        {
            grayMap = transform.Find("GrayMap").GetComponent<SpriteRenderer>();
        }
        grayMap.gameObject.SetActive(true);
        grayMap.sortingLayerID = towerCategory - 1;
    }

    private Vector3 mousePosition;
    [SerializeField] private FusionRange fusionRange;

    public void OnDrag(PointerEventData eventData)
    {
        // dragging
        transform.position = Input.mousePosition;
    }

    private TowerController targetLevel;
    public void OnEndDrag(PointerEventData eventData)
    {
        if(fusionRange.canFuse == true)
        {
            targetLevel = fusionRange.targetTower.GetComponent<TowerController>();
            if (targetLevel)
            {
                targetLevel.LevelUp();
            }
            else
            {
                return;
            }
            Destroy(this.gameObject, 0.5f);
        }
    }*/
    private int level = 0;
    [SerializeField] private Sprite[] otherImgs;
    public void LevelUp()
    {
        level+=1;
        sprite.sprite = otherImgs[level];
    }
}
