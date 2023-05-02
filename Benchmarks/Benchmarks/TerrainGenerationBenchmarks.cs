using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using SourceCode;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class TerrainGenerationBenchmarks
    {
        private Algorithm? algorithm;
        private readonly int size = 256;

        [GlobalSetup(Targets = new string[] { nameof(WhiteNoiseTest) })]
        public void SetupWhiteNoise() => algorithm = new WhiteNoise();

        [GlobalSetup(Targets = new string[] { nameof(PerlinNoiseTest) })]
        public void SetupPerlinNoise() => algorithm = new PerlinNoise();

        [GlobalSetup(Targets = new string[] { nameof(DiamondSquareTest) })]
        public void SetupDiamondSquare() => algorithm = new DiamondSquare();

        [GlobalSetup(Targets = new string[] { nameof(WorleyNoiseTest) })]
        public void SetupWorleyNoise() => algorithm = new WorleyNoise();

        [Benchmark]
        public void WhiteNoiseTest() => algorithm?.GenerateHeightMap(size);

        [Benchmark]
        public void PerlinNoiseTest() => algorithm?.GenerateHeightMap(size);

        [Benchmark]
        public void DiamondSquareTest() => algorithm?.GenerateHeightMap(size);

        [Benchmark]
        public void WorleyNoiseTest() => algorithm?.GenerateHeightMap(size);

        [GlobalCleanup(Targets = new string[] { nameof(WhiteNoiseTest), nameof(PerlinNoiseTest), nameof(DiamondSquareTest), nameof(WorleyNoiseTest) })]
        public void Cleanup() => GC.Collect();
    }
}
