using System;

public class PerlinNoise : Algorithm
{
    public override float[,] GenerateHeightMap(int size)
    {
        float[,] noise = new float[size, size];

        int[] permutationTable = GetPermutationTable(size * 2);

        int octaves = 6;
        float persistence = 0.75f;
        float amplitude = 5f;
        float frequency = 1f;

        for (int i = 0; i < octaves; i++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float sampleX = (float)x / size * frequency;
                    float sampleY = (float)y / size * frequency;

                    int xi0 = (int)Math.Floor(sampleX) % 256;
                    int yi0 = (int)Math.Floor(sampleY) % 256;
                    int xi1 = (xi0 + 1) % 256;
                    int yi1 = (yi0 + 1) % 256;

                    float tx = sampleX - xi0;
                    float ty = sampleY - yi0;

                    int aa = permutationTable[permutationTable[xi0] + yi0];
                    int ab = permutationTable[permutationTable[xi0] + yi1];
                    int ba = permutationTable[permutationTable[xi1] + yi0];
                    int bb = permutationTable[permutationTable[xi1] + yi1];

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

    private int[] GetPermutationTable(int size)
    {
        int[] permutationTable = new int[size];

        for (int i = 0; i < size; i++)
        {
            permutationTable[i] = i;
        }

        Random random = new();

        for (int i = 255; i >= 0; i--)
        {
            int j = random.Next(i + 1);
            (permutationTable[j], permutationTable[i]) = (permutationTable[i], permutationTable[j]);
        }

        return permutationTable;
    }

    private float Grad(int hash, float x, float y)
    {
        return (hash & 3) switch
        {
            0 => x + y,
            1 => -x + y,
            2 => x - y,
            3 => -x - y,
            _ => 0,
        };
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
