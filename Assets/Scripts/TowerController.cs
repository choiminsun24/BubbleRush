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
    [SerializeField] private Object bullet;
    // 타워 근처 적 감지 범위
    private void DetectEnemies(GameObject enemy)
    {
        GameObject bull = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        bull.GetComponent<BulletController>().TriggerMove(enemy.transform);
    }
    void OnCreate() {}
    void OnUpdate() {}
    void OnDrawGizmos()
    {
        float maxDistance = 2f;
        RaycastHit2D hit;
        // Physics.Raycast (레이저를 발사할 위치, 발사 방향, 충돌 결과, 최대 거리)
        hit = Physics2D.CircleCast(transform.position, maxDistance, Vector2.up, maxDistance);
        
        
        if(hit && hit.collider.gameObject.tag == "Player")
        {
            print("ENEMY DETECTED");
            Debug.DrawRay(transform.position, new Vector2(-1,0) * hit.distance, Color.red);
            DetectEnemies(hit.collider.gameObject);
        }
        else
            Debug.DrawRay(transform.position, new Vector2(-1,0) * 2f, Color.gray);
        
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
