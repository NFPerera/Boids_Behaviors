using System;
using UnityEngine;

namespace _Main.Scripts.Boids
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] private BoidModel model;
        [SerializeField] private Material visionConeMaterial;
        [SerializeField] private int visionConeResolution = 120; 
        //the vision cone will be made up of triangles, the higher this value is the pretier the vision cone will be

        private Mesh m_visionConeMesh;
        private MeshFilter m_meshFilter;
        private BoidData m_data;


        //Create all of these variables, most of them are self explanatory, but for the ones that aren't i've added a comment to clue you in on what they do
        //for the ones that you dont understand dont worry, just follow along
        void Start()
        {
            transform.GetComponent<MeshRenderer>().material = visionConeMaterial;
            m_meshFilter = GetComponent<MeshFilter>();
            m_visionConeMesh = new Mesh();
            m_data = model.GetData();
        }

        private void OnGUI()
        {
            Generate3dFovView();
        }

        void Generate3dFovView()
        {
            m_meshFilter.mesh = m_visionConeMesh;

            // Vertices
            Vector3[] vertices = new Vector3[(visionConeResolution + 1) * 2 + 1];
            float angleIncrement = (m_data.ViewAngle * 2) / visionConeResolution;

            var centerPos = transform.position + transform.forward * m_data.ViewRange;
            for (int i = 0; i <= visionConeResolution; i++)
            {
                float angle = Mathf.Deg2Rad * angleIncrement * i;
                var centerOffset = Mathf.Deg2Rad * (90f - m_data.ViewAngle/2);
                float x = Mathf.Cos(angle + centerOffset) * m_data.ViewRange * 2;
                float z = Mathf.Sin(angle + centerOffset) * m_data.ViewRange * 2;
                
                vertices[i] = new Vector3(x, -3, z);
                vertices[i + visionConeResolution + 1] = new Vector3(x,  + 3, z );
            }
            vertices[vertices.Length - 1] = transform.position;

            // Triangles
            int[] triangles = new int[visionConeResolution * 6];
            for (int i = 0, vi = 0; i < visionConeResolution * 3; i += 6, vi++)
            {
                triangles[i] = vi;
                triangles[i + 1] = (vi + 1) % (visionConeResolution + 1);
                triangles[i + 2] = vi + visionConeResolution + 1;

                triangles[i + 3] = vi + visionConeResolution + 1;
                triangles[i + 4] = (vi + 1) % (visionConeResolution + 1);
                triangles[i + 5] = (vi + 1) % (visionConeResolution + 1) + visionConeResolution + 1;
            }

            // Normals
            Vector3[] normals = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                normals[i] = Vector3.up;
            }

            m_visionConeMesh.Clear();
            m_visionConeMesh.vertices = vertices;
            m_visionConeMesh.triangles = triangles;
            m_visionConeMesh.normals = normals;
        }
        
        void DrawVisionCone3D()
        {
            int resolution = visionConeResolution; // Renamed for clarity
            int numVertices = (resolution + 1) * (resolution + 1) + 1;
            int numTriangles = resolution * resolution * 6; // 2 triangles per quad

            Vector3[] vertices = new Vector3[numVertices];
            int[] triangles = new int[numTriangles];

            
            vertices[0] = Vector3.zero;

            // Calculate vertices in both horizontal and vertical axes
            for (int i = 0; i <= resolution; i++)
            {
                for (int j = 0; j <= resolution - 1; j++)
                {
                    float horizontalAngle = -m_data.ViewAngle / 2 + i * (m_data.ViewAngle / resolution);
                    float verticalAngle = -m_data.ViewAngle / 2 + j * (m_data.ViewAngle / resolution);

                    float x = Mathf.Sin(horizontalAngle * Mathf.Deg2Rad) * Mathf.Cos(verticalAngle * Mathf.Deg2Rad);
                    float y = Mathf.Sin(verticalAngle * Mathf.Deg2Rad);
                    float z = Mathf.Cos(horizontalAngle * Mathf.Deg2Rad) * Mathf.Cos(verticalAngle * Mathf.Deg2Rad);

                    Vector3 direction = new Vector3(x, y, z);
                    if (Physics.Raycast(transform.position, direction, out RaycastHit hit, m_data.ViewRange, m_data.ObstacleMask))
                    {
                        vertices[i * (resolution + 1) + j + 1] = hit.point - transform.position;
                    }
                    else
                    {
                        vertices[i * (resolution + 1) + j + 1] = direction * m_data.ViewRange;
                    }
                }
            }

            // Calculate triangles
            int vert = 0;
            int tris = 0;
            for (int i = 0; i < resolution; i++)
            {
                for (int j = 0; j < resolution; j++)
                {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + resolution + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + resolution + 1;
                    triangles[tris + 5] = vert + resolution + 2;

                    vert++;
                    tris += 6;
                }
                vert++;
            }

            // Assign vertices and triangles to the mesh
            m_visionConeMesh.Clear();
            m_visionConeMesh.vertices = vertices;
            m_visionConeMesh.triangles = triangles;
            m_meshFilter.mesh = m_visionConeMesh;
            
        }
        
        void DrawVisionCone2D() //this method creates the vision cone mesh
        {
            int[] l_triangles = new int[(visionConeResolution - 1) * 3];
            Vector3[] l_vertices = new Vector3[visionConeResolution + 1];
            l_vertices[0] = Vector3.zero;
            float l_currentAngle = - (m_data.ViewAngle * Mathf.Deg2Rad)  / 2;
            float l_angleIcrement = (m_data.ViewAngle * Mathf.Deg2Rad) / (visionConeResolution - 1);

            for (int l_i = 0; l_i < visionConeResolution; l_i++)
            {
                var l_sine = Mathf.Sin(l_currentAngle);
                var l_cosine = Mathf.Cos(l_currentAngle);
                Vector3 l_raycastDirection = (transform.forward * l_cosine) + (transform.right * l_sine);
                Vector3 l_vertForward = (Vector3.forward * l_cosine) + (Vector3.right * l_sine);
                if (Physics.Raycast(transform.position, l_raycastDirection, out RaycastHit l_hit, m_data.ViewRange,
                        m_data.ObstacleMask))
                {
                    l_vertices[l_i + 1] = l_vertForward * l_hit.distance;
                }
                else
                {
                    l_vertices[l_i + 1] = l_vertForward * m_data.ViewRange;
                }


                l_currentAngle += l_angleIcrement;
            }

            for (int l_i = 0, l_j = 0; l_i < l_triangles.Length; l_i += 3, l_j++)
            {
                l_triangles[l_i] = 0;
                l_triangles[l_i + 1] = l_j + 1;
                l_triangles[l_i + 2] = l_j + 2;
            }

            m_visionConeMesh.Clear();
            m_visionConeMesh.vertices = l_vertices;
            m_visionConeMesh.triangles = l_triangles;
            m_meshFilter.mesh = m_visionConeMesh;
        }
    }
}