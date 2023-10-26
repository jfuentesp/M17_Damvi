using System.Collections;
using System.Collections.Generic;
using streetsofraval;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIBehaviour : MonoBehaviour
{
    [Header("GUI References")]
    [SerializeField]
    private TextMeshProUGUI m_EnemiesText;
    [SerializeField]
    private TextMeshProUGUI m_ScoreText;
    [SerializeField]
    private TextMeshProUGUI m_WaveText;
    [SerializeField]
    private TextMeshProUGUI m_Lives;
    [SerializeField]
    private Image m_HPBar;
    [SerializeField]
    private Image m_EnergyBar;

    private GameManager m_GameManager;
    private PlayerBehaviour m_Player;

    // Start is called before the first frame update
    void Start()
    {
        m_GameManager = GameManager.GameManagerInstance;
        m_Player = PlayerBehaviour.PlayerInstance;
        UpdateScoreGUI(0);
        UpdateWaveGUI(0);
        UpdateLivesGUI(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateWaveGUI(int wave)
    {
        m_WaveText.text = "Wave: " + m_GameManager.Wave;
    }

    public void UpdateScoreGUI(int score)
    {
        m_ScoreText.text = "Score: " + m_GameManager.Score;
    }

    public void UpdateLivesGUI(int lives)
    {
        m_Lives.text = "Player lives: " + m_GameManager.Lives;
    }

    public void UpdatePlayerHealthbarGUI(int hp)
    {
        m_HPBar.fillAmount = m_Player.Hitpoints / m_Player.MaxHitpoints;
    }

    public void UpdatePlayerEnergyBarGUI(int energy)
    {
        m_EnergyBar.fillAmount = m_Player.Energy / m_Player.MaxEnergy;
    }
}
