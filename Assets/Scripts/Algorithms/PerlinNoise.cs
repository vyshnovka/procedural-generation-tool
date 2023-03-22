using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PerlinNoise : Noise
{
    public override float[,] GenerateHeightMap(int width, int height)
    {
        float[,] noiseMap = new float[width, height];
        float maxPossibleHeight = 0f;

        int octaves = 6;
        float persistence = 1f;
        float amplitude = 5f;
        float frequency = 1f;

        for (int i = 0; i < octaves; i++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float sampleX = (float)x / width * frequency;
                    float sampleY = (float)y / height * frequency;

                    int xi0 = (int)Math.Floor(sampleX) % 256;
                    int yi0 = (int)Math.Floor(sampleY) % 256;
                    int xi1 = (xi0 + 1) % 256;
                    int yi1 = (yi0 + 1) % 256;

                    float tx = sampleX - xi0;
                    float ty = sampleY - yi0;

                    int aa = PermutationTable[PermutationTable[xi0] + yi0];
                    int ab = PermutationTable[PermutationTable[xi0] + yi1];
                    int ba = PermutationTable[PermutationTable[xi1] + yi0];
                    int bb = PermutationTable[PermutationTable[xi1] + yi1];

                    float gradientX1 = Grad(aa, tx, ty);
                    float gradientX2 = Grad(ba, tx - 1f, ty);
                    float gradientX3 = Grad(ab, tx, ty - 1f);
                    float gradientX4 = Grad(bb, tx - 1f, ty - 1f);

                    float u = Smooth(tx);
                    float v = Smooth(ty);

                    float i1 = Lerp(gradientX1, gradientX2, u);
                    float i2 = Lerp(gradientX3, gradientX4, u);
                    float final = Lerp(i1, i2, v);

                    noiseMap[x, y] += final * amplitude;
                }
            }

            maxPossibleHeight += amplitude;
            amplitude *= persistence;
            frequency *= 2f;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                noiseMap[x, y] /= maxPossibleHeight;
            }
        }

        return noiseMap;
    }

    private float Grad(int hash, float x, float y)
    {
        switch (hash & 3)
        {
            case 0:
                return x + y;
            case 1:
                return -x + y;
            case 2:
                return x - y;
            case 3:
                return -x - y;
            default:
                return 0;
        }
    }

    private float Smooth(float t)
    {
        return t * t * t * (t * (t * 6f - 15f) + 10f);
    }

    private float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }
}
