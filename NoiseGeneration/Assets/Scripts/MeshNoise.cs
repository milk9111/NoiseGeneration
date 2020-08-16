using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshNoise : MonoBehaviour
{
    public int xSize = 20;
    public int zSize = 20;

    public float scale = 20f;
    public float offsetX = 100f;
    public float offsetY = 100f;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private NoiseGenerator _noise;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        _noise = new NoiseGenerator();

        BuildMesh();
    }

    private void BuildMesh()
    {
        CreateShape();
        UpdateMesh();
    }

    private void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        var i = 0;
        for (var z = 0; z <= zSize; z++)
        {
            for (var x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x, GenerateHeight(x, z), z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        var vert = 0;
        var tris = 0;
        for (var z = 0; z < zSize; z++)
        {
            for (var x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }
    }

    private float GenerateHeight(float x, float y)
    {
        var xCoord = (float)x / xSize * scale + offsetX;
        var yCoord = (float)y / zSize * scale + offsetY;

        return _noise.Generate(xCoord, yCoord);
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {
        BuildMesh();
    }
}
