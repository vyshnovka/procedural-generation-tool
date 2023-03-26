using System;

public enum AlgorithmType
{
    None = 0,
    PerlinNoise = 1,
    DiamondSquare = 2,
    WorleyNoise = 3,
}

public abstract class Algorithm
{
    public abstract float[,] GenerateHeightMap(int size);
}
