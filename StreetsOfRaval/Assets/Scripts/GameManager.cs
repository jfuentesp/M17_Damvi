using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    //Instance of the GameManager. Refers to this own gameobject.
    private static GameManager m_Instance;
    public static GameManager GameManagerInstance => m_Instance; //A getter for the instance of the game manager. Similar to get { return m_Instance }. (Accessor)

    [Header("Game Parameters")]
    [SerializeField]
    private int m_Wave = 0;
    [SerializeField]
    private int m_Score = 0;
    [SerializeField]
    private int m_Lives = 2;
    [SerializeField]
    List<int> m_NumberOfEnemiesByWave;

    private int m_RemainingEnemies;

    public int Wave => m_Wave;
    public int Score => m_Score;
    public int Lives => m_Lives;
    public List<int> NumberOfEnemiesByWave => m_NumberOfEnemiesByWave;
    public int NumberOfEnemies => m_NumberOfEnemiesByWave[m_Wave];

    [Header("GameEvents for the Game Mechanics")]
    [SerializeField]
    GameEventVoid m_OnNextWave;
    [SerializeField]
    GameEventVoid m_OnWaveFinished;

    SpawnerBehaviour m_Spawner;

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
        InitializeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_Spawner.IsSpawning && m_Spawner.EnemiesSpawned == 0)
        {
            StartCoroutine(NextWaveCoroutine());
            m_OnWaveFinished.Raise();
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

    private IEnumerator NextWaveCoroutine()
    {
        yield return new WaitForSeconds(5f);
        AddWave(1);
        m_OnNextWave.Raise();
    }
}
