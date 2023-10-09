using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace murciainvaders
{
    public class GameManager : MonoBehaviour
    {
        //Instance of the GameManager. Refers to this own gameobject.
        private static GameManager m_Instance;
        public static GameManager GameManagerInstance => m_Instance; //A getter for the instance of the game manager. Similar to get { return m_Instance }. (Accessor)

        //Constant variables with the name of the scenes. Constant are variables on whose value cannot be modified, like a read-only value.
        public const string m_MainMenuScene = "EscenaTitol";
        public const string m_BulletGameScene = "EscenaJoc";
        public const string m_MurciaScene = "EscenaJoc2";
        public const string m_GameOver = "EscenaGameOver";

        //Variable for the actual game score
        private int m_CurrentScore = 0;
        //Getter and setter for the CurrentScore variable
        public int CurrentScore
        {
            get { return m_CurrentScore; }
            set { m_CurrentScore = value; }
        }

        //Variable for the Top Score
        private int m_TopScore = 0;
        //Getter and setter for the TopScore variable 
        public int TopScore
        {
            get { return m_TopScore; }
            set { m_TopScore = value; }
        }

        //Variable for the name input
        private string m_NameInput;
        //Getter and setter for the PlayerName variable
        public string PlayerName
        {
            get { return m_NameInput; }
            set { m_NameInput = value; }
        }

        //Variable for the Top Score Player
        private string m_TopScorePlayer;
        //Getter and setter for the Top Score Player variable
        public string TopScorePlayer
        {
            get { return m_TopScorePlayer; }
            set { m_TopScorePlayer = value;}
        }

        private void Awake()
        {
            //First, we initialize an instance of GameManager. If there is already an instance, it destroys the element and returns.
            if(m_Instance == null)
            {
                m_Instance = this;
            } else
            {
                Destroy(this.gameObject);
                return;
            }

            m_TopScorePlayer = "Player";
            m_TopScore = 100;

            DontDestroyOnLoad(this.gameObject);

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        //Simple script to load a scene set by string. 
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void QuitGame()
        {
            //A standard way to close the application. Will lose all data.
            Application.Quit();
        }

        public void StartGame()
        {
            //We set the name set on the input field as new player and reset the score to 0 when we start the game.
            m_NameInput = MainTitleGUIBehaviour.MainTitleGUIInstance.GetPlayerName();
            m_CurrentScore = 0;
            LoadScene(m_BulletGameScene);
        }

        //Shooting scene
        private void OnShootingSceneStart()
        {

        }

        //Tilemap scene
        private void OnLandingSceneStart()
        {

        }

        public void OnGameOver()
        {
            LoadScene(m_GameOver);
        }

        private void OnVictory()
        {

        }

        public void OnExitToMainMenu()
        {
            LoadScene(m_MainMenuScene);
        }

        public void UpdateCurrentScoreValue(int score)
        {
            m_CurrentScore += score;
        }
    }
}

