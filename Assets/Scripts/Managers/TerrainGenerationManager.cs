using System.Linq;
using UnityEditor;
using UnityEngine;
using TerrainGeneration.Algorithms;
using Utils;
using SharedDefs;
using System;

namespace TerrainGeneration
{
    public class TerrainGenerationManager : MonoBehaviour
    {
        public static event Action<int> TerrainSizeChanged;

        [SerializeField]
        private AlgorithmType algorithmType = AlgorithmType.None;

        [Header("Terrain Data")]
        [SerializeField]
        private Terrain terrain;
        [SerializeField]
        [Tooltip("Values ​​below 128 are not recommended as this will result in low quality textures. \nValues ​​above 2048 may lead to poor performance on some devices.")]
        private Size size;
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

        public int TerrainSize
        {
            get { return (int)size; }
            set 
            {
                size = (Size)value;
                TerrainSizeChanged?.Invoke((int)size); 
            }
        }
        public float[,] HeightMap { get; set; }
        public bool NeedToGenerate { get; set; } = true;

        private Algorithm algorithm;
        private Gradient gradient;
        private string texturePath;

        void Start()
        {
            TerrainSize = (int)size; //? I don't like it.
            texturePath = AssetDatabase.GetAssetPath(texture);

            ResetTerrain();
        }

        /// <summary>Generate terrain using corresponding algorithm and apply texture to it.</summary>
        public void DisplayResult()
        {
            TerrainSize = (int)size; //? Same, looks bad and makes no sense.

            //? Maybe separate this?..
            if (NeedToGenerate)
            {
                switch (algorithmType)
                {
                    case AlgorithmType.WhiteNoise:
                        algorithm = new WhiteNoise();
                        break;
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
                        ResetTerrain();
                        return;
                }

                HeightMap = algorithm.GenerateHeightMap(TerrainSize);
                HeightMap.NormalizeArray(TerrainSize, TerrainSize);
            }

            GenerateTerrain();
            ApplyTexture();
            PaintTerrain();
            NeedToGenerate = true;
        }

        /// <summary>Apply generated texture pixel by pixel.</summary>
        private void ApplyTexture()
        {
            // Reimport texture with corresponding max size.
            TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            importer.maxTextureSize = TerrainSize;
            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);

            for (int x = 0; x < TerrainSize; x++)
            {
                for (int y = 0; y < TerrainSize; y++)
                {
                    float n = HeightMap[x, y];
                    texture.SetPixel(x, y, new Color(n, n, n, 1));
                }
            }

            texture.Apply();
        }

        /// <summary>Set terrain heights.</summary>
        private void GenerateTerrain()
        {
            terrain.terrainData.heightmapResolution = TerrainSize;
            terrain.terrainData.size = new Vector3(TerrainSize, TerrainSize / 10, TerrainSize);
            terrain.terrainData.SetHeights(0, 0, HeightMap);
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
                    break;
            }

            // Loop through each point on terrain and set color depending on height.
            for (int x = 0; x < terrain.terrainData.heightmapResolution; x++)
            {
                for (int y = 0; y < terrain.terrainData.heightmapResolution; y++)
                {
                    float height = heights[x, y];
                    Color color = gradient.Evaluate(height);

                    float[] textureValues = new float[textureCount];
                    for (int i = 0; i < textureCount; i++)
                    {
                        textureValues[i] = i == 0 ? 1f : 0f;
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
                terrainLayers[i].tileSize = new Vector2(TerrainSize, TerrainSize);
                terrainLayers[i].diffuseTexture.Apply();
            }
        }

        /// <summary>Reset terrain and texture data.</summary>
        private void ResetTerrain()
        {
            terrain.terrainData.heightmapResolution = TerrainSize;
            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
        }
    }
}