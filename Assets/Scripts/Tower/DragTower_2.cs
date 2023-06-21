using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragTower_2 : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Image img;                          // 색깔 바꾸기
    private Vector2 initLoc;                    // UI 기본 위치
    private Canvas canvas;
    private	Transform		canvasTrans;			// UI가 소속되어 있는 최상단의 Canvas Transform
	private	Transform		previousParent;		// 해당 오브젝트가 직전에 소속되어 있었던 부모 Transfron
	private	RectTransform	rect;				// UI 위치 제어를 위한 RectTransform


    private void Start()
    {
        img = GetComponent<Image>();
        canvas = FindObjectOfType<Canvas>();
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
		transform.SetParent(canvasTrans);		// 부모 오브젝트를 Canvas로 설정
		transform.SetAsLastSibling();		// 가장 앞에 보이도록 마지막 자식으로 설정

        print("OnBeginDrag");
    }

    private Vector3 mousePosition;
    private Vector2 rectPosition;
    private RaycastHit2D hit;
    
    public void OnDrag(PointerEventData eventData)
    {
        img.color = new Color(1,1,1,0.5f);

        mousePosition = eventData.position;
        print("eventData Position: " + mousePosition);
        rectPosition = new();
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(this.transform as RectTransform, mousePosition, canvas.worldCamera, out rectPosition);
        
        mousePosition = canvas.worldCamera.ScreenToWorldPoint(eventData.position);
        
        print("world Position: " + mousePosition);
        rectPosition = mousePosition;
        
        print("Rect Position: " + rectPosition);
        rect.localPosition = rectPosition;

        hit = Physics2D.Raycast(mousePosition, transform.forward, 15f);
        Debug.DrawRay(mousePosition, transform.forward * 10, Color.red, 0.3f);


        print("OnDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        img.color = new Color(1,1,1,1);
        
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
        

        print("OnEndDrag");
    }


    public void OnDrop(PointerEventData eventData)
    {
        
        print("OnDrop");
    }
}
