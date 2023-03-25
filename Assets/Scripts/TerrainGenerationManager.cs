using UnityEngine;

public class TerrainGenerationManager : MonoBehaviour
{
    [SerializeField]
    private AlgorithmType algorithmType;

    [Header("Terrain Data")]
    [SerializeField]
    private Terrain terrain;
    [SerializeField]
    private int size = 512;

    private Algorithm algorithm;
    private float[,] heightMap;

    private Texture2D texture;

    public void DisplayResult()
    {
        switch (algorithmType)
        {
            case AlgorithmType.DiamondSquare:
                algorithm = new DiamondSquare();
                break;
            case AlgorithmType.PerlinNoise: 
                algorithm = new PerlinNoise();
                break;
            case AlgorithmType.SimplexNoise:
                algorithm = new SimplexNoise();
                break;
            case AlgorithmType.WorleyNoise:
                algorithm = new WorleyNoise();
                break;
            default:
                terrain.terrainData.heightmapResolution = 1;
                return;
        }

        heightMap = algorithm.GenerateHeightMap(size);
        heightMap.NormalizeArray(size, size);

        ApplyTexture();
        GenerateTerrain();
    }

    void OnGUI()
    {
        Rect rect = new Rect();
        rect.min = new Vector2(0, 0);
        rect.max = new Vector2(size, size);

        GUI.DrawTexture(rect, texture);
    }

    private void ApplyTexture()
    {
        texture = new Texture2D(size, size);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float n = heightMap[x, y];
                texture.SetPixel(x, y, new Color(n, n, n, 1));
            }
        }

        texture.Apply();
    }

    private void GenerateTerrain()
    {
        this.terrain.terrainData.heightmapResolution = size + 1;
        this.terrain.terrainData.size = new Vector3(size, 30, size);
        this.terrain.terrainData.SetHeights(0, 0, heightMap);
    }
}
