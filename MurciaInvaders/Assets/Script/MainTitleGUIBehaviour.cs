using System.Collections;
using System.Collections.Generic;
using murciainvaders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainTitleGUIBehaviour : MonoBehaviour
{
    //Instance of the Main Title GUI. Refers to this own gameobject.
    private static MainTitleGUIBehaviour m_Instance;
    public static MainTitleGUIBehaviour MainTitleGUIInstance => m_Instance; //A getter for the instance of the main title GUI. Similar to get { return m_Instance }. (Accessor)

    //GUI Elements
    [Header("Elements shown and handled to be modified in the GUI")]
    [SerializeField]
    private TextMeshProUGUI m_InputFieldName;
    [SerializeField]
    private TextMeshProUGUI m_TopPlayerName;
    [SerializeField]
    private TextMeshProUGUI m_TopPlayerScore;

    private GameManager m_GameManager;
    

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

        
    }

    private void Start()
    {
        m_GameManager = GameManager.GameManagerInstance;

        SetTopScore();
    }

    public void SetTopScore()
    {
        m_TopPlayerName.text = "Player: " + m_GameManager.TopScorePlayer;
        m_TopPlayerScore.text = "Score: " + m_GameManager.TopScore;
    }

    public string GetPlayerName()
    {
        string PlayerName = m_InputFieldName.text;
        return PlayerName;
    }
}
