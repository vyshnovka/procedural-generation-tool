using System.Numerics;

namespace SourceCode
{
    internal class Program
    {
        readonly static int size = 4096;

        static void Main() 
        {
            Algorithm? algorithm;

            algorithm = new WhiteNoise();
            algorithm.GenerateHeightMap(size);

            algorithm = new PerlinNoise();
            algorithm.GenerateHeightMap(size);

            algorithm = new DiamondSquare();
            algorithm.GenerateHeightMap(size);

            algorithm = new WorleyNoise();
            algorithm.GenerateHeightMap(size);
        }
    }
}
