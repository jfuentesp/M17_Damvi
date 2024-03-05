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
    private TerrainController m_Terrain;
    private WaterController m_Water;
    private DetailsController m_Details;

    private bool m_IsBiomeMapGenerated;
    private bool m_IsTerrainGenerated;

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
    [SerializeField]
    private Image m_MapImage;
    [SerializeField]
    private Image m_BiomeImage;

    [Header("Tilemaps")]
    [SerializeField]
    private Tilemap m_WaterTilemap;
    [SerializeField]
    private Tilemap m_TerrainTilemap;
    [SerializeField]
    private Tilemap m_DetailTilemap;


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

        m_IsBiomeMapGenerated = false;
        m_IsTerrainGenerated = false;

        m_BaseMap = GetComponent<BaseMapBiomesController>();
        m_Terrain = GetComponent<TerrainController>();
        m_Details = GetComponent<DetailsController>();
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
            //Generate base biome map
            m_BaseMap.GeneratePerlinMap(m_WaterTilemap, m_Width, m_Height, m_BiomeImage);
            m_IsBiomeMapGenerated = true;
            Debug.Log("Q key was pressed. Generating base biome map. Base biome map has been saved.");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //Generate terrain
            if(m_IsBiomeMapGenerated)
            {
                m_Terrain.GeneratePerlinMap(m_TerrainTilemap, m_Width, m_Height, m_PerlinImage, m_MapImage, m_WaterTilemap);
                m_IsTerrainGenerated = true;
                Debug.Log("W key was pressed. Generating terrain map. This sets an specific tile based on the biome map.");
            } else
            {
                Debug.Log("There is no biome map generated. First, generate a biome map pressing Q.");
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Generate details
            if (m_IsTerrainGenerated)
            {
                m_Details.GeneratePerlinMap(m_DetailTilemap, m_Width, m_Height, m_PerlinImage, m_MapImage, m_TerrainTilemap);
                Debug.Log("E key was pressed. Generating details map. This sets an specific tile based on the biome map.");
            }
            else
            {
                Debug.Log("There is no terrain generated. First, generate a terrain map pressing W.");
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            //Hide show help?
            Debug.Log("Hide/Show help");
        }
    }

}
