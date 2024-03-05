using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DetailsController : MonoBehaviour
{
    //We can set a struct and an array of structs to settle a tile and a range. So, once we have to set the tiles, we can loop and settle a tile or another given by its range.
    //Also, we can set the different perlin noise infos (for each component such as biome, land, details, npcs...) in scriptable objects and set an scriptable object instead of a tile inside the struct.
    [Serializable]
    private struct PerlinTileInfo
    {
        public TileBase tile;
        public float range;
        public BiomeEnum biome;
    }
    [SerializeField]
    private PerlinTileInfo[] m_WarmTileInfos;
    [SerializeField]
    private PerlinTileInfo[] m_SnowTileInfos;
    [SerializeField]
    private PerlinTileInfo[] m_DesertTileInfos;
    [SerializeField]
    private PerlinTileInfo[] m_WaterTileInfos;


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
    private Texture2D m_NoiseTexture;
    private Texture2D m_FriendlyTexture;
    private Sprite m_FinalNoiseTexture;
    private Sprite m_FinalFriendlyTexture;
    [SerializeField]
    private Gradient m_NoiseGradient;
    [SerializeField]
    private Gradient m_FriendlyGradient;
    private BaseMapBiomesController m_BiomesMap;

    private void Awake()
    {
        m_BiomesMap = GetComponent<BaseMapBiomesController>();
    }

    public void GeneratePerlinMap(Tilemap tilemap, int width, int height, Image GUINoiseimage, Image GUIFriendlyImage, Tilemap terraintilemap)
    {
        //Creating the texture2D for the noise
        m_NoiseTexture = new Texture2D(width, height);
        m_NoiseTexture.filterMode = FilterMode.Point; //This filtermode makes the result set pixel by pixel, less blurry

        //Creating a representative texture2D for human eye
        m_FriendlyTexture = new Texture2D(width, height);
        m_NoiseTexture.filterMode = FilterMode.Point;

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                float PerlinNoise = ProceduralBehaviour.CalculatePerlinNoise(col, row, m_Frequency, width, height, m_OffsetX, m_OffsetY, m_Octaves, m_Lacunarity, m_Persistence, m_Carve);
                
                //We extract the color given by a gradient that we previously have set, giving the perlin noise value as prompt for that position
                Color noisecolor = m_NoiseGradient.Evaluate(PerlinNoise); 
                m_NoiseTexture.SetPixel(col, row, noisecolor); // Then set that color to the pixel
                Color friendlycolor = m_FriendlyGradient.Evaluate(PerlinNoise);
                m_FriendlyTexture.SetPixel(col, row, friendlycolor);

                EvaluateAndSetTile(row, col, PerlinNoise, tilemap, terraintilemap);             
            }
        }
        m_NoiseTexture.Apply(); //Need to apply the changes to the texture before setting it
        m_FriendlyTexture.Apply();
        
        m_FinalFriendlyTexture = Sprite.Create(m_FriendlyTexture, new Rect(0, 0, (float)width, (float)height), Vector2.zero);
        m_FinalNoiseTexture = Sprite.Create(m_NoiseTexture, new Rect(0, 0, (float)width, (float)height), Vector2.zero); //We create a new sprite based on the Texture2D
        GUINoiseimage.sprite = m_FinalNoiseTexture; //We set the sprite as the image on the GUI
        GUIFriendlyImage.sprite = m_FinalFriendlyTexture;
    }

    public void EvaluateAndSetTile(int row, int col, float sample, Tilemap tilemap, Tilemap terraintilemap)
    {

        //First we check in the biome perlin what kind of biome is it for the current position
        BiomeEnum biome = m_BiomesMap.CheckBiomePosition(col, row);
        Vector3Int positionToCheck = new Vector3Int(col, row, 0);
        float probability1 = Random.Range(0, 1f);
        TileBase detail = null;


        if (terraintilemap.GetTile(positionToCheck) != null)
        {
            TileBase TileBase = terraintilemap.GetTile(positionToCheck); // find the tile
            string name = TileBase.name; // take the name of the tile
            Debug.Log(name);
            switch (sample)
            {
                case >0.99f:
                    TileBase castle = m_WarmTileInfos.Where(tileinfo => tileinfo.tile.name.Equals("castillo")).Select(tileinfo => tileinfo.tile).First();
                    if (probability1 > 0.6f && name.Equals("Warm"))
                        tilemap.SetTile(new Vector3Int(col, row), castle);
                    if (name.Equals("Warm"))
                        Debug.Log("Entro");
                    break;
                case >= 0.9f:
                    if (name.Equals("Warm"))
                        detail = m_WarmTileInfos.Where(tileinfo => tileinfo.range == 0.9f).Select(tileinfo => tileinfo.tile).First();
                    if (name.Equals("Snow"))
                        detail = m_SnowTileInfos.Where(tileinfo => tileinfo.range == 0.9f).Select(tileinfo => tileinfo.tile).First();
                    if (name.Equals("Desert"))
                        detail = m_DesertTileInfos.Where(tileinfo => tileinfo.range == 0.9f).Select(tileinfo => tileinfo.tile).First();
                    if (probability1 > 0.5f)
                        tilemap.SetTile(new Vector3Int(col, row), detail);
                    break;
                case >= 0.8f:
                    if (name.Equals("Warm"))
                        detail = m_WarmTileInfos.Where(tileinfo => tileinfo.range == 0.8f).Select(tileinfo => tileinfo.tile).First();
                    if (name.Equals("Snow"))
                        detail = m_SnowTileInfos.Where(tileinfo => tileinfo.range == 0.8f).Select(tileinfo => tileinfo.tile).First();
                    if (probability1 > 0.5f)
                        tilemap.SetTile(new Vector3Int(col, row), detail);
                    break;
                case >= 0.7f:
                    if (name.Equals("Warm"))
                        detail = m_WarmTileInfos.Where(tileinfo => tileinfo.range == 0.7f).Select(tileinfo => tileinfo.tile).First();
                    if (name.Equals("Desert"))
                        detail = m_DesertTileInfos.Where(tileinfo => tileinfo.range == 0.7f).Select(tileinfo => tileinfo.tile).First();
                    if (probability1 > 0.5f)
                        tilemap.SetTile(new Vector3Int(col, row), detail);
                    break;
                default:
                    break;
            }
        }
    }
}
