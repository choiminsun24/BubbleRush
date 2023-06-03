using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Enemy 난이도 조절
    [Header("Enemy 난이도 조절")]
    public int num_Enemy = 10;
    public float spawn_Speed = 3f;
    public float move_Speed = 2f;
    public int hp_Enemy = 20;


    // 적 스폰
    [SerializeField]private Object prefab_enemy;
    [SerializeField]private Transform spawnPoint;
    //private EnemyMove[] enemies;
    int[] num = new int[4];
    private IEnumerator SpawnEnemies(int _num_Enemy, float _spawn_Speed, float _move_Speed, int _hp_Enemy)
    {
        for (int i=0; i<_num_Enemy; i++)
        {
            GameObject enemy = Instantiate(prefab_enemy, spawnPoint.position, Quaternion.identity) as GameObject;
            //enemy.GetComponent<EnemyMove>().speed = _move_Speed;
            //enemy.GetComponent<EnemyMove>().hp = _hp_Enemy;
            //enemy.GetComponent<EnemyMove>().hp = _hp_Enemy;
            
            yield return new WaitForSeconds(_spawn_Speed);
        }
    }


    // 게임 생명 개수
    private int heart = 3;
    // 적이 도착지점에 도착하였을 때 -1, 아이템을 먹었을 때 +1
    public void AddHeart(int _heart)
    {
        heart += _heart;
        if(heart <= 0)
            GameOver();
    }
    public int GetHeart()
    {
        Debug.Log(heart);
        return heart;
    }

    // 게임 생명 닳았을 때
    private void GameOver()
    {
        Debug.Log("Game Over");
        // 모든 오브젝트 멈추기
        // UI 띄우기
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies(num_Enemy, spawn_Speed, move_Speed, hp_Enemy));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
