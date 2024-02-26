using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class DemoManager : MonoBehaviour
{
    //DemoManager will be an instance. Only one will be executed during the runtime
    public static DemoManager instance;

    //Components
    private BaseMapBiomesController m_BaseMap;

    //Size of the area or texture we will fill with the perlin noise information (eg tilemap)
    [Header("Size parameters")]
    [SerializeField]
    private int m_Width;
    public int Width => m_Width;
    [SerializeField]
    private int m_Height;
    public int Height => m_Height;

    //We set a reference to the user interface and an Image to set a new texture that simulates the perlin noise
    [Header("User Interface")]
    [SerializeField]
    private GameObject m_GUI;
    [SerializeField]
    private Image m_PerlinImage;

    [Header("Tilemap")]
    [SerializeField]
    private Tilemap m_Tilemap;
    

    private void Awake()
    {
        //In order to make it a singleton, if it does execute and there's an existing DemoManager, it will destroy this instance and won't let it continue.
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }

        m_BaseMap = GetComponent<BaseMapBiomesController>();
    }

    void Start()
    {
        //First, we fill the tilemap with a base tile ?
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Generate base?
            m_BaseMap.GeneratePerlinMap(m_Tilemap, m_Width, m_Height, m_PerlinImage);
            Debug.Log("Q key was pressed. Generating base map.");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //Generate base?
            Debug.Log("W key was pressed");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Generate base?
            Debug.Log("E key was pressed");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Generate base?
            Debug.Log("R key was pressed");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            //Generate base?
            Debug.Log("T key was pressed");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            //Generate base?
            Debug.Log("Y key was pressed");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            //Hide show help?
            Debug.Log("Hide/Show help");
        }
    }

}
