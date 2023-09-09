using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower0Attack : MonoBehaviour
{
    [SerializeField] private GameObject tongue;
    // For Skill Guage
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject fill;
    [SerializeField] private RectTransform skillGuage;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform rectParent;
    private Vector2 localPos = Vector2.zero;
    private bool isZero = false;


    private TowerController tc;
    private float coolTime, skillTime, coolTime_1, skillTime_1 = 0f;
    // Start is called before the first frame update
    void Start()
    {
        tc = GetComponent<TowerController>();
        coolTime_1 = 2f;
        skillTime_1 = coolTime_1;
        coolTime = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!tc.isAttacking)
        {
            // Skill Guage UI Position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent,
                                                                    Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.6f, 0)),
                                                                    canvas.worldCamera, out localPos);
            skillGuage.localPosition = localPos;
        }
        
        isZero = slider.value == 0 ? false : true;
        fill.SetActive(isZero);
        

        if (!GameManager.Instance.isStarted)
        {
            // 라운드 끝나면 false 되는 시기 넣어야함
            return;
        }
        skillTime += Time.deltaTime;
        skillTime_1 += Time.deltaTime;
        if (skillTime_1 >= coolTime_1)
        {
            // 혀 일반 공격
            tongue.SetActive(tc.canTongue);
        }
        else if(skillTime_1 <= coolTime_1*2)
        {
            tongue.SetActive(false);
        }
        else
        {
            skillTime_1 = 0f;
        }

        if (skillTime >= coolTime)
        {
            Attack();
        }
        else
        {
            slider.value = 1 / coolTime * skillTime;
        }
    }

    private void Attack()
    {
        if(!tc.canTongue)
        {
            return;
        }
        Debug.Log("깨물기 공격");
        SoundManager.Instance.EffectPlay(SoundManager.Instance.skillBite[Random.Range(0,4)]);
        skillTime = 0;
    }
}
