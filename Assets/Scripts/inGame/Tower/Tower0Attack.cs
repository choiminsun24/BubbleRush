using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Tower0Attack : MonoBehaviour
{
    [SerializeField] private GameObject tongue;
    [SerializeField] private Animator auraAnim;
    private bool canAnimate = true;

    // For Skill Guage
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject fill;
    [SerializeField] private RectTransform skillGuage;
    private Canvas canvas;
    private RectTransform rectParent;
    private Vector2 localPos = Vector2.zero;
    private bool isZero = false;


    private TowerController tc;
    private float coolTime, skillTime, coolTime_1, skillTime_1 = 0f;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
        rectParent = canvas.GetComponent<RectTransform>();

        skillGuage.transform.SetParent(rectParent);
        skillGuage.transform.SetAsLastSibling();


        tc = GetComponent<TowerController>();
        coolTime_1 = 2f;
        coolTime = 20f;

    }

    // Update is called once per frame
    void Update()
    {
        // 합성 후 게이지 초기화
        if (tc.isFused)
        {
            slider.value = 0;
            tc.isFused = false;
        }
        if (!GameManager.Instance.isStarted)
        {
            // 일반 공격 진행중이었다면 초기화
            if (canAnimate == false || tc.anim.GetBool("isAttack") == true)
            {
                tc.anim.SetBool("isAttack", false);
                auraAnim.SetBool("isAttack", false);
                skillTime_1 = 0f;
                canAnimate = true;
            }
            // 라운드 끝나면 false 되는 시기 넣어야함
            return;
        }
        else
        {
            // Skill Guage UI Position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent,
                                                                    Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.1f, -0.15f, 0)),
                                                                    Camera.main, out localPos);
            skillGuage.localPosition = localPos;
            skillGuage.localScale = new Vector3(1, 1, 1);
            
            // 게이지 fill
            slider.value = 1 / coolTime * skillTime;

            isZero = slider.value == 0 ? false : true;
            fill.SetActive(isZero);
        }

        

        skillTime += Time.deltaTime;
        skillTime_1 += Time.deltaTime;
        // 가까운 적 존재 && 애니메이션 종료 && 쿨타임 만족
        if (tc.canTongue && canAnimate && skillTime_1 <= coolTime_1)
        {
            // 뼈 일반 공격
            tc.anim.SetBool("isUp", false);
            tc.anim.SetBool("isAttack", true);
            auraAnim.SetBool("isAttack", true);
            canAnimate = false;
        }
        // 일반 공격 duration 지났을 때 초기화
        else if(skillTime_1>=coolTime_1)
        {
            tc.anim.SetBool("isAttack", false);
            auraAnim.SetBool("isAttack", false);
            skillTime_1 = 0f;
            canAnimate = true;
        }

        // 일반 공격 이후 스킬 공격 가능 시
        if (tc.isAttacking && skillTime >= coolTime)
        {
            // 스킬 공격
            Attack();
            tc.isAttacking = false;
            skillTime = 0f;
        }
        else
        {
            
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
