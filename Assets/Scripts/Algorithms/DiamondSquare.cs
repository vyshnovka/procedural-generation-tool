using System;

namespace TerrainGeneration.Algorithms
{
    public class DiamondSquare : Algorithm
    {
        public override float[,] GenerateHeightMap(int size)
        {
            float[,] noise = new float[size + 1, size + 1];

            noise[0, 0] = noise[0, size] = noise[size, 0] = noise[size, size] = 0;

            Random random = new();

            float range = 1f;

            for (int sideLength = size; sideLength >= 2; sideLength /= 2, range /= 2.0f)
            {
                int halfSide = sideLength / 2;

                for (int x = 0; x < size; x += sideLength)
                {
                    for (int y = 0; y < size; y += sideLength)
                    {
                        float avg = noise[x, y] + noise[x + sideLength, y] + noise[x, y + sideLength] + noise[x + sideLength, y + sideLength];
                        avg /= 4.0f;

                        noise[x + halfSide, y + halfSide] = avg + (float)random.NextDouble() * 2f * range - range;
                    }
                }

                for (int x = 0; x < size; x += halfSide)
                {
                    for (int y = (x + halfSide) % sideLength; y < size; y += sideLength)
                    {
                        float avg = noise[(x - halfSide + size + 1) % (size + 1), y] + noise[(x + halfSide) % (size + 1), y] + noise[x, (y + halfSide) % (size + 1)] + noise[x, (y - halfSide + size + 1) % (size + 1)];
                        avg /= 4.0f;
                        avg = avg + (float)random.NextDouble() * 2f * range - range;

                        noise[x, y] = avg;

                        if (x == 0) noise[size, y] = avg;
                        if (y == 0) noise[x, size] = avg;
                    }
                }
            }

            return noise;
        }
    }
}