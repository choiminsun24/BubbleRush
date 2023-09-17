using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    private static TowerManager _instance;

    // 인스턴스에 접근하기 위한 프로퍼티
    public static TowerManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(TowerManager)) as TowerManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    public List<GameObject>[] towers = new List<GameObject>[6];
    private List<GameObject> tower0 = new List<GameObject>(), tower1 = new List<GameObject>(),
                            tower2 = new List<GameObject>(), tower3 = new List<GameObject>(), 
                            tower4 = new List<GameObject>(), tower5 = new List<GameObject>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // 인스턴스가 존재하는 경우 기존 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(_instance.gameObject);
        }
        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
        towers[0] = tower0; towers[1] = tower1;
        towers[2] = tower2; towers[3] = tower3;
        towers[4] = tower4; towers[5] = tower5;
    }


    private bool dragging = false;
    private Touch touch;
    private Vector3 initPos;
    private Vector2 vec;
    private SpriteRenderer sprite;

    private int towerCategory = 0;
    private TowerController towerController;
    private GameObject touchedObject;
    [SerializeField] private GameObject grayMap;
    private FusionRange fusionRange;
    private TowerController targetTc;
    private float offset = 0.5f;

    // Update is called once per frame
    void Update()
    {
        // 다중 터치 시 스킵
        if (Input.touchCount != 1)
        {
            dragging = false;
            return;
        }

        // 입력된 터치 수
        touch = Input.touches[0];

        //1번만 실행되어야함
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPos, Camera.main.transform.forward);


            if (hitInformation.collider != null)
            {
                touchedObject = hitInformation.transform.gameObject;
                if (touchedObject.tag =="Tower")
                {
                    // 기존 위치 저장
                    initPos = touchedObject.transform.position;
                    // 타워 종류 저장
                    towerCategory = int.Parse(touchedObject.name.Replace("Tower", ""));

                    towerController = touchedObject.GetComponent<TowerController>();
                    if (!towerController && !(towerController.isInstantiated))
                    {
                        return;
                    }


                    sprite = touchedObject.GetComponent<SpriteRenderer>();
                    sprite.color = new Color(1, 1, 1, 0.5f);
                    // fusionRange = touchedObject.GetComponent<FusionRange>();
                    // fusionRange.gameObject.SetActive(true);

                    grayMap.SetActive(true);
                    // 같은 종류, 레벨의 타워만 띄우기
                    for(int i = 0; i<towers.Length; i++)
                    {
                        if(i != towerCategory)
                        {
                            foreach (var tower in towers[i])
                            {
                                tower.SetActive(false);
                            }
                        }
                        else
                        {
                            foreach (var tower in towers[i])
                            {
                                if(towerController.level != tower.GetComponent<TowerController>().level)
                                {
                                    tower.SetActive(false);
                                }
                            }
                        }
                    }
                    dragging = true;
                }
            }
        }

        if (!towerController || !(towerController.isInstantiated))
        {
            dragging = false;
            return;
        }
        

        if (dragging && touch.phase == TouchPhase.Moved)
        {
            towerController.canTongue = false;
            // if (!towerController.isFusioning)
            // {
            //     towerController.isFusioning = true;
                
            // }
            // 터치 좌표를 월드 좌표로 계산
            vec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            vec = Camera.main.ScreenToWorldPoint(vec);
            touchedObject.transform.position = vec;

            // if (fusionRange.canFuse == true)
            // {
            //     sprite.color = new Color(0.2f, 1f, 0.2f, 0.8f);
            // }
            // else
            // {
            //     sprite.color = new Color(1f, 1f, 1f, 1f);
            // }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //towerController.isFusioning = false;
            towerController.canTongue = true;
            sprite.color = new Color(1, 1, 1, 1f);

            if (dragging == true)
            {
                // if (fusionRange.canFuse == true)
                // {
                //     towerController = fusionRange.targetTower.GetComponent<TowerController>();
                //     if (towerController)
                //     {
                //         towerController.LevelUp();
                //         // find 때문에 Stack -> List로 변경
                //         towers[towerCategory].Remove(towers[towerCategory].Find(x => x == touchedObject));
                //         Destroy(touchedObject, 0.1f);
                //     }
                //     else
                //     {
                //         return;
                //     }
                // }
                float min_distance = offset;
                foreach (GameObject target in towers[towerCategory])
                {
                    if(target == touchedObject)
                    {
                        continue;
                    }

                    float distance = Vector3.Distance(target.transform.position, touchedObject.transform.position);
                    if (distance <= offset && distance <= min_distance && target.activeSelf)
                    {
                        targetTc = target.GetComponent<TowerController>();
                        min_distance = distance;
                    }
                }

                if (targetTc)
                {
                    targetTc.LevelUp();
                    // find 때문에 Stack -> List로 변경
                    towers[towerCategory].Remove(towers[towerCategory].Find(x => x == touchedObject));
                    Destroy(touchedObject, 0.1f);
                }
                else
                {
                    touchedObject.transform.position = initPos;
                }
            }
            
            // 나머지 타워 다시 복귀
            for (int i = 0; i < towers.Length; i++)
            {
                foreach (var tower in towers[i])
                {
                    tower.SetActive(true);
                }
            }
            towerController = null;
            targetTc = null;
            grayMap.SetActive(false);
            dragging = false;

        }
    }
}
