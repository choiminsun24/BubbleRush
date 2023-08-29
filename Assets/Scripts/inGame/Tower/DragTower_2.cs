using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragTower_2 : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Image img;                          // 색깔 바꾸기
    private Vector2 initLoc;                    // UI 기본 위치
    private	Transform		canvasTrans;		// UI가 소속되어 있는 최상단의 Canvas Transform
	private	Transform		previousParent;		// 해당 오브젝트가 직전에 소속되어 있었던 부모 Transfron
	private	RectTransform	rect;				// UI 위치 제어를 위한 RectTransform
    private GameObject draggingTower;           // 월드에 동시에 배치되고 있는 타워
    private DetectRange draggingRange;          // 해당 타워 사거리

    public Canvas canvas;

    [SerializeField] private Object tower;      // 월드 맵에 배치할 타워 프리팹
    [SerializeField] private Transform hierarchy;// 타워 배치할 오브젝트 계층
    [SerializeField] private GameObject[] terrain;//설치 가능 구역 표시


    private void Start()
    {
        img = GetComponent<Image>();
        canvasTrans	= canvas.transform;
		rect		= GetComponent<RectTransform>();
        initLoc = rect.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        img.color = new Color(1,1,1,0.5f);

        // 드래그 직전에 소속되어 있던 부모 Transform 정보 저장
        previousParent = transform.parent;
        // 현재 드래그중인 UI가 화면의 최상단에 출력되도록 하기 위해
        transform.SetParent(canvasTrans);       // 부모 오브젝트를 Canvas로 설정
        transform.SetAsLastSibling();		// 가장 앞에 보이도록 마지막 자식으로 설정

        // 드래그 시작 위치에 맞는 월드 좌표에 타워 배치
        mousePosition = canvas.worldCamera.ScreenToWorldPoint(eventData.position);
        renderPosition = mousePosition;
        draggingTower = Instantiate(tower, new Vector3(renderPosition.x, renderPosition.y, 0f), Quaternion.identity) as GameObject;
        draggingTower.name = draggingTower.name.Replace("(Clone)", "");
        draggingRange = draggingTower.GetComponentInChildren<DetectRange>();

        // 타워 설치 가능 구역 표시
        foreach(GameObject map in terrain)
        {
            map.SetActive(true);
        }

        // 타워 설치 사거리 표시
        draggingRange.DisplayRange();
    }

    private Vector3 mousePosition;
    private Vector2 renderPosition;
    
    public void OnDrag(PointerEventData eventData)
    {
        img.color = new Color(1,1,1,0.5f);

        // UI dragging
        transform.position = Input.mousePosition;
        // 마우스 좌표 받아서 월드 좌표로 계산 후 타워 동시 드래깅
        mousePosition = canvas.worldCamera.ScreenToWorldPoint(eventData.position);
        renderPosition = mousePosition;
        draggingTower.transform.position = renderPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        img.color = new Color(1,1,1,1);

        // 설치 가능한 구역이 아닐 때 타워 배치 불가
        if(draggingRange.hit == false)
        {
            Destroy(draggingTower);
        }
        else
        {
            // 합성 드래그 가능
            draggingTower.GetComponent<TowerController>().isInstantiated = true;
            // 타워 종류별 데이터 저장
            TowerManager.Instance.towers[int.Parse(draggingTower.name.Replace("Tower", ""))].Push(draggingTower);
        }

        // 드래그를 시작하면 부모가 canvas로 설정되기 때문에
		// 드래그를 종료할 때 부모가 canvas이면 아이템 슬롯이 아닌 엉뚱한 곳에
		// 드롭을 했다는 뜻이기 때문에 드래그 직전에 소속되어 있던 아이템 슬롯으로 아이템 이동
		if ( transform.parent == canvasTrans )
		{
			// 마지막에 소속되어있었던 previousParent의 자식으로 설정하고, 해당 위치로 설정
			transform.SetParent(previousParent);
			rect.position = previousParent.GetComponent<RectTransform>().position;
		}

        
        // 기본 위치로 변경
        rect.position = new Vector3(initLoc.x, initLoc.y, 0);
        
        // 타워 설치 가능 구역 비표시
        foreach(GameObject map in terrain)
        {
            map.SetActive(false);
        }

        // 타워 설치 사거리 비표시
        draggingRange.ClearRange();
    }


    public void OnDrop(PointerEventData eventData)
    {

    }
}
