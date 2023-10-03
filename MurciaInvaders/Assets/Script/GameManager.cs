using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace murciainvaders
{
    public class GameManager : MonoBehaviour
    {
        //Instance of the GameManager.
        private static GameManager m_Instance;
        public static GameManager Instance => m_Instance; //A getter for the instance of the game manager. Similar to get { return m_Instance }. (Accessor)

        //Constant variables with the name of the scenes. Constant are variables on whose value cannot be modified, like a read-only value.
        public const string MainMenuScene = "EscenaTitol";
        public const string GameScene = "EscenaJoc";

        //Variable for the actual game score
        private int m_CurrentScore = 0;
        //Variable for the Top Score
        private int m_TopScore = 0;
        //Variable for the name input
        private string m_NameInput;
        //Getter and setter for the PlayerName function
        public string m_PlayerName
        {
            get
            {
                return m_PlayerName;
            }
            set
            {
                m_PlayerName = m_NameInput;
            }
        }
      
        
        
        public int TopScore //Getter and setter for the TopScore function.
        {
            get
            {
                return m_TopScore;
            }
            set
            {
                if(m_CurrentScore > m_TopScore)
                {
                    m_CurrentScore = m_TopScore;
                }
            }
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

        private void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}

