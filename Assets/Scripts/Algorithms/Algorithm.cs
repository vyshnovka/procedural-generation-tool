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
    Grayscale = 0,
    Mountains = 1,
    Water = 2,
}

public enum Size
{
    _32 = 32,
    _64 = 64,
    _128 = 128,
    _256 = 256,
    _512 = 512,
    _1024 = 1024,
    _2048 = 2048,
    _4096 = 4096,
    _8192 = 8192,
}

public abstract class Algorithm
{
    public abstract float[,] GenerateHeightMap(int size);
}
