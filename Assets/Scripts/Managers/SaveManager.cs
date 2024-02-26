using UnityEngine;
using SimpleFileBrowser;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TerrainGeneration;
using System.Collections;
using System;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        private readonly string fileName = "height_map.dat";

        [SerializeField]
        private TerrainGenerationManager terrainManager;

        /// <summary>Save float array (height map) to a .dat file in binary.</summary>
        public void SaveFloatArray()
        {
            var arrayToSave = terrainManager.HeightMap;
            if (arrayToSave == null) 
                return;

            FileBrowser.ShowSaveDialog((paths) => 
            {
                var filePath = paths[0];

                BinaryFormatter formatter = new();
                using (MemoryStream stream = new())
                {
                    formatter.Serialize(stream, arrayToSave);
                    FileBrowserHelpers.WriteBytesToFile(filePath, stream.ToArray());
                }
            }, null, FileBrowser.PickMode.Files, false, null, fileName, "Save Height Map as...", "Save");
        }

        /// <summary>Load height map from a .dat file that contains a float array in binary.</summary>
        public void LoadFloatArray(Action<bool> LoadResultCallback) => StartCoroutine(ShowLoadDialogCoroutine(LoadResultCallback));

        private IEnumerator ShowLoadDialogCoroutine(Action<bool> LoadResultCallback)
        {
            yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, fileName, "Load Height Map from...", "Select");

            bool isSuccess = FileBrowser.Success;
            if (isSuccess)
            {
                var filePath = FileBrowser.Result[0];

                var byteArray = FileBrowserHelpers.ReadBytesFromFile(filePath);

                BinaryFormatter formatter = new();
                using (MemoryStream stream = new(byteArray))
                {
                    terrainManager.HeightMap = (float[,])formatter.Deserialize(stream);
                    terrainManager.SelectedSizeAsNumber = terrainManager.HeightMap.GetLength(0);
                    terrainManager.NeedToGenerate = false;
                }
            }

            LoadResultCallback?.Invoke(isSuccess);
        }
    }
}
