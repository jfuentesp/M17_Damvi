using System.Collections;
using System.Collections.Generic;
using streetsofraval;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    //Instance of the GameManager. Refers to this own gameobject.
    private static GameManager m_Instance;
    public static GameManager GameManagerInstance => m_Instance; //A getter for the instance of the game manager. Similar to get { return m_Instance }. (Accessor)

    private const string m_MainTitleScene = "MainTitleScene";
    private const string m_GameScene = "GameScene";
    private const string m_GameOverScene = "GameOverScene";

    [Header("Game Parameters")]
    [SerializeField]
    private int m_Wave = 0;
    [SerializeField]
    private int m_Score = 0;
    [SerializeField]
    private int m_Lives = 2;
    [SerializeField]
    List<int> m_NumberOfEnemiesByWave;
    private int m_EnemiesSpawned;

    private int m_RemainingEnemies;

    public int Wave => m_Wave;
    public int Score => m_Score;
    public int Lives => m_Lives;
    public List<int> NumberOfEnemiesByWave => m_NumberOfEnemiesByWave;
    public int NumberOfEnemies => m_EnemiesSpawned;

    [Header("GameEvents for the Game Mechanics")]
    [SerializeField]
    GameEventVoid m_OnNextWave;
    [SerializeField]
    GameEventVoid m_OnWaveFinished;

    Vector3 m_PlayerSpawnPoint;

    SpawnerBehaviour m_Spawner;
    PlayerBehaviour m_Player;

    private void Awake()
    {
        //First, we initialize an instance of GameManager. If there is already an instance, it destroys the element and returns.
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Spawner = SpawnerBehaviour.SpawnerInstance;
        m_Player = PlayerBehaviour.PlayerInstance;
        m_PlayerSpawnPoint = m_Player.transform.position;
        InitializeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Spawner.IsSpawning && m_Spawner.EnemiesSpawned == 0)
        {
            m_OnWaveFinished.Raise();    //!!!!!!!!!!!!!!    
        }
    }

    public void InitializeGame()
    {
        AddScore(0);
        AddWave(0);
        AddLives(0);
    }

    public void OnPlayerDeath()
    {
        SubstractLives(1);
        if(m_Lives != 0) 
        {
            StartCoroutine(PlayerDeathCoroutine());
        } else
        {
            SceneManager.LoadScene("GameOverScene");
        }     
    }

    public void AddScore(int score)
    {
        m_Score += score;
    }

    private void AddWave(int wave)
    {
        m_Wave += wave;
    }

    private void AddLives(int lives)
    {
        m_Lives += lives;
    }

    private void SubstractLives(int lives)
    {
        m_Lives -= lives;
    }

    public void OnEnemySpawn()
    {
        //m_EnemiesSpawned = m_Spawner.EnemiesSpawned;
    }

    private IEnumerator NextWaveCoroutine()
    {
        yield return new WaitForSeconds(5f);
        AddWave(1);
        m_OnNextWave.Raise();
    }

    private IEnumerator PlayerDeathCoroutine()
    {
        m_Player.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        m_Player.transform.position = m_PlayerSpawnPoint;
        m_Player.gameObject.SetActive(true);
    }
}
