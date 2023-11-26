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

    public List<Enemy> enemies = new List<Enemy>();
    // 라운드 시작
    public bool isStarted {get; set;} = false;
    // 라운드 당 버블(적) 수
    private bool lastSpawn = false; 

    // Enemy 난이도 조절
    [Header("Enemy 난이도 조절")]

    // 적 스폰
    [SerializeField]private GameObject[] prefab_enemy;
    [SerializeField]private Transform spawnPoint;
    
    List<Dictionary<string, string>> enemyData;

    public Transform canvas;
    private int enemyIndex = 0;

    private IEnumerator SpawnEnemies() //int _num_Enemy, float _spawn_Speed)
    {
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
                if (enemyIndex >= enemyData.Count)
                {
                    lastSpawn = true;
                    yield break;
                }
                spawnTime = float.Parse(enemyData[enemyIndex]["SpawnTime"]) / ms; //스폰 시간 미리 세팅
            }

            yield return null;  
        }

        lastSpawn = true;
    }

    // 배열에서 적 제거
    public void RemoveEnemy(Enemy _enemy)
    {
        enemies.Remove(_enemy);

        // 적이 다 죽으면 다음 라운드 준비
        if (lastSpawn == true && enemies.Count == 0)
        {
            NextRound();
        }
    }

    private void NextRound()
    {
        isStarted = false;
        ui.offFast();

        if (round >= 10)
        {
            ui.Win();
            return;
        }

        int r = GetRoundNum();

        if (heart > 0 && (r == 2 || r == 5 || r == 8))
        {
            StartBuff();
            if(isAuto)
            {
                return;
            }
        }

        if (isAuto)
        {
            StartRound();
        }

        ui.nextRoundBtn.SetActive(true);
    }

    // Auto 모드
    private bool isAuto;

    public bool getAuto()
    {
        return isAuto;
    }

    public void autoOn()
    {
        isAuto = true;
    }

    public void autoOff()
    {
        isAuto = false;
    }


    //게임 생명 개수
    private int heart = 3;

    //적이 도착지점에 도착하였을 때 -1, 아이템을 먹었을 때 +1
    public void AddHeart(int _heart)
    {
        heart += _heart;

        //변동 수만큼 ui 매니저 호출
        if (_heart < 0)
            for (int i = 0; i > _heart; i--)
                ui.minusHeart();
        else if (heart > 0)
            for (int i = 0; i < _heart; i++)
                ui.plusHeart();
        else
            Debug.Log("0일거면 메소드 왜 씀?");

        //0개가 되면 게임오버
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
        SoundManager.Instance.EffectPlay(SoundManager.Instance.die);
        Debug.Log("Game Over");
        // UI 띄우기
        ui.gameOverWindow.SetActive(true);
        // 모든 오브젝트 멈추기
        Invoke("PauseGame", 0.3f);
    }

    // 게임 일시 정지
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    // 게임 회복
    public void ReleaseGame()
    {
        Time.timeScale = (float)fastLevel;
    }

    private UIManager ui;
    public InGameData inGameData;

    // Start is called before the first frame update
    void Start()
    {
        fastLevel = 1;
        ReleaseGame();

        ui = GetComponent<UIManager>();
        ui.UpdateStageCoin(inGameData.GetStageCoin());
        SoundManager.Instance.BGMPlay(SoundManager.Instance.inStage);

        enemyData = ExelReader.Read("Data/inGame/Stage1");

        isAuto = false; ///싱글톤 유지하게 되면 수정

        //StartBuff();
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
    public int fastLevel; 

    public void StartRound()
    {
        isStarted = true;
        lastSpawn = false;
        round++;
        ui.UpdateRound(GetRoundNum());
        ui.onFast();
        ReleaseGame();
        StartCoroutine(SpawnEnemies());
    }

    public void clickFast()
    {
        if (fastLevel == 1)
            fastLevel = 2;
        else if (fastLevel == 2)
            fastLevel = 1;

        ui.updateFast(fastLevel);
        Time.timeScale = (float)fastLevel;
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

    public Buff buff;
    public Quest quest;

    //버프
    public void StartBuff()
    {
        buff.play();
        SoundManager.Instance.EffectPlay(SoundManager.Instance.card);
    }

    public void StartQuest()
    {
        quest.play();
        SoundManager.Instance.EffectPlay(SoundManager.Instance.card);
    }
}
