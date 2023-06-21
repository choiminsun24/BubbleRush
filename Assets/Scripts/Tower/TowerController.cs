using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private Tower data;
    BulletController bullCtr;
    // Start is called before the first frame update
    void Start()
    {
        data = new Tower();
        data.hp = 10;
        data.attack = 5;
        data.range = 3;
        data.time = 0.5f;
    }
    public List<GameObject> enemies;
    [SerializeField] private Object bullet;
    // 타워 근처 적 감지 범위
    public void DetectEnemies(GameObject enemy)
    {
        if (enemies.IndexOf(enemy) == -1)
        {
            enemies.Add(enemy);
        }
    }

    // 적이 감지 범위에서 벗어났을 때
    public void RemoveEnemies(GameObject enemy)
    {
        if (enemies.IndexOf(enemy) != -1)
        {
            GameObject temp = enemies.Find(element => element == enemy);
            enemies.Remove(temp);
        }
    }

    void OnCreate() {}
    void OnUpdate() {}
    void OnDrawGizmos()
    {
        // float maxDistance = 2f;
        // RaycastHit2D hit;
        // // Physics.Raycast (레이저를 발사할 위치, 발사 방향, 충돌 결과, 최대 거리)
        // hit = Physics2D.CircleCast(transform.position, maxDistance, Vector2.up, maxDistance);
        
        
        // if(hit && hit.collider.gameObject.tag == "Player")
        // {
        //     print("ENEMY DETECTED");
        //     Debug.DrawRay(transform.position, new Vector2(-1,0) * hit.distance, Color.red);
        //     DetectEnemies(hit.collider.gameObject);
        // }
        // else
        //     Debug.DrawRay(transform.position, new Vector2(-1,0) * 2f, Color.gray);
        
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
        foreach (GameObject enemy in enemies)
        {
            GameObject bull = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
            bullCtr = bull.GetComponent<BulletController>();
            bullCtr.setATK(data.attack);
            bullCtr.TriggerMove(enemy.transform);
        }
    }
}
