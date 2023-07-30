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
        data.time = 1f;
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

    private float angle;
    private GameObject bull;
    public void Attack()
    {
        if(enemies.Count==0)
        {
            return;
        }

        // 감지된 적 있을 때 바라보며 공격하기
        angle = Mathf.Atan2(enemies[0].transform.position.y - transform.position.y,
                            enemies[0].transform.position.x - transform.position.x)
              * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 270);

        // Bullet 생성하여 적을 향해 이동시키기
        foreach (GameObject enemy in enemies)
        {
            bull = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
            bullCtr = bull.GetComponent<BulletController>();
            bullCtr.setATK(data.attack);
            bullCtr.TriggerMove(enemy.transform);
        }
    }

}
