using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WorleyNoise : Noise
{
    // Working code, but it generates the noise that is too smooth.
    //public override float[,] GenerateHeightMap(int size)
    //{
    //    int[] permutationTable = PermutationTable(size * 2);
    //    float[,] noise = new float[size, size];

    //    int numPoints = 10;
    //    float scale = 2f;
    //    System.Random prng = new System.Random();

    //    Vector2[] points = new Vector2[numPoints];
    //    for (int i = 0; i < numPoints; i++)
    //    {
    //        float x = Mathf.Round((float)prng.NextDouble() * size);
    //        float y = Mathf.Round((float)prng.NextDouble() * size);
    //        points[i] = new Vector2(x, y);
    //    }

    //    for (int y = 0; y < size; y++)
    //    {
    //        for (int x = 0; x < size; x++)
    //        {
    //            float closestDistance = float.MaxValue;
    //            float secondClosestDistance = float.MaxValue;

    //            for (int i = 0; i < numPoints; i++)
    //            {
    //                float distance = Mathf.Sqrt(Mathf.Pow(points[i].x - x, 2) + Mathf.Pow(points[i].y - y, 2));

    //                if (distance < closestDistance)
    //                {
    //                    secondClosestDistance = closestDistance;
    //                    closestDistance = distance;
    //                }
    //                else if (distance < secondClosestDistance)
    //                {
    //                    secondClosestDistance = distance;
    //                }
    //            }

    //            float value = Mathf.Abs(closestDistance - secondClosestDistance);
    //            noise[x, y] = value / scale;
    //        }
    //    }

    //    for (int y = 0; y < size; y++)
    //    {
    //        for (int x = 0; x < size; x++)
    //        {
    //            float noiseValue = 0f;
    //            float normalizationFactor = 0f;

    //            for (int i = 0; i < numPoints; i++)
    //            {
    //                float distance = Mathf.Sqrt(Mathf.Pow(points[i].x - x, 2) + Mathf.Pow(points[i].y - y, 2));
    //                float weight = 1f / Mathf.Pow(distance + 0.001f, 2);
    //                int index = permutationTable[(int)(Mathf.Floor(points[i].x) + permutationTable[(int)Mathf.Floor(points[i].y) % permutationTable.Length]) % permutationTable.Length];
    //                noiseValue += weight * ((float)index / permutationTable.Length);
    //                normalizationFactor += weight;
    //            }

    //            noise[x, y] = noiseValue / normalizationFactor;
    //        }
    //    }

    //    return noise;
    //}

    // Also works, but it looks too sharp.
    //public override float[,] GenerateHeightMap(int size)
    //{
    //    int[] permutationTable = PermutationTable(size * 2);
    //    float[,] noise = new float[size, size];

    //    int numCells = 10;
    //    float frequency = 2f;
    //    System.Random prng = new System.Random();

    //    Vector2[] points = new Vector2[numCells];
    //    for (int i = 0; i < numCells; i++)
    //    {
    //        points[i] = new Vector2(prng.Next(0, size), prng.Next(0, size));
    //    }

    //    float maxDistance = Mathf.Sqrt(size * size + size * size);
    //    float inverseFrequency = 1f / frequency;

    //    for (int x = 0; x < size; x++)
    //    {
    //        for (int y = 0; y < size; y++)
    //        {
    //            float closestDistance = maxDistance;
    //            int closestIndex = -1;

    //            for (int i = 0; i < numCells; i++)
    //            {
    //                float distance = Vector2.Distance(points[i], new Vector2(x, y));
    //                if (distance < closestDistance)
    //                {
    //                    closestDistance = distance;
    //                    closestIndex = i;
    //                }
    //            }

    //            float distanceToSecondClosest = maxDistance;
    //            for (int i = 0; i < numCells; i++)
    //            {
    //                if (i == closestIndex) continue;

    //                float distance = Vector2.Distance(points[i], new Vector2(x, y));
    //                if (distance < distanceToSecondClosest)
    //                {
    //                    distanceToSecondClosest = distance;
    //                }
    //            }

    //            float falloff = Mathf.InverseLerp(0f, maxDistance, closestDistance);
    //            float edgeFalloff = Mathf.InverseLerp(0f, maxDistance, distanceToSecondClosest);
    //            float value = (closestIndex + falloff * edgeFalloff) * inverseFrequency;

    //            noise[x, y] = value;
    //        }
    //    }

    //    return noise;
    //}

    public override float[,] GenerateHeightMap(int size)
    {
        //TODO: make it use permutation table.
        int[] permutationTable = PermutationTable(size * 2);
        float[,] noise = new float[size, size];

        //TODO: maybe make this a random.
        int numCells = 10;
        System.Random random = new System.Random();

        Vector2[] points = new Vector2[numCells];
        for (int i = 0; i < numCells; i++)
        {
            points[i] = new Vector2(random.Next(size), random.Next(size));
        }

        float maxDistance = Mathf.Sqrt(size * size + size * size);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float[] distances = new float[numCells];
                float closestDistance = maxDistance;

                for (int i = 0; i < numCells; i++)
                {
                    distances[i] = Vector2.Distance(new Vector2(x, y), points[i]);
                    if (distances[i] < closestDistance)
                    {
                        closestDistance = distances[i];
                    }
                }

                float noiseValue = closestDistance / maxDistance;
                noise[x, y] = noiseValue * noiseValue * (3 - 2 * noiseValue);
            }
        }

        return noise;
    }
}
