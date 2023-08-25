using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    private bool dragging = false;
    private Touch touch;
    private Vector3 initPos;
    private Vector2 vec;
    private SpriteRenderer sprite;

    private int towerCategory = 1;
    private TowerController towerController;
    private GameObject touchedObject;
    [SerializeField] private SpriteRenderer grayMap;
    private FusionRange fusionRange;

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
        initPos = touch.position;

//1번만 실행되어야함
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPos, Camera.main.transform.forward);

            Debug.Log(hitInformation.collider.gameObject.name);

            if (hitInformation.collider != null)
            {
                touchedObject = hitInformation.transform.gameObject;
                if (touchedObject.tag =="Tower")
                {
                    // 터치 좌표를 월드 좌표로 계산
                    initPos = Input.mousePosition;
                    vec = new Vector2(initPos.x, initPos.y);
                    vec = Camera.main.ScreenToWorldPoint(vec);

                    Debug.Log(touchedObject.gameObject.name);

                    sprite = touchedObject.GetComponent<SpriteRenderer>();
                    sprite.color = new Color(1, 1, 1, 0.5f);
                    fusionRange = touchedObject.GetComponent<FusionRange>();
                    fusionRange.gameObject.SetActive(true);
                    if (!grayMap)
                    {
                        grayMap = GameManager.Instance.grayMap;
                    }
                    grayMap.gameObject.SetActive(true);
                    //grayMap layer 조종 필요
                    dragging = true;
                }
            }
        }

        if (dragging && touch.phase == TouchPhase.Moved)
        {
            // dragging
            vec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            vec = Camera.main.ScreenToWorldPoint(vec);
            touchedObject.transform.position = vec;

            if (fusionRange.canFuse == true)
            {
                sprite.color = new Color(0.5f, 1f, 0.5f, 0.8f);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            sprite.color = new Color(1, 1, 1, 1f);

            if (dragging == true)
            {
                if (fusionRange.canFuse == true)
                {
                    towerController = fusionRange.targetTower.GetComponent<TowerController>();
                    if (towerController)
                    {
                        towerController.LevelUp();
                    }
                    else
                    {
                        return;
                    }
                    Destroy(this.gameObject, 0.5f);
                }
            }
            fusionRange.gameObject.SetActive(false);
            grayMap.gameObject.SetActive(false);
            dragging = false;
        }
    }
}
