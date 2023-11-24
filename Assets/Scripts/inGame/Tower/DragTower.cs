using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
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

    public Canvas canvas;

    [SerializeField] private int towerCategory;      // 월드 맵에 배치할 타워 종류
    [SerializeField] private Transform hierarchy;// 타워 배치할 오브젝트 계층

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
            temp.Add(int.Parse(co["xpos[px]"])+1060);
            temp.Add(int.Parse(co["ypos[py]"])+540);
            //Instantiate(Resources.Load("Daebak"), Camera.main.ScreenToWorldPoint(new Vector3(temp[0], temp[1], 0f)), Quaternion.identity, GameManager.Instance.transform);
            if (co["IsGround"] == "TRUE")
            {
                temp.Add(1);
            }
            else
            {
                temp.Add(0);
            }
            temp.Add(int.Parse(co["column"]));
            coords.Add(temp);
        }
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

        // 타워 설치 사거리 표시
        draggingRange.DisplayRange();
    }

    private Vector3 mousePosition;
    private Vector3 screenPosition;
    private Vector2 renderPosition;
    private RectTransform rectParent;
    private Vector2 localPos = Vector2.zero;
    private bool canLocate = true;
    private bool canCalculate = true;
    
    public void OnDrag(PointerEventData eventData)
    {
        img.color = new Color(1,1,1,0f);

        // UI dragging
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent,
                                                                canvas.worldCamera.WorldToScreenPoint(mousePosition),
                                                                Camera.main, out localPos);
        rect.localPosition = localPos;
        // 마우스 좌표 받아서 월드 좌표로 계산 후 타워 동시 드래깅
        mousePosition = canvas.worldCamera.ScreenToWorldPoint(eventData.position);
        renderPosition = mousePosition;

        screenPosition = Input.mousePosition;

        SelectCoord();
        draggingTower.transform.position = nearestCoord;
        if (!canLocate)
        {
            draggingRange.sprite.color = Color.red;
        }
        else
        {
            draggingRange.sprite.color = Color.grey;
        }
    }

    private Vector3 [] offsets = {new Vector3(-0.1f, -0.1f, 0f), new Vector3(0.3f, 0f, 0f), new Vector3(0.1f, -0.1f, 0f),
                                    new Vector3(-0.17f, -0.17f, 0f), new Vector3(-0.0f, -0.1f, 0f), new Vector3(0f, 0f, 0f)};
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log(SelectCoord(draggingTower.transform));
        img.color = new Color(1,1,1,1);
        // 설치 가능한 구역이 아닐 때 타워 배치 불가
        if(!canLocate || draggingTower.transform.position.x >= 8f)
        {
            DeleteTower(towerCategory, draggingTower);
        }
        else
        {
            
            draggingTower.transform.position = nearestCoord + offsets[towerCategory];
            

            AfterInstall(draggingTower);
        }

        // 드래그를 시작하면 부모가 canvas로 설정되기 때문에
		// 드래그를 종료할 때 부모가 canvas이면 아이템 슬롯이 아닌 엉뚱한 곳에
		// 드롭을 했다는 뜻이기 때문에 드래그 직전에 소속되어 있던 아이템 슬롯으로 아이템 이동
		if ( transform.parent == canvasTrans )
		{
			// 마지막에 소속되어있었던 previousParent의 자식으로 설정하고, 해당 위치로 설정
			transform.SetParent(previousParent);
            // UI 기존 위치로 변경
            rect.position = new Vector3(initLoc.x, initLoc.y, 0);
        }


    }

    private void AfterInstall(GameObject tower)
    {
        // 합성 드래그 가능
        TowerController tempTc = tower.GetComponent<TowerController>();
        tempTc.isInstantiated = true;
        tempTc.data = GetTowerData(towerCategory);
        

        draggingRange.transform.localScale = new Vector3(5, 5, 1);
        // // 타워 종류별 데이터 저장
        // int towerNum = int.Parse(tower.name.Replace("Tower", ""));
        // TowerManager.Instance.towers[towerNum].Add(tower);
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
        // 타워 설치 사거리 비표시
        draggingRange.ClearRange();
    }

    // Calculate the nearest Coordination
    private Vector3 nearestCoord = Vector3.zero;
    private Vector3 lastCoord = Vector3.zero;
    [SerializeField] private float offset = 0.1f;
    private int lastCol;
    //0.4824561403508772f;
    private Vector3 SelectCoord()
    {
        float min_distance = float.MaxValue;
        screenPosition = Input.mousePosition;
        screenPosition = new Vector3(screenPosition.x, screenPosition.y, 0f);
        if (nearestCoord == Vector3.zero || Vector3.Distance(lastCoord, screenPosition) >= offset)
        {
            for (int i=0; i<coords.Count; ++i)
            {
                // 거리 비교할 때는 스크린 좌표로 비교
                List<int> co = coords[i];
                Vector3 pivot = new Vector3(co[0], co[1], 0);
                //Debug.Log("[" + i + "] " + "pivot: " + pivot + " screenPosition: " + screenPosition+" Distance: "+ Vector2.Distance(pivot, screenPosition));
                if (Vector2.Distance(pivot, screenPosition) < min_distance)
                {
                    min_distance = Vector2.Distance(pivot, screenPosition);
                    //Debug.Log("[" + i + "] " + "pivot: " + pivot + " min_distance: " + min_distance);
                    lastCoord = pivot;
                    // 월드 좌표로 변환해서 저장
                    pivot = canvas.worldCamera.ScreenToWorldPoint(pivot);
                    pivot = new Vector3(pivot.x, pivot.y, 0f);
                    nearestCoord = pivot;
                    if(co[2] == 0)
                    {
                        canLocate = false;
                    }
                    else
                    {
                        draggingTower.transform.position = nearestCoord;
                        canLocate = true;
                    }
                    lastCol = co[3];
                    //Debug.Log("lastCol: "+lastCol);
                }
            }
        }
        return nearestCoord;
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
            case 3:
            return TowerManager.Instance.gobyInfo.curTower;
            case 4:
                return TowerManager.Instance.tutuInfo.curTower;
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
            case 3:
                return TowerManager.Instance.gobyInfo.GetGoby();
            case 4:
                return TowerManager.Instance.tutuInfo.GetTutu();
            default:
            return TowerManager.Instance.daebakInfo.GetDaebak();
        }
    }

    private void DeleteTower(int category, GameObject obj)
    {
        switch(category)
        {
            case 0:
            TowerManager.Instance.daebakInfo.ReturnDaebak(obj);
            break;
            case 1:
            TowerManager.Instance.nabiInfo.ReturnNabi(obj);
            break;
            case 2:
            TowerManager.Instance.toriInfo.ReturnTori(obj);
            break;
            case 3:
                TowerManager.Instance.gobyInfo.ReturnGoby(obj);
                break;
            case 4:
                TowerManager.Instance.tutuInfo.ReturnTutu(obj);
                break;
            default:
            TowerManager.Instance.daebakInfo.ReturnDaebak(obj);
            break;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}