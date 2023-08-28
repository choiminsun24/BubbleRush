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

                    towerController = touchedObject.GetComponent<TowerController>();
                    if (!towerController && !(towerController.isInstantiated))
                    {
                        return;
                    }


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

        if (!towerController || !(towerController.isInstantiated))
        {
            return;
        }

        if (dragging && touch.phase == TouchPhase.Moved)
        {
            // 터치 좌표를 월드 좌표로 계산
            vec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            vec = Camera.main.ScreenToWorldPoint(vec);
            touchedObject.transform.position = vec;

            if (fusionRange.canFuse == true)
            {
                sprite.color = new Color(0.2f, 1f, 0.2f, 0.8f);
            }
            else
            {
                sprite.color = new Color(1f, 1f, 1f, 1f);
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
                        Destroy(touchedObject, 0.1f);
                    }
                    else
                    {
                        return;
                    }
                }
            }

            towerController = null;
            grayMap.gameObject.SetActive(false);
            dragging = false;

            touchedObject.transform.position = initPos;
        }
    }
}
