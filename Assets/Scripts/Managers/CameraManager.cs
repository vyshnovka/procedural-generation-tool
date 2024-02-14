using TerrainGeneration;
using UnityEngine;
using Utils;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        void OnEnable()
        {
            TerrainGenerationManager.TerrainSizeChanged += UpdateCameraPositionForTerrainSize;
        }

        void OnDisable()
        {
            TerrainGenerationManager.TerrainSizeChanged -= UpdateCameraPositionForTerrainSize;
        }

        /// <summary>Adjust camera to always show the whole terrain depending on its size.</summary>
        private void UpdateCameraPositionForTerrainSize(int position) => Camera.main.transform.SetPosition(position / 2, position / 2, -(position / 4));
    }
}
