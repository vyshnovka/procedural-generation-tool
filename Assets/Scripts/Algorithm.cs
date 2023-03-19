using UnityEngine;

public enum AlgorithmType
{
    None = 0,
    DiamondSquare = 1,
    PerlinNoise = 2,
    SimplexNoise = 3,
    ValueNoise = 4,
}

public abstract class Algorithm
{
    public abstract float[,] GenerateHeightMap();
}
