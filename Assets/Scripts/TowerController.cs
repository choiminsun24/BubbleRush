using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private Tower data;
    // Start is called before the first frame update
    void Start()
    {
        data = new Tower();
        data.hp = 10;
        data.attack = 1;
        data.range = 3;
        data.time = 2f;
    }
    private GameObject[] enemies; 
    // 타워 근처 적 감지 범위
    private void DetectEnemies()
    {
        
    }

    void OnDrawGizmos()
    {
        float maxDistance = 100;
        RaycastHit hit;
        // Physics.Raycast (레이저를 발사할 위치, 발사 방향, 충돌 결과, 최대 거리)
        bool isHit = Physics.Raycast (transform.position, transform.forward, out hit, maxDistance);
 
        Gizmos.color = Color.red;
        if (isHit) {
            Gizmos.DrawRay (transform.position, transform.forward * hit.distance);
        } else {
            Gizmos.DrawRay (transform.position, transform.forward * maxDistance);
        }
    }

    // 일정한 주기로 공격
    private float time = 0f;
    // Update is called once per frame
    void Update()
    {

        time+=Time.deltaTime;
        if(time>=data.time)
        {
            time = 0f;
            Attack();
        }
    }
    public void Attack()
    {

    }
}
