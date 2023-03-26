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
    [SerializeField]
    private Gradient gradient;

    private float[,] heightMap;
    private Algorithm algorithm;
    private Texture2D texture;

    public void DisplayResult()
    {
        switch (algorithmType)
        {
            case AlgorithmType.PerlinNoise: 
                algorithm = new PerlinNoise();
                break;
            case AlgorithmType.DiamondSquare:
                algorithm = new DiamondSquare();
                break;
            case AlgorithmType.WorleyNoise:
                algorithm = new WorleyNoise();
                break;
            default:
                terrain.terrainData.heightmapResolution = 1;
                texture = new Texture2D(size, size);
                return;
        }

        heightMap = algorithm.GenerateHeightMap(size);
        heightMap.NormalizeArray(size, size);

        ApplyTexture();
        GenerateTerrain();
        PaintTerrain();
    }

    void OnGUI()
    {
        Rect rect = new()
        {
            min = new Vector2(0, 0),
            max = new Vector2(size, size)
        };

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
        terrain.terrainData.heightmapResolution = size + 1;
        terrain.terrainData.size = new Vector3(size, 30, size);
        terrain.terrainData.SetHeights(0, 0, heightMap);
    }

    private void PaintTerrain()
    {
        //TODO paint somehow depending on height?
    }
}
