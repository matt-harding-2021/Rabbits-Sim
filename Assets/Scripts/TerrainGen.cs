using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGen : MonoBehaviour
{
    Mesh mesh;

    int xSize;
    int zSize;
    Vector3[] vertices;
    void Start()
    {
        xSize = 250;
        zSize = 250;
        CreateMesh();
    }

    void CreateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.name = "Terrain Mesh";

        vertices= new Vector3[(xSize + 1) * (zSize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];

        Color[] colors = new Color[uv.Length];


        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                float y = GetComponent<Noise>().Noise2D(x * 0.025f, z * 0.025f) * 1.5f;
                y += GetComponent<Noise>().Noise2D(x * 0.01f, z * 0.01f) * 5.0f;
                if (y > -1f)
                    y += GetComponent<Noise>().Noise2D(x * 1.8f, z * 1.8f) * 0.15f;
                if (y >= 4.5f) Debug.Log("Terrain Height: " + y);
                vertices[i] = new Vector3(x, y, z) * 2.0f;

                uv[i] = new Vector2(
                    (float)x / xSize,
                    (float)z / zSize
                );

                if (y > -1f)
                {
                    colors[i] = Color.Lerp(
                        new Color(250f / 255f, 250f / 255f, 250f / 255f),
                        new Color(50f / 255f, 150f / 255f, 0f / 255f),
                        (uv[i].x + uv[i].y) / 1.25f);
                }
                else
                    colors[i] = new Color(220f / 255f, 220f / 255f, 120f / 255f);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.colors = colors;

        int[] triangles = new int[xSize * zSize * 6];
        for (int tri_index = 0, vert_index = 0, z = 0; z < zSize; z++, vert_index++)
        {
            for (int x = 0; x < xSize; x++, tri_index += 6, vert_index++)
            {
                //Creating two triangles for each grid
                triangles[tri_index] = vert_index;
                triangles[tri_index + 3] = triangles[tri_index + 2] = vert_index + 1; //Triangles share a point
                triangles[tri_index + 4] = triangles[tri_index + 1] = vert_index + xSize + 1; //Triangles share a point
                triangles[tri_index + 5] = vert_index + xSize + 2;
            }
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }
}
