using UnityEngine;

namespace Utils
{
    public static class ArrayExtensions
    {
        /// <summary>Normalize float array with given <paramref name="width"/> and <paramref name="height"/>.</summary>
        public static float[,] NormalizeArray(this float[,] arr, int width, int height)
        {
            float min = float.PositiveInfinity;
            float max = float.NegativeInfinity;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float v = arr[x, y];
                    if (v < min) min = v;
                    if (v > max) max = v;
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float v = arr[x, y];
                    arr[x, y] = (v - min) / (max - min);
                }
            }

            return arr;
        }
    }

    public static class TransformExtensions
    {
        /// <summary> Set the position of a given transform (Unity only has the implementation that also includes rotation).</summary>
        public static void SetPosition(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            var objectToMove = transform.position;
            Camera.main.transform.position = new Vector3(x ?? objectToMove.x, y ?? objectToMove.y, z ?? objectToMove.z);
        }
    }
}