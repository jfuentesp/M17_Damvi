using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralBehaviour
{
    private ProceduralBehaviour() { }

    //Perlin noise function will return a sample (a float value between 0 and 1)
    public static float CalculatePerlinNoise(float x, float y, float frequency, int width, int height, float offsetX = 0, float offsetY = 0,
            int octaves = 0, float lacunarity = 2, float persistence = 0.5f, bool carveOctaves = true,
            bool verbose = false, bool returnAllValues = false)
    {
        //Finding the length of the step between the distance of each point of reference in the noise map based on frequency (as period)
        float step = frequency / Mathf.Max(width, height);

        /*
        Offset returns the section of the noise map we are going to use as reference. It is multiplied by the step, so the bigger the step, 
        the bigger the area that the offset will take. 
         */
        float xCoord = offsetX + x * step;
        float yCoord = offsetY + y * step;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);

        if (verbose) Debug.Log("Sample is: " + sample);

        for (int octave = 1; octave <= octaves; octave++)
        {

            float newStep = frequency * lacunarity * octave / Mathf.Max(width, height);
            float xOctaveCoord = offsetX + x * newStep;
            float yOctaveCoord = offsetY + y * newStep;

            float octaveSample = Mathf.PerlinNoise(xOctaveCoord, yOctaveCoord);

            //La Persistence afecta a l'amplitud de cada subseqent octava. El limitem a [0.1, 0.9] de forma
            // que cada nou valor afecti menys al resultat final.
            //Si Carve Octaves est actiu ->
            // addicionalment, farem que el soroll en comptes de ser un valor base [0,1] sigui [-0.5f,0.5f]
            // i aix pugui sumar o restar al valor inicial
            octaveSample = (octaveSample - (carveOctaves ? .5f : 0)) * (persistence / octave);

            //acumulaci del soroll amb les octaves i base anteriors
            if (verbose) Debug.Log($"Octave {octave}: [{x},{y}] = {octaveSample}");
            sample += octaveSample;

        }

        if (verbose) Debug.Log("Final sample: " + sample);
        return sample;
    }

}
