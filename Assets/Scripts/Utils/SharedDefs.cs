namespace SharedDefs
{
    /// <summary>Algorithm for terrain generation.</summary>
    public enum AlgorithmType
    {
        None = 0,
        WhiteNoise = 1,
        PerlinNoise = 2,
        DiamondSquare = 3,
        WorleyNoise = 4,
    }

    /// <summary>Colors to apply to terrain depending on height.</summary>
    public enum GradientType
    {
        Grayscale = 0,
        Mountains = 1,
        Water = 2,
    }

    /// <summary>Size of resulting terrain and texture.</summary>
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
}