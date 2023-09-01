using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const int ENEMYID = 1000;
    const int ms = 1000;

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

    // 라운드 시작
    public bool isStarted = false;

    // Enemy 난이도 조절
    [Header("Enemy 난이도 조절")]

    // 적 스폰
    [SerializeField]private GameObject[] prefab_enemy;
    [SerializeField]private Transform spawnPoint;
    
    private List<Enemy> enemies = new List<Enemy>();
    int[] num = new int[4];

    List<Dictionary<string, string>> enemyData;
    int enemyIndex = 0;

    private IEnumerator SpawnEnemies() //int _num_Enemy, float _spawn_Speed)
    {
        Debug.Log("start 코루틴");

        Enemy temp;
        float time = 0;
        float spawnTime = 0;

        while (int.Parse(enemyData[enemyIndex]["Round"]) == round)
        {
            time += Time.deltaTime;


            //스폰시간 되면 스폰
            if (time >= spawnTime)
            {
                //버블 생성
                GameObject enemy = Instantiate(prefab_enemy[int.Parse(enemyData[enemyIndex]["EnemyType"]) - ENEMYID], spawnPoint.position, Quaternion.identity); //스폰
                temp = enemy.GetComponent<Enemy>();
                temp.setEnemy(int.Parse(enemyData[enemyIndex]["HealthPoint"]), int.Parse(enemyData[enemyIndex]["velocity"])); //setEnemy

                //배열로 버블 관리
                enemies.Add(temp);

                enemyIndex++;
                spawnTime = float.Parse(enemyData[enemyIndex]["SpawnTime"]) / ms; //스폰 시간 미리 세팅

                Debug.Log(spawnTime);
            }

            yield return null;  
        }
    }

    // 배열에서 적 제거
    public void RemoveEnemy(Enemy _enemy)
    {
        enemies.Remove(_enemy);
        // 적이 다 죽으면 다음 라운드 준비
        if(enemies.Count == 0)
        {
            if(round >= 10)
            {
                return;
            }

            int r = GetRoundNum();

            if (r == 3 || r == 6 || r == 9)
            {
                StartBuff();
            }

            ui.UpdateRound(GetRoundNum());
            ui.nextRoundBtn.SetActive(true);
            isStarted = false;
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
        //Debug.Log(heart);
        return heart;
    }

    // 게임 생명 닳았을 때
    private void GameOver()
    {
        Debug.Log("Game Over");
        // UI 띄우기
        ui.gameOverWindow.SetActive(true);
        // 모든 오브젝트 멈추기
        PauseGame();
    }

    // 게임 일시 정지
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    // 게임 회복
    public void ReleaseGame()
    {
        Time.timeScale = 1f;
    }

    private UIManager ui;
    public InGameData inGameData;

    // Start is called before the first frame update
    void Start()
    {
        ReleaseGame();
        ui = GetComponent<UIManager>();
        ui.UpdateStageCoin(inGameData.GetStageCoin());
        SoundManager.Instance.BGMToInGame();

        enemyData = ExelReader.Read("Data/inGame/Stage1");

        StartQuest();
    }


    //스테이지 코인 - 스테이지 내 재화 관리(의존: InGameData, UIManager)
    public void Coin(int coin)
    {
        inGameData.AddStageCoin(coin);
        ui.UpdateStageCoin(inGameData.GetStageCoin());
    }

    // 라운드 관리
    private int round = 0;

    public int GetRoundNum()
    {
        return round;
    }

    // 다음 라운드 버튼 누르면 시작
    public void StartRound()
    {
        isStarted = true;
        round++;
        ReleaseGame();
        StartCoroutine(SpawnEnemies());

        //Debug.Log(round);
    }

    // 사운드
    [Header("Sound Manager")]
    public AudioSource bubblePop;

    public SpriteRenderer grayMap;


    //SceneLoader
    public void NomalSceneLoader(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadingSceneLoader(string sceneName) //추후 구현 예정
    {
        SceneManager.LoadScene(sceneName);
    }

    //버프
    public void StartBuff()
    {
        Buff.Instance.play();
    }

    public void StartQuest()
    {
        Quest.Instance.play();
    }
}
