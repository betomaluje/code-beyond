using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class PlaneMeshGenerator : MonoBehaviour
{
    [SerializeField] private int xSize = 20, zSize = 20;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Vector4[] tangents;
    Vector2[] uv;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        mesh.name = "Generated Mesh";
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    private void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        uv = new Vector2[vertices.Length];
        tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, 0, z);
                uv[i] = new Vector2((float)x / xSize, (float)z / zSize);
                tangents[i] = tangent;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
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

        mesh.uv = uv;
        mesh.tangents = tangents;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        foreach (Vector3 vertice in vertices)
        {
            Gizmos.DrawSphere(vertice, .1f);
        }
    }
}
