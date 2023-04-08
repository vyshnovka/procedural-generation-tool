using System.Linq;
using UnityEditor;
using UnityEngine;

public class TerrainGenerationManager : MonoBehaviour
{
    [SerializeField]
    private AlgorithmType algorithmType;

    [Header("Terrain Data")]
    [SerializeField]
    private Terrain terrain;
    [SerializeField]
    [Tooltip("Values below 128 are not recommended.")]
    private Size size = Size._256;
    [SerializeField]
    private GradientType gradientType;
    [SerializeField]
    private Texture2D texture;

    [Header("Gradients")]
    [SerializeField]
    private Gradient grayscale;
    [SerializeField]
    private Gradient mountains;
    [SerializeField]
    private Gradient water;

    private float[,] heightMap;
    private int maxSize;
    private Algorithm algorithm;
    private Gradient gradient;

    private string texturePath;

    void Start()
    {
        maxSize = (int)size;
        texturePath = AssetDatabase.GetAssetPath(texture);

        ResetTerrain();
    }

    /// <summary>Generate terrain using corresponding algorithm and apply texture to it.</summary>
    public void DisplayResult()
    {
        maxSize = (int)size;

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
                ResetTerrain();
                return;
        }

        heightMap = algorithm.GenerateHeightMap(maxSize);
        heightMap.NormalizeArray(maxSize, maxSize);

        GenerateTerrain();
        ApplyTexture();
        PaintTerrain();
    }

    /// <summary>Apply generated texture pixel by pixel.</summary>
    private void ApplyTexture()
    {
        // Reimport texture with corresponding max size.
        TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        importer.maxTextureSize = maxSize;
        AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);

        for (int x = 0; x < maxSize; x++)
        {
            for (int y = 0; y < maxSize; y++)
            {
                float n = heightMap[x, y];
                texture.SetPixel(x, y, new Color(n, n, n, 1));
            }
        }

        texture.Apply();
    }

    /// <summary>Set terrain heights.</summary>
    private void GenerateTerrain()
    {
        terrain.terrainData.heightmapResolution = maxSize;
        terrain.terrainData.size = new Vector3(maxSize, 30, maxSize);
        terrain.terrainData.SetHeights(0, 0, heightMap);
    }

    /// <summary>Paint terrain pixel by pixel depending on its height.</summary>
    private void PaintTerrain()
    {
        float[,] heights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);

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

                float maxTextureValue = textureValues.Max();

                // Set colors based on corresponding gradient.
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

        // Update all terrain layers.
        for (int i = 0; i < textureCount; i++)
        {
            terrainLayers[i].tileSize = new Vector2(maxSize, maxSize);
            terrainLayers[i].diffuseTexture.Apply();
        }
    }

    /// <summary>Reset terrain and texture data.</summary>
    private void ResetTerrain()
    {
        terrain.terrainData.heightmapResolution = maxSize;
        AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
    }
}
