using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace streetsofraval
{
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
        private SpawnerBehaviour m_Spawner;

        // Start is called before the first frame update
        void Start()
        {
            m_GameManager = GameManager.GameManagerInstance;
            m_Player = PlayerBehaviour.PlayerInstance;
            m_Spawner = SpawnerBehaviour.SpawnerInstance;
            UpdateScoreGUI(0);
            UpdateWaveGUI(0);
            UpdateLivesGUI(2);
            UpdateMonstersGUI(1,12);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateGUI()
        {
            m_EnemiesText.text = m_Spawner.EnemiesSpawned + "/" + m_Spawner.EnemiesToSpawn;
            m_WaveText.text = "Wave: " + m_GameManager.Wave;
            m_ScoreText.text = "Score: " + m_GameManager.Score;
            m_Lives.text = "Player lives: " + m_GameManager.Lives;
            m_HPBar.fillAmount = m_Player.Hitpoints / m_Player.MaxHitpoints;
            m_EnergyBar.fillAmount = m_Player.Energy / m_Player.MaxEnergy;
        }

        public void UpdateMonstersGUI(int spawneds, int tospawn)
        {
            m_EnemiesText.text = spawneds + "/" + tospawn;
        }

        public void UpdateWaveGUI(int wave)
        {
            m_WaveText.text = "Wave: " + wave;
        }

        public void LoadingWaveGUI()
        {
            m_WaveText.text = "Awaiting for next wave...";
        }

        public void UpdateScoreGUI(int score)
        {
            m_ScoreText.text = "Score: " + m_GameManager.Score;
        }

        public void UpdateLivesGUI(int lives)
        {
            m_Lives.text = "Player lives: " + lives;
        }

        public void UpdatePlayerHealthbarGUI()
        {
            m_HPBar.fillAmount = m_Player.Hitpoints / m_Player.MaxHitpoints;
        }

        public void UpdatePlayerEnergyBarGUI()
        {
            m_EnergyBar.fillAmount = m_Player.Energy / m_Player.MaxEnergy;
        }
    }

}
