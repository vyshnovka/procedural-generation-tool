using System.Linq;
using UnityEditor;
using UnityEngine;
using TerrainGeneration.Algorithms;
using Utils;
using SharedDefs;
using System;
using System.Collections.Generic;

namespace TerrainGeneration
{
    public class TerrainGenerationManager : MonoBehaviour
    {
        public static event Action<int> TerrainSizeChanged;

        [Header("Terrain Data")]
        [SerializeField]
        private Terrain terrain;
        [SerializeField]
        private Texture2D texture;
        
        [Header("Gradients")]
        [SerializeField]
        private List<Gradient> gradients;

        private AlgorithmType selectedAlgorithmType = AlgorithmType.None;
        //TODO Add this tooltip as a warning to UI.
        [Tooltip("Values ​​below 128 are not recommended as this will result in low quality textures. \nValues ​​above 2048 may lead to poor performance on some devices.")]
        private Size selectedSize = Size._256;
        private GradientType selectedGradientType = GradientType.Grayscale;

        public string SelectedAlgorithmTypeAsName 
        { 
            get => selectedAlgorithmType.ToString(); 
            set
            {
                if (Enum.TryParse(value, out AlgorithmType newValue)) 
                {
                    selectedAlgorithmType = newValue;
                }
            }
        }
        public int SelectedSizeAsNumber
        {
            get => (int)selectedSize;
            set => selectedSize = (Size)value;
        }
        public int SelectedGradientTypeAsNumber
        {
            get => (int)selectedGradientType;
            set => selectedGradientType = (GradientType)value;
        }
        public float[,] HeightMap { get; set; }
        public bool NeedToGenerate { get; set; } = true;

        private string texturePath;

        void Start()
        {
            texturePath = AssetDatabase.GetAssetPath(texture);

            ResetTerrain();
        }

        /// <summary>Generate terrain using corresponding algorithm and apply texture to it.</summary>
        public void DisplayResult()
        {
            if (NeedToGenerate)
            {
                if (Type.GetType(typeof(Algorithm).Namespace + "." + SelectedAlgorithmTypeAsName) is Type algorithmType)
                {
                    var algorithm = (Algorithm)Activator.CreateInstance(algorithmType);
                    HeightMap = algorithm.GenerateHeightMap(SelectedSizeAsNumber);
                    HeightMap.NormalizeArray(SelectedSizeAsNumber, SelectedSizeAsNumber);
                }
                else
                {
                    ResetTerrain();
                    return;
                }
            }

            TerrainSizeChanged?.Invoke(SelectedSizeAsNumber);
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
            importer.maxTextureSize = SelectedSizeAsNumber;
            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);

            for (int x = 0; x < SelectedSizeAsNumber; x++)
            {
                for (int y = 0; y < SelectedSizeAsNumber; y++)
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
            terrain.terrainData.heightmapResolution = SelectedSizeAsNumber;
            terrain.terrainData.size = new Vector3(SelectedSizeAsNumber, SelectedSizeAsNumber / 10, SelectedSizeAsNumber);
            terrain.terrainData.SetHeights(0, 0, HeightMap);
        }

        /// <summary>Paint terrain pixel by pixel depending on its height.</summary>
        private void PaintTerrain()
        {
            float[,] heights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);

            TerrainLayer[] terrainLayers = terrain.terrainData.terrainLayers;
            int textureResolution = terrain.terrainData.alphamapResolution;
            int textureCount = terrainLayers.Length;

            // Loop through each point on terrain and set color depending on height and selected gradient.
            var selectedGradient = gradients[SelectedGradientTypeAsNumber];
            for (int x = 0; x < terrain.terrainData.heightmapResolution - 1; x++)
            {
                for (int y = 0; y < terrain.terrainData.heightmapResolution - 1; y++)
                {
                    float height = heights[x, y];
                    Color color = selectedGradient.Evaluate(height);

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
                terrainLayers[i].tileSize = new Vector2(SelectedSizeAsNumber, SelectedSizeAsNumber);
                terrainLayers[i].diffuseTexture.Apply();
            }
        }

        /// <summary>Reset terrain and texture data.</summary>
        private void ResetTerrain()
        {
            terrain.terrainData.heightmapResolution = SelectedSizeAsNumber;
            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
            TerrainSizeChanged?.Invoke(SelectedSizeAsNumber);
        }
    }
}