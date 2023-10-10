using murciainvaders;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryMenuGUIBehaviour : MonoBehaviour
{
    //Instance of the Victory GUI. Refers to this own gameobject.
    private static VictoryMenuGUIBehaviour m_Instance;
    public static VictoryMenuGUIBehaviour MainTitleGUIInstance => m_Instance; //A getter for the instance of the victory GUI. Similar to get { return m_Instance }. (Accessor)

    //GUI Elements
    [Header("Elements shown and handled to be modified in the GUI")]
    [SerializeField]
    private Button m_ExitButton;
    [SerializeField]
    private TextMeshProUGUI m_TopPlayerName;
    [SerializeField]
    private TextMeshProUGUI m_TopScore;
    [SerializeField]
    GameObject m_HighScoreLayout;

    private GameManager m_GameManager;



    // Start is called before the first frame update
    void Start()
    {
        m_GameManager = GameManager.GameManagerInstance;
        if (m_GameManager.IsNewHighscore)
        {
            m_HighScoreLayout.SetActive(true);
            m_TopPlayerName.text = "Player => " + m_GameManager.PlayerName;
            m_TopScore.text = "New highscore =>" + m_GameManager.CurrentScore;
        } else
        {
            m_HighScoreLayout.SetActive(false);
        }

        m_ExitButton.onClick.AddListener(ToMainMenu);
        
    }

    private void ToMainMenu()
    {
        m_GameManager.OnExitToMainMenu();
    }



}
