using UnityEngine;

public class TerrainGenerationManager : MonoBehaviour
{
    [SerializeField]
    private AlgorithmType algorithmType;

    [Header("Terrain Data")]
    [SerializeField]
    private Terrain terrain;
    [SerializeField]
    protected int width = 512;
    [SerializeField]
    protected int height = 512;

    private Algorithm algorithm;
    private float[,] heightMap;

    private Texture2D texture;

    public void DisplayResult()
    {
        switch (algorithmType)
        {
            case AlgorithmType.DiamondSquare:
                algorithm = new DiamondSquare();
                break;
            default:
                return;
                break;
        }

        heightMap = algorithm.GenerateHeightMap();
        heightMap.NormalizeArray(width, height);

        ApplyTexture();
        GenerateTerrain();
    }

    void OnGUI()
    {
        Rect rect = new Rect();
        rect.min = new Vector2(0, 0);
        rect.max = new Vector2(width / 2, height / 2);

        GUI.DrawTexture(rect, texture);
    }

    private void ApplyTexture()
    {
        texture = new Texture2D(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float n = heightMap[x, y];
                texture.SetPixel(x, y, new Color(n, n, n, 1));
            }
        }

        texture.Apply();
    }

    private void GenerateTerrain()
    {
        this.terrain.terrainData.heightmapResolution = width + 1;
        this.terrain.terrainData.size = new Vector3(width, 30, height);
        this.terrain.terrainData.SetHeights(0, 0, heightMap);
    }
}
