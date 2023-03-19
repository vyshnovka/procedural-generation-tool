public static class Extensions
{
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
