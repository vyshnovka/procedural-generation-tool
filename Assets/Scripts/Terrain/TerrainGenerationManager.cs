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
    private Gradient gradient;

    private float[,] heightMap;
    private Algorithm algorithm;
    private Texture2D drawNoise;
    private Texture2D terrainColor;

    void Start()
    {
        terrain.terrainData.heightmapResolution = 1;
        drawNoise = new Texture2D(size, size);
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
            default:
                terrain.terrainData.heightmapResolution = 1;
                drawNoise = new Texture2D(size, size);
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

        GUI.DrawTexture(rect, drawNoise);
    }

    private void ApplyTexture()
    {
        drawNoise = new Texture2D(size, size);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float n = heightMap[x, y];
                drawNoise.SetPixel(x, y, new Color(n, n, n, 1));
            }
        }

        drawNoise.Apply();
    }

    private void GenerateTerrain()
    {
        terrain.terrainData.heightmapResolution = size + 1;
        terrain.terrainData.size = new Vector3(size, 30, size);
        terrain.terrainData.SetHeights(0, 0, heightMap);
    }

    private void PaintTerrain()
    {
        //TODO clean up the code!!!

        // Get the terrain's heightmap.
        float[,] heights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);

        int maxHeight = 20;

        // Get the terrain's texture data.
        TerrainLayer[] terrainLayers = terrain.terrainData.terrainLayers;
        int textureResolution = terrain.terrainData.alphamapResolution;
        int textureCount = terrainLayers.Length;

        // Loop through each point on the terrain and set the color based on the height.
        for (int x = 0; x < terrain.terrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < terrain.terrainData.heightmapResolution; y++)
            {
                float height = heights[x, y] * maxHeight;
                Color color = gradient.Evaluate(height / maxHeight);

                // Set the texture colors.
                float[] textureValues = new float[textureCount];
                for (int i = 0; i < textureCount; i++)
                {
                    textureValues[i] = (i == 0) ? 1f : 0f;
                    if (terrainLayers[i]?.diffuseTexture != null && terrainLayers[i].diffuseTexture.isReadable)
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
                            terrainLayers[i].diffuseTexture.SetPixel(y, x, color);
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
