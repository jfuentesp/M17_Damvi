using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BaseMapBiomesController : MonoBehaviour
{
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

    private bool m_IsBiomeMapSaved;
    public bool IsBiomeMapSaved => m_IsBiomeMapSaved;

    private float[,] m_BiomesPerlinNoise;

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

    //We set a new texture that will create on the double for loop, pixel by pixel, based on every point of the perlin noise, setting a color based on a Gradient
    [Header("Texture parameters")]
    private Texture2D m_Texture;
    private Sprite m_FinalTexture;
    [SerializeField]
    private Gradient m_Gradient;

    // Start is called before the first frame update
    void Start()
    {
        m_IsBiomeMapSaved = false;
    }

    public void GeneratePerlinMap(Tilemap tilemap, int width, int height, Image GUIimage)
    {
        //Creating the texture2D
        m_Texture = new Texture2D(width, height);
        m_Texture.filterMode = FilterMode.Point; //This filtermode makes the result set pixel by pixel, less blurry

        m_BiomesPerlinNoise = new float[width, height];

        Debug.Log(string.Format("Width received: {0} || Height received: {1}", width, height));

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                float PerlinNoise = ProceduralBehaviour.CalculatePerlinNoise(col, row, m_Frequency, width, height, m_OffsetX, m_OffsetY, m_Octaves, m_Lacunarity, m_Persistence, m_Carve);
                m_BiomesPerlinNoise[col, row] = PerlinNoise;

                //We extract the color given by a gradient that we previously have set, giving the perlin noise value as prompt for that position
                Color color = m_Gradient.Evaluate(PerlinNoise); 
                m_Texture.SetPixel(col, row, color); // Then set that color to the pixel
            }
        }

        m_Texture.Apply(); //Need to apply the changes to the texture before setting it
        m_FinalTexture = Sprite.Create(m_Texture, new Rect(0, 0, (float)width, (float)height), Vector2.zero); //We create a new sprite based on the Texture2D
        GUIimage.sprite = m_FinalTexture; //We set the sprite as the image on the GUI

        m_IsBiomeMapSaved = true;
    }

    public BiomeEnum CheckBiomePosition(int x, int y)
    {
        float biome = m_BiomesPerlinNoise[x, y];
        switch(biome)
        {
            case > 0.8f:
                return BiomeEnum.SNOW;
            case > 0.4f:
                return BiomeEnum.WARM;
            default:
                return BiomeEnum.DESERT;
        }
    }
}
