using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower0Attack : MonoBehaviour
{
    [SerializeField] private Transform skillGuage;
    private TowerController tc;
    private float coolTime, skillTime = 0f;
    private bool canTongue = false;
    // Start is called before the first frame update
    void Start()
    {
        tc = GetComponent<TowerController>();
        coolTime = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.isStarted)
        {
            // 라운드 끝나면 false 되는 시기 넣어야함
            return;
        }
        skillTime += Time.deltaTime;
        if (skillTime >= coolTime)
        {
            canTongue = true;
            Attack();
        }
        else
        {
            skillGuage.localScale = new Vector3(1 / coolTime * skillTime, skillGuage.localScale.y, skillGuage.localScale.z);
        }
    }

    private void Attack()
    {
        if(!canTongue)
        {
            return;
        }
        Debug.Log("깨물기 공격");
        SoundManager.Instance.EffectPlay(SoundManager.Instance.skillBite[Random.Range(0,4)]);
        skillTime = 0;
    }
}
