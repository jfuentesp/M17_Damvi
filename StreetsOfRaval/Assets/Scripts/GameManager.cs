using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        m_Wave = 0;
        m_Score = 0;
        m_Lives = 2;
    }
}
