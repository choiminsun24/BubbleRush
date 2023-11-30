using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower0Attack : MonoBehaviour
{
    [SerializeField] private GameObject tongue;
    [SerializeField] private Animator auraAnim;
    private bool canAnimate = true;


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
        if (!GameManager.Instance.isStarted)
        {
            // 라운드 끝나면 false 되는 시기 넣어야함
            return;
        }
        skillTime += Time.deltaTime;
        skillTime_1 += Time.deltaTime;
        if (tc.canTongue && canAnimate)
        {
            // 뼈 일반 공격
            tc.anim.SetBool("isUp", false);
            tc.anim.SetBool("isAttack", true);
            auraAnim.SetBool("isAttack", true);
            canAnimate = false;
        }
        // if (skillTime_1 >= coolTime_1)
        // {
            
        // }
        else if(!tc.canTongue)
        {
            tc.anim.SetBool("isAttack", false);
            auraAnim.SetBool("isAttack", false);
            canAnimate = true;
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
