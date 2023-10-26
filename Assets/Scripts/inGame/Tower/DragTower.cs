using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragTower : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Image img;                          // 색깔 바꾸기
    private Vector2 initLoc;                    // UI 기본 위치
    private	Transform		canvasTrans;		// UI가 소속되어 있는 최상단의 Canvas Transform
	private	Transform		previousParent;		// 해당 오브젝트가 직전에 소속되어 있었던 부모 Transfron
	private	RectTransform	rect;				// UI 위치 제어를 위한 RectTransform
    private GameObject draggingTower;           // 월드에 동시에 배치되고 있는 타워
    private DetectRange draggingRange;          // 해당 타워 사거리
    private float posUI_x = 8f;                 // UI 화면 시작 좌표 (설치 불가능)

    public Canvas canvas;

    [SerializeField] private int towerCategory;      // 월드 맵에 배치할 타워 종류
    [SerializeField] private Transform hierarchy;// 타워 배치할 오브젝트 계층
    [SerializeField] private GameObject[] terrain;//설치 가능 구역 표시

    // 맵 그리드 데이터
    private List<Dictionary<string, string>> gridData;
    private List<List<int>> coords = new List<List<int>>();

    private void Start()
    {
        img = GetComponent<Image>();
        canvasTrans	= canvas.transform;
        rectParent = canvas.GetComponent<RectTransform>();
        rect		= GetComponent<RectTransform>();
        initLoc = rect.position;
        gridData = ExelReader.Read("Data/inGame/Grid");
        
        
        foreach (var co in gridData)
        {
            List<int> temp = new List<int>();
            temp.Add(int.Parse(co["xpos[px]"])+1140);
            temp.Add(int.Parse(co["ypos[py]"])+540);
            if(co["IsGround"] == "TRUE")
            {
                temp.Add(1);
            }
            else
            {
                temp.Add(0);
            }
            coords.Add(temp);
        }
        //Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(coords[0][0], coords[00][1], 0f)));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        img.color = new Color(1,1,1,0f);

        // 드래그 직전에 소속되어 있던 부모 Transform 정보 저장
        previousParent = transform.parent;
        // 현재 드래그중인 UI가 화면의 최상단에 출력되도록 하기 위해
        transform.SetParent(canvasTrans);       // 부모 오브젝트를 Canvas로 설정
        transform.SetAsLastSibling();		// 가장 앞에 보이도록 마지막 자식으로 설정

        // 드래그 시작 위치에 맞는 월드 좌표에 타워 배치
        mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
        renderPosition = mousePosition;
        draggingTower = CreateTower(towerCategory);
        draggingTower.name = draggingTower.name.Replace("(Clone)", "");
        draggingRange = draggingTower.GetComponentInChildren<DetectRange>();

        // 타워 설치 가능 구역 표시
        // foreach(GameObject map in terrain)
        // {
        //     map.SetActive(true);
        // }

        // 타워 설치 사거리 표시
        draggingRange.DisplayRange();
    }

    private Vector3 mousePosition;
    private Vector2 renderPosition;
    private RectTransform rectParent;
    private Vector2 localPos = Vector2.zero;
    private bool isCalculated = true;
    
    public void OnDrag(PointerEventData eventData)
    {
        if(!isCalculated)
        {
            return;
        }
        img.color = new Color(1,1,1,0f);

        // UI dragging
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent,
                                                                canvas.worldCamera.WorldToScreenPoint(mousePosition),
                                                                Camera.main, out localPos);
        rect.localPosition = localPos;
        // 마우스 좌표 받아서 월드 좌표로 계산 후 타워 동시 드래깅
        mousePosition = canvas.worldCamera.ScreenToWorldPoint(eventData.position);
        renderPosition = mousePosition;
        //draggingTower.transform.position = renderPosition;

        if (!SelectCoord())
        {
            draggingRange.sprite.color = Color.red;
        }
        else
        {
            draggingRange.sprite.color = Color.grey;
        }
        Debug.Log(nearestCoord);
        draggingTower.transform.position = nearestCoord;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        img.color = new Color(1,1,1,1);
        // 설치 가능한 구역이 아닐 때 타워 배치 불가
        if(!SelectCoord() || draggingTower.transform.position.x >= posUI_x)
        {
            Destroy(draggingTower);
        }
        else
        {
            
            draggingTower.transform.position = nearestCoord;
            // 합성 드래그 가능
            TowerController tempTc = draggingTower.GetComponent<TowerController>();
            tempTc.isInstantiated = true;
            tempTc.data = GetTowerData(towerCategory);
            
        Debug.Log(tempTc.data.skillCoolTime);

            draggingRange.transform.localScale = new Vector3(5, 5, 1);
            // // 타워 종류별 데이터 저장
            // int towerNum = int.Parse(draggingTower.name.Replace("Tower", ""));
            // TowerManager.Instance.towers[towerNum].Add(draggingTower);
            //타워 종류에 따른 사운드
            switch(towerCategory)
            {
                case 0:
                    SoundManager.Instance.EffectPlay(SoundManager.Instance.towerInstall[Random.Range(0, 3)]);
                    break;
                case 1:
                    SoundManager.Instance.EffectPlay(SoundManager.Instance.tower1Install[Random.Range(0, 3)]);
                    break;
                case 2:
                    //SoundManager.Instance.EffectPlay(SoundManager.Instance.tower2Install[Random.Range(0, 3)]);
                    break;
                default:
                    SoundManager.Instance.EffectPlay(SoundManager.Instance.towerInstall[Random.Range(0, 3)]);
                    break;
            }
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
        // foreach(GameObject map in terrain)
        // {
        //     map.SetActive(false);
        // }

        // 타워 설치 사거리 비표시
        draggingRange.ClearRange();
    }


    // Calculate the nearest Coordination
    private Vector3 nearestCoord = Vector3.zero;
    [SerializeField] private float offset = 10f;
    //0.4824561403508772f;
    private bool SelectCoord()
    {
        isCalculated = false;
        if (nearestCoord == Vector3.zero || Vector3.Distance(nearestCoord, renderPosition) >= offset)
        {
            float min_distance;
            Vector3 first = new Vector3(coords[0][0], coords[0][1], 0);
            first = Camera.main.ScreenToWorldPoint(first);
            first.z = 0f;
            min_distance = Vector3.Distance(first, renderPosition);
            foreach (List<int> co in coords)
            {
                Vector3 pivot = new Vector3(co[0], co[1], 0);
                pivot = Camera.main.ScreenToWorldPoint(pivot);
                pivot.z = 0f;
                //Debug.Log(Vector3.Distance(pivot, renderPosition)));
                if (Vector3.Distance(pivot, renderPosition) < min_distance)
                {
                    nearestCoord = pivot;
                    min_distance = Vector3.Distance(pivot, renderPosition);
                    if(co[2] == 0)
                    {
                        isCalculated = true;
                        return false;
                    }
                }
            }
        }
        //Debug.Log(nearestCoord);
        // if(nearestCoord != Vector3.zero && Vector3.Distance(nearestCoord, renderPosition) >= offset)
        // {
        //     nearestCoord = Vector3.zero;
        // }
        isCalculated = true;
        return true;
    }

    private Tower GetTowerData(int category)
    {
        switch(category)
        {
            case 0:
            return TowerManager.Instance.daebakInfo.curTower;
            case 1:
            return TowerManager.Instance.nabiInfo.curTower;
            case 2:
            return TowerManager.Instance.toriInfo.curTower;
            default:
            return TowerManager.Instance.daebakInfo.curTower;
        }
    }

    private GameObject CreateTower(int category)
    {
        switch(category)
        {
            case 0:
            return TowerManager.Instance.daebakInfo.GetDaebak();
            case 1:
            return TowerManager.Instance.nabiInfo.GetNabi();
            case 2:
            return TowerManager.Instance.toriInfo.GetTori();
            default:
            return TowerManager.Instance.daebakInfo.GetDaebak();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
