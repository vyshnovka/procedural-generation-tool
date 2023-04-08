using System;
using UnityEngine;

public class WorleyNoise : Algorithm
{
    public override float[,] GenerateHeightMap(int size)
    {
        float[,] noise = new float[size, size];

        System.Random random = new();

        int numCells = random.Next(size / 4);
        float hardness = 1f;

        Vector2[] points = new Vector2[numCells];

        for (int i = 0; i < numCells; i++)
        {
            points[i] = new Vector2(random.Next(size), random.Next(size));
        }

        float maxDistance = Mathf.Sqrt(size * size + size * size);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
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
                noise[x, y] = noiseValue * noiseValue * (hardness - 2 * noiseValue);
            }
        }

        return noise;
    }
}
