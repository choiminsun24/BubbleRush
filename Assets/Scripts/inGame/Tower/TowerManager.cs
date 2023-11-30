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

    public bool canFuse {get; set;} = true;
    public int chances = -1;
    public Daebak daebakInfo;
    public Nabi nabiInfo;
    public Tori toriInfo;
    public Goby gobyInfo;
    public Tutu tutuInfo;
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

        ITower.InitializeDB();
        daebakInfo = new Daebak();
        daebakInfo.Clone();
        nabiInfo = new Nabi();
        nabiInfo.Clone();
        toriInfo = new Tori();
        toriInfo.Clone();
        gobyInfo = new Goby();
        gobyInfo.Clone();
        tutuInfo = new Tutu();
        tutuInfo.Clone();
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
    private float offset = 1f;

    float min_distance=1f;



    // Update is called once per frame
    void Update()
    {
        if (chances == 0)
        {
            return;
        }
        // 다중 터치 시 스킵
        if (Input.touchCount != 1 || !canFuse)
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
                    towerController = touchedObject.GetComponent<TowerController>();
                    towerCategory = towerController.towerCategory;

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
                    for(int i = 0; i<6; i++)
                    {
                        if(i != towerCategory)
                        {
                            foreach (var tower in GetTowerList(i))
                            {
                                tower.SetActive(false);
                            }
                        }
                        else
                        {
                            foreach (var tower in GetTowerList(i))
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
            targetTc = null;
        }

        if (!towerController || !(towerController.isInstantiated))
        {
            dragging = false;
            return;
        }
        

        if (dragging && touch.phase == TouchPhase.Moved)
        {
            towerController.canTongue = false;
            
            // 터치 좌표를 월드 좌표로 계산
            vec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            vec = Camera.main.ScreenToWorldPoint(vec);
            touchedObject.transform.position = vec;

            min_distance = offset;
            foreach (GameObject target in GetTowerList(towerCategory))
            {
                if (target == touchedObject || !target.activeSelf)
                {
                    continue;
                }

                float distance = Vector3.Distance(target.transform.position, touchedObject.transform.position);
                if (distance <= offset && distance <= min_distance && target.activeSelf)
                {
                    if(targetTc)
                    {
                        targetTc.anim.SetBool("isUp", false);
                    }
                    targetTc = target.GetComponent<TowerController>();
                    min_distance = distance;
                }
            }

            if (targetTc)
            {
                targetTc.anim.SetBool("isUp", true);
            }
            if(targetTc && Vector3.Distance(targetTc.transform.position, touchedObject.transform.position) > offset)
            {
                targetTc.anim.SetBool("isUp", false);
                targetTc = null;
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            //towerController.isFusioning = false;
            towerController.canTongue = true;
            sprite.color = new Color(1, 1, 1, 1f);

            if (dragging == true)
            {
                
                float min_distance = offset;
                foreach (GameObject target in GetTowerList(towerCategory))
                {
                    if(target == touchedObject || !target.activeSelf)
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
                    --chances;
                    targetTc.anim.SetBool("isUp", false);
                    targetTc.LevelUp();
                    ReturnTower(towerCategory, touchedObject);
                    Destroy(touchedObject, 0.1f);
                }
                else
                {
                    touchedObject.transform.position = initPos;
                }
            }
            
            // 나머지 타워 다시 복귀
            for (int i = 0; i < 6; i++)
            {
                foreach (var tower in GetTowerList(i))
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

    public List<GameObject> GetTowerList(int category)
    {
        switch(category)
        {
            case 0:
            return Daebak.listDaebak;
            case 1:
            return Nabi.listNabi;
            case 2:
            return Tori.listTori;
            case 3:
            return Goby.listGoby;
            case 4:
            return Tutu.listTutu;
            case 5:
            return Tower5.listTower5;
            default:
            return null;
        }
    }

    public void ReturnTower(int category, GameObject obj)
    {
        switch(category)
        {
            case 0:
            daebakInfo.ReturnDaebak(obj);
            break;
            case 1:
            nabiInfo.ReturnNabi(obj);
            break;
            case 2:
            toriInfo.ReturnTori(obj);
            break;
            case 3:
            gobyInfo.ReturnGoby(obj);
            break;
            case 4:
            tutuInfo.ReturnTutu(obj);
            break;
            // case 5:
            // nabiInfo.ReturnNabi(obj);
            // break;
            default:
            break;
        }
    }
}
