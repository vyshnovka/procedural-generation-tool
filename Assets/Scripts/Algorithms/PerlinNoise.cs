using System;

namespace TerrainGeneration.Algorithms
{
    public class PerlinNoise : Algorithm
    {
        public override float[,] GenerateHeightMap(int size)
        {
            float[,] noise = new float[size, size];

            int[] permutationTable = GetPermutationTable(size);

            int octaves = 6;
            float persistence = 0.75f;
            float amplitude = 5f;
            float frequency = 1f;

            for (int i = 0; i < octaves; i++)
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        float sampleX = (float)x / size * frequency;
                        float sampleY = (float)y / size * frequency;

                        int xi0 = (int)Math.Floor(sampleX) % size;
                        int yi0 = (int)Math.Floor(sampleY) % size;
                        int xi1 = (xi0 + 1) % size;
                        int yi1 = (yi0 + 1) % size;

                        float tx = sampleX - xi0;
                        float ty = sampleY - yi0;

                        int aa = permutationTable[(permutationTable[xi0 % size] + yi0) % size];
                        int ab = permutationTable[(permutationTable[xi0 % size] + yi1) % size];
                        int ba = permutationTable[(permutationTable[xi1 % size] + yi0) % size];
                        int bb = permutationTable[(permutationTable[xi1 % size] + yi1) % size];

                        float gradientX1 = Grad(aa, tx, ty);
                        float gradientX2 = Grad(ba, tx - 1f, ty);
                        float gradientX3 = Grad(ab, tx, ty - 1f);
                        float gradientX4 = Grad(bb, tx - 1f, ty - 1f);

                        float u = Smooth(tx);
                        float v = Smooth(ty);

                        float i1 = Lerp(gradientX1, gradientX2, u);
                        float i2 = Lerp(gradientX3, gradientX4, u);
                        float final = Lerp(i1, i2, v);

                        noise[x, y] += final * amplitude;
                    }
                }

                amplitude *= persistence;
                frequency *= 2f;
            }

            return noise;
        }

        /// <summary>Generate permutation table of a given size and fill it with random values.</summary>
        private int[] GetPermutationTable(int size)
        {
            int[] permutationTable = new int[size];

            for (int i = 0; i < size; i++)
            {
                permutationTable[i] = i / 2;
            }

            Random random = new();

            for (int i = size - 1; i >= 0; i--)
            {
                int j = random.Next(i);
                (permutationTable[j], permutationTable[i]) = (permutationTable[i], permutationTable[j]);
            }

            return permutationTable;
        }

        #region Perlin-specific Helper Methods
        private float Grad(int hash, float x, float y) => (hash & 3) switch
        {
            0 => x + y,
            1 => -x + y,
            2 => x - y,
            3 => -x - y,
            _ => 0,
        };

        private float Smooth(float t) => t * t * t * (t * (t * 6f - 15f) + 10f);

        private float Lerp(float a, float b, float t) => a + (b - a) * t;
        #endregion
    }
}