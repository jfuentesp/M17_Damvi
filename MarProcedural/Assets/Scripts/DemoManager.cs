using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DemoManager : MonoBehaviour
{
    //Components
    private ProceduralBehaviour m_ProceduralBehaviour;

    //Size of the area or texture we will fill with the perlin noise information (eg tilemap)
    [Header("Size parameters")]
    [SerializeField]
    private int m_Width;
    [SerializeField]
    private int m_Height;

    [Header("Tilemap parameters")]
    //References to components
    [SerializeField]
    private Tilemap m_Tilemap;
    //We can set a struct and an array of structs to settle a tile and a range. So, once we have to set the tiles, we can loop and settle a tile or another given by its range.
    //Also, we can set the different perlin noise infos (for each component such as biome, land, details, npcs...) in scriptable objects and set an scriptable object instead of a tile inside the struct.
    [Serializable]
    private struct PerlinTileInfo
    {
        public Tile tile;
        public float range;
    }
    [SerializeField]
    private PerlinTileInfo[] m_TileInfos;

    [Header("Perlin parameters")]
    /*
     Scale => The scale is the amount of "pixels", or grid points, the perlin noise texture has. It can be represented as a Zoom, since, as low as this value is, more smooth and bigger will be. The higher the value is,
               more detailed and shrink will be.
     Amplitude => It determines the height or the intensity of the noise. A high amplitude will result on a noise with higher curves and valleys. A low amplitude will result on a noise more smooth and less prominent. 
     Frequency => It determines how long are the STEPS between units where the noise is evaluated. It's represented as a period. If the frequency is high, the distance on the steps will be shorter, so they represent
                a high frequent changes on the noise (crestas y valles más agresivos). A low frequency, the distance on the steps will be further, so it will be more smooth changes.
     Offset => It allows us to move through the whole perlin noise, giving us a region of the area (imagine it as a texture where you take a small part as reference, given by this values)
            This is also known as the "SEED", since this position can define how the results will be, and the same position always results in the same.
     
     */
    [SerializeField]
    private float m_OffsetX;
    [SerializeField]
    private float m_OffsetY;
    [SerializeField]
    private float m_Frequency;

    [Header("Octave parameters")]
    /*
     Octaves => Octaves are a layer of noise with a specific frequency. Each octave represents a grade of noise. The more octaves you apply over the result, more detailed will be, since you're applying different layers of
            noise, and variations, all subsequentially.
     Lacunarity => Lacunarity is applied in octaves fractality. It determines how much will have impact every subsequent octave in the results. The higher the lacunarity, the details will be more spread and less homogeneus,
            and if its low, they will be more focused in points and more uniforms.
     Persistence =>
     */
    private const int MAX_OCTAVES = 8;
    [SerializeField]
    [Range(0, MAX_OCTAVES)]
    private int m_Octaves = 0;
    [SerializeField]
    [Range(2, 3)]
    private int m_Lacunarity = 2;
    [SerializeField]
    [Range(0.1f, 0.9f)]
    private float m_Persistence = 0.5f;
    [SerializeField]
    private bool m_Carve = true;

    void Start()
    {
        //First, we fill the tilemap with a base tile ?
        GeneratePerlinMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Generate base?
            GenerateBaseMap();
            Debug.Log("Q key was pressed");
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

    private void GeneratePerlinMap()
    {
        for(int row = 0; row < m_Height; row++)
        {
            for(int col = 0; col < m_Width; col++)
            {
                float PerlinNoise = ProceduralBehaviour.CalculatePerlinNoise(col, row, m_Frequency, m_Width, m_Height, m_OffsetX, m_OffsetY, m_Octaves, m_Lacunarity, m_Persistence, m_Carve, false, false);
                //Acción para comprobar posiciones y ver qué se debería pintar en cada casilla
            }
        }
    }

    private void GenerateBaseMap()
    {

    }
}
