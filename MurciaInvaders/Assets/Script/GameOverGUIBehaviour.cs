using murciainvaders;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverGUIBehaviour : MonoBehaviour
{
    //Instance of the Game Over GUI. Refers to this own gameobject.
    private static GameOverGUIBehaviour m_Instance;
    public static GameOverGUIBehaviour MainTitleGUIInstance => m_Instance; //A getter for the instance of the game over GUI. Similar to get { return m_Instance }. (Accessor)

    //GUI Elements
    [Header("Elements shown and handled to be modified in the GUI")]
    [SerializeField]
    private Button m_RestartButton;
    [SerializeField]
    private Button m_ExitButton;

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

    // Start is called before the first frame update
    void Start()
    {
        m_GameManager = GameManager.GameManagerInstance;
        m_RestartButton.onClick.AddListener(RestartGame);
        m_ExitButton.onClick.AddListener(ExitMainMenu);
    }

    private void RestartGame()
    {
        m_GameManager.StartGame();
    }

    private void ExitMainMenu()
    {
        m_GameManager.OnExitToMainMenu();
    }
}
