using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshNoise : MonoBehaviour
{
    public int xSize = 20;
    public int zSize = 20;

    public List<NoiseLayer> noiseLayers = new List<NoiseLayer>();

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

        foreach(var layer in noiseLayers)
        {
            layer.SetNoiseGenerator(_noise);
        }

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
                var elevation = 0f;
                foreach (var layer in noiseLayers)
                {
                    elevation += layer.Noise(x, z);
                }

                vertices[i] = new Vector3(x, elevation, z);
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
        foreach(var layer in noiseLayers)
        {
            layer.offset.x -= Time.deltaTime * 1.2f;
            layer.offset.y -= Time.deltaTime * 1.2f;
        }

        BuildMesh();
    }
}
