using System;

namespace TerrainGeneration.Algorithms
{
    public class WhiteNoise : Algorithm
    {
        public override float[,] GenerateHeightMap(int size)
        {
            float[,] noise = new float[size, size];

            Random random = new();

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    noise[x, y] = (float)random.NextDouble();
                }
            }

            return noise;
        }
    }
}