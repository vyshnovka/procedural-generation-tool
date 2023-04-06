public enum AlgorithmType
{
    None = 0,
    PerlinNoise = 1,
    DiamondSquare = 2,
    WorleyNoise = 3,
    RandomNoise = 4,
}

public enum GradientType
{
    None = 0,
    Grayscale = 1,
    Mountains = 2,
    Water = 3,
}

public abstract class Algorithm
{
    public abstract float[,] GenerateHeightMap(int size);
}
