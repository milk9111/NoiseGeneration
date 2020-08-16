using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainNoise : MonoBehaviour
{
    public int depth = 20;
    public int width = 256;
    public int height = 256;

    public float scale = 20f;

    public float offsetX = 100f;
    public float offsetY = 100f;

    private Terrain _terrain;
    private NoiseGenerator _noise;

    // Start is called before the first frame update
    void Start()
    {
        _terrain = GetComponent<Terrain>();
        _noise = new NoiseGenerator();

        BuildTerrain();
    }

    private void BuildTerrain()
    {
        _terrain.terrainData = GenerateTerrain(_terrain.terrainData);
    }

    private TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);

        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    private float[,] GenerateHeights()
    {
        var heights = new float[width, height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                heights[x, y] = GenerateHeight(x, y);
            }
        }

        return heights;
    }

    private float GenerateHeight(float x, float y)
    {
        var xCoord = (float)x / width * scale + offsetX;
        var yCoord = (float)y / height * scale + offsetY;

        return _noise.Generate(xCoord, yCoord);
    }

    // Update is called once per frame
    void Update()
    {
        BuildTerrain();

        offsetX += Time.deltaTime * 2f;
        offsetY += Time.deltaTime * 2f;
    }
}
