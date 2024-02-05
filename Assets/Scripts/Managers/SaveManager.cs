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

        public void SaveFloatArray()
        {
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

        public void LoadFloatArray()
        {
            string filePath = EditorUtility.OpenFilePanel("Load Height Map from...", "", "dat");

            if (!string.IsNullOrEmpty(filePath))
            {
                BinaryFormatter formatter = new();
                using (FileStream stream = new(filePath, FileMode.Open))
                {
                    //TODO Also need to set the size of the map accordingly. Now it can cause errors if loaded map is smaller/bigger than the size that is set.
                    terrainManager.isLoaded = true;
                    terrainManager.HeightMap = (float[,])formatter.Deserialize(stream);
                }
            }
        }
    }
}
