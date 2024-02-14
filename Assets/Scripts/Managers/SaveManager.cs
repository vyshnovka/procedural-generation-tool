using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TerrainGeneration;
using UnityEditor;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        private readonly string fileName = "height_map.dat";

        [SerializeField]
        private TerrainGenerationManager terrainManager;

        /// <summary>Save float array (height map) to a .dat file.</summary>
        public void SaveFloatArray()
        {
            //? Also need to save the terrain image.
            var arrayToSave = terrainManager.HeightMap;
            string filePath = EditorUtility.SaveFilePanel("Save Height Map as...", "", fileName, "dat");

            if (string.IsNullOrEmpty(filePath))
                return;

            BinaryFormatter formatter = new();
            using (FileStream stream = new(filePath, FileMode.Create))
            {
                formatter.Serialize(stream, arrayToSave);
            }
        }

        /// <summary>Load height map from a .dat file that contains a float array.</summary>
        public bool LoadFloatArray()
        {
            string filePath = EditorUtility.OpenFilePanel("Load Height Map from...", "", "dat");

            if (string.IsNullOrEmpty(filePath))
                return false;

            BinaryFormatter formatter = new();
            using (FileStream stream = new(filePath, FileMode.Open))
            {
                terrainManager.NeedToGenerate = false;
                terrainManager.HeightMap = (float[,])formatter.Deserialize(stream);
                terrainManager.SelectedSizeAsNumber = terrainManager.HeightMap.GetLength(0);
                return true;
            }
        }
    }
}
