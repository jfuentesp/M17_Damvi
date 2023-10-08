using System.Collections;
using System.Collections.Generic;
using murciainvaders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletSceneGUIBehaviour : MonoBehaviour
{
    //Instance of the First Game Scene GUI. Refers to this own gameobject.
    private static BulletSceneGUIBehaviour m_Instance;
    public static BulletSceneGUIBehaviour MainTitleGUIInstance => m_Instance; //A getter for the instance of the First Game Scene GUI. Similar to get { return m_Instance }. (Accessor)

    //GUI Elements
    [Header("Elements shown and handled to be modified in the GUI")]
    [SerializeField]
    private TextMeshProUGUI m_PlayerName;
    [SerializeField]
    private TextMeshProUGUI m_Score;
    [SerializeField]
    private Image m_PlayerHealthBar;
    [SerializeField]
    private Image m_BossHealthBar;

    private GameManager m_GameManager;
    private PlayerBehaviour m_Player;
    private BossBehaviour m_Boss;

    private void Awake()
    {
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
        m_Player = PlayerBehaviour.PlayerInstance;
        m_Boss = BossBehaviour.BossInstance;
        //Once everything is set on Awake for every script, we set the name and the score.
        GetScore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GetScore()
    {

        m_PlayerName.text = "Player: " + m_GameManager.PlayerName;
        m_Score.text = "Score: " + m_GameManager.CurrentScore;
    }

    public void UpdateScoreGUI(int score)
    {
        m_Score.text = "Score: " + m_GameManager.CurrentScore;
    }

    public void UpdatePlayerHealthBar(int hp)
    {
        m_PlayerHealthBar.fillAmount = (float)m_Player.CurrentPlayerHP / m_Player.MaxPlayerHP;
    }

    public void UpdateBossHealthBar(int hp)
    {
        m_BossHealthBar.fillAmount = (float)m_Boss.BossCurrentHP / m_Boss.BossMaxHP;
    }

}
