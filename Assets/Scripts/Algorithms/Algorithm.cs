using System;

public enum AlgorithmType
{
    None = 0,
    DiamondSquare = 1,
    PerlinNoise = 2,
    WorleyNoise = 3,
}

public abstract class Algorithm
{
    public abstract float[,] GenerateHeightMap(int size);
}

public abstract class Noise : Algorithm
{
    public static int[] PermutationTable(int size)
    {
        int[] permutationTable = new int[size];

        for (int i = 0; i < size; i++)
        {
            permutationTable[i] = i;
        }

        Random rand = new Random();

        for (int i = 255; i >= 0; i--)
        {
            int j = rand.Next(i + 1);
            int temp = permutationTable[i];
            permutationTable[i] = permutationTable[j];
            permutationTable[j] = temp;
        }

        return permutationTable;
    }
}
