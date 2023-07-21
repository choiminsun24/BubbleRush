using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2D Object Drag & Drop 참고 링크
// https://tkablog.tistory.com/2

public class DragTower : MonoBehaviour
{
    [SerializeField] private Object newTower;
    [SerializeField] private Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        initLoc = transform.position;
    }
    [HideInInspector]
    public Transform nextLocation = null;
    private bool dragging = false;
    private Touch touch;
    private Vector2 pos, vec, offset, initLoc;
    private Transform toDrag;
    private RaycastHit hit;
    private GameObject hitObj;
    private SpriteRenderer hitObjSpr;
    // Update is called once per frame
    void Update()
    {
        // 다중 터치 시 스킵
        if(Input.touchCount != 1)
        {
            dragging = false;
            return;
        }

        // 입력된 터치 수 및 위치
        touch = Input.touches[0];
        pos = touch.position;

        if(touch.phase == TouchPhase.Began)
        {
            hitObj = gameObject;
            toDrag = hitObj.transform;
            hitObjSpr = hitObj.GetComponent<SpriteRenderer>();
            hitObjSpr.color = new Color(1,1,1,0.5f);
            // 터치 좌표를 월드 좌표로 계산
            vec = new Vector2(pos.x, pos.y);
            vec = Camera.main.ScreenToWorldPoint(vec);
            // 타워와 터치 위치 차이
            offset = (Vector2) toDrag.position - vec;
            dragging = true;
        }

        if(dragging && touch.phase == TouchPhase.Moved)
        {
            hitObjSpr.color = new Color(1,1,1,0.5f);
            vec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            vec = Camera.main.ScreenToWorldPoint(vec);
            toDrag.position = vec + offset;
        }

        if(dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            hitObjSpr.color = new Color(1,1,1,1f);
            if(dragging == true)
            {
                if(nextLocation == null)
                {
                    print("원위치로 돌아가기");
                    toDrag.position = initLoc;
                }
                else
                {
                    Instantiate(newTower, new Vector3(initLoc.x, initLoc.y, 0), Quaternion.identity, parent);
                    toDrag.position = nextLocation.position;
                    initLoc = nextLocation.position;
                    transform.SetParent(nextLocation);
                    transform.localScale = new Vector3(0.5f, 0.5f, 1);
                    GetComponent<DragTower>().enabled = false;
                }
            }
            dragging = false;
            hitObj = null;
        }

    }
}
