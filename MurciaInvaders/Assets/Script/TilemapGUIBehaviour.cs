using System.Collections;
using System.Collections.Generic;
using murciainvaders;
using TMPro;
using UnityEngine;

public class TilemapGUIBehaviour : MonoBehaviour
{
    //Instance of the First Game Scene GUI. Refers to this own gameobject.
    private static TilemapGUIBehaviour m_Instance;
    public static TilemapGUIBehaviour TilemapGUIInstance => m_Instance; //A getter for the instance of the First Game Scene GUI. Similar to get { return m_Instance }. (Accessor)

    //GUI Elements
    [Header("Elements shown and handled to be modified in the GUI")]
    [SerializeField]
    private TextMeshProUGUI m_Timeleft;

    private GameManager m_GameManager;


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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetTimeText()
    {
        m_Timeleft.text = "Time left: " + m_GameManager.TimeLeft;  
    }

}
