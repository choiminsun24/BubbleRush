using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static GameManager Instance
    {
        get {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if(!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // 인스턴스가 존재하는 경우 기존 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(_instance.gameObject);
        }
        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }

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
            enemy.GetComponent<Enemy>().setEnemy(20, 100);
            
            yield return new WaitForSeconds(_spawn_Speed);
        }
    }


    // 게임 생명 개수
    private int heart = 3;
    // 적이 도착지점에 도착하였을 때 -1, 아이템을 먹었을 때 +1
    public void AddHeart(int _heart)
    {
        print("Add Heart: "+_heart.ToString());
        heart += _heart;
        ui.UpdateHearts(heart);
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
        // UI 띄우기
        ui.gameOverWindow.SetActive(true);
        // 모든 오브젝트 멈추기
        Time.timeScale = 0f;
    }

    private UIManager ui;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(SpawnEnemies(num_Enemy, spawn_Speed, move_Speed, hp_Enemy));
        ui = GetComponent<UIManager>();
    }

    public void StartCore()
    {
        Time.timeScale = 1;
        StartCoroutine(SpawnEnemies(num_Enemy, spawn_Speed, move_Speed, hp_Enemy));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
