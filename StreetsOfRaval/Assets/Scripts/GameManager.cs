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
    private int m_Wave;
    [SerializeField]
    private int m_Score;
    [SerializeField]
    private int m_Lives;

    public int Wave => m_Wave;
    public int Score => m_Score;
    public int Lives => m_Lives;

    // Start is called before the first frame update
    void Start()
    {
        InitializeGame();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeGame()
    {
        SetScoreAndWave(0, 0);
        SetLives(0);
    }

    private void SetScoreAndWave(int score, int wave)
    {
        m_Score = score;
        m_Wave = wave;
    }

    private void SetLives(int lives)
    {
        m_Lives = lives;
    }
}
