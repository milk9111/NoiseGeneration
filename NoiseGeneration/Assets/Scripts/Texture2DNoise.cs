using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture2DNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    public int scale = 10;

    public float offsetX = 100;
    public float offsetY = 100;

    public float seed;
    public bool useSeed = false;

    private Renderer _renderer;
    private NoiseGenerator _noise;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _noise = new NoiseGenerator();
        BuildTexture();
    }

    void Update()
    {
        BuildTexture();
    }

    public void UpdateSeed()
    {
        if (useSeed)
        {
            _noise.Seed(seed);
        }
        else
        {
            _noise.RandomSeed();
        }
    }

    public void BuildTexture()
    {
        _renderer.material.mainTexture = GenerateTexture();
    }

    private Texture2D GenerateTexture()
    {
        var texture = new Texture2D(width, height);

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                texture.SetPixel(x, y, CalculateColor(x, y));
            }
        }

        texture.Apply();
        return texture;
    }

    private Color CalculateColor(int x, int y)
    {
        var xCoord = (float)x / width * scale + offsetX;
        var yCoord = (float)y / height * scale + offsetY;

        var num = _noise.Generate(xCoord, yCoord);
        return new Color(num, num, num);
    }
}
