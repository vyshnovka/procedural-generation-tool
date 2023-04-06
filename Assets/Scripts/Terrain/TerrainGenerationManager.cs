using System.Linq;
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
    private GradientType gradientType;

    [Header("Gradients")]
    [SerializeField]
    private Gradient grayscale;
    [SerializeField]
    private Gradient mountains;
    [SerializeField]
    private Gradient water;

    private float[,] heightMap;
    private Algorithm algorithm;
    private Texture2D texture;
    private Gradient gradient;

    void Start()
    {
        terrain.terrainData.heightmapResolution = 1;
        texture = new Texture2D(size, size);
    }

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
            case AlgorithmType.RandomNoise:
                algorithm = new RandomNoise();
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
            max = new Vector2(256, 256)
        };

        GUI.DrawTexture(rect, texture);
    }

    private void ApplyTexture()
    {
        texture = new Texture2D(size, size);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
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
        // Get the terrain's heightmap.
        float[,] heights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);

        // Get the terrain's texture data.
        TerrainLayer[] terrainLayers = terrain.terrainData.terrainLayers;
        int textureResolution = terrain.terrainData.alphamapResolution;
        int textureCount = terrainLayers.Length;

        switch (gradientType)
        {
            case GradientType.Grayscale:
                gradient = grayscale;
                break;
            case GradientType.Mountains:
                gradient = mountains;
                break;
            case GradientType.Water:
                gradient = water;
                break;
            default:
                gradient = new();
                break;
        }

        // Loop through each point on the terrain and set the color based on the height.
        for (int x = 0; x < terrain.terrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < terrain.terrainData.heightmapResolution; y++)
            {
                float height = heights[x, y];
                Color color = gradient.Evaluate(height);

                // Set the texture colors.
                float[] textureValues = new float[textureCount];
                for (int i = 0; i < textureCount; i++)
                {
                    textureValues[i] = (i == 0) ? 1f : 0f;
                    if (terrainLayers[i].diffuseTexture != null && terrainLayers[i].diffuseTexture.isReadable)
                    {
                        Color textureColor = terrainLayers[i].diffuseTexture.GetPixelBilinear((float)x / textureResolution, (float)y / textureResolution);
                        float textureValue = textureColor.grayscale;
                        textureValues[i] = textureValue;
                    }
                }

                // Find the maximum texture value.
                float maxTextureValue = textureValues.Max();

                // Set the texture colors based on the gradient color.
                for (int i = 0; i < textureCount; i++)
                {
                    if (terrainLayers[i].diffuseTexture != null && terrainLayers[i].diffuseTexture.isReadable)
                    {
                        Color textureColor = terrainLayers[i].diffuseTexture.GetPixelBilinear((float)x / textureResolution, (float)y / textureResolution);
                        float textureValue = textureColor.grayscale;
                        if (textureValue == maxTextureValue)
                        {
                            terrainLayers[i].diffuseTexture.SetPixel(y, x, color); // Why the hell is it inverted?..
                        }
                    }
                }
            }
        }

        // Apply the texture changes.
        for (int i = 0; i < textureCount; i++)
        {
            terrainLayers[i].tileSize = new Vector2(size, size);
            terrainLayers[i].diffuseTexture.Apply();
        }
    }
}
