using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace _Main.Scripts.Boids
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class BoidView : MonoBehaviour
    {
        [Header("FOV Settings")]
        [SerializeField] private Transform fovOffset;
        [SerializeField] private Material visionConeMaterial;
        [SerializeField] private int visionConeResolution = 120;
        [SerializeField] private float visionConeHeight = 3f;
        
        private Mesh m_visionConeMesh;
        private MeshFilter m_meshFilter;
        private BoidModel m_model;
        private BoidData m_data;
        private bool m_isBoidSelected;

        private void Awake()
        {
            m_model = GetComponent<BoidModel>();
        }

        
        void Start()
        {
            transform.GetComponent<MeshRenderer>().material = visionConeMaterial;
            m_meshFilter = GetComponent<MeshFilter>();
            m_visionConeMesh = new Mesh();
            m_data = m_model.GetData();
        }
         

        private void OnGUI()
        {
            Generate3dFovView();
        }

        #region Fov_View
        private void Generate3dFovView()
        {
            m_meshFilter.mesh = m_visionConeMesh;

            // Vertices
            Vector3[] l_vertices = new Vector3[(visionConeResolution + 1) * 2 + 1];
            float l_angleIncrement = (m_data.ViewAngle * 2) / visionConeResolution;

            
            for (int i = 0; i <= visionConeResolution; i++)
            {
                float l_angleRad = Mathf.Deg2Rad * l_angleIncrement * i;
                
                
                var l_centerOffset = Mathf.Deg2Rad * (90f - m_data.ViewAngle/2);
                
                float l_x = Mathf.Cos(l_angleRad + l_centerOffset) * m_data.ViewRange * 2;
                float l_z = Mathf.Sin(l_angleRad + l_centerOffset) * m_data.ViewRange * 2;
                
                l_vertices[i] = new Vector3(l_x, -visionConeHeight, l_z);
                l_vertices[i + visionConeResolution + 1] = new Vector3(l_x,  + visionConeHeight, l_z );
            }
            l_vertices[l_vertices.Length - 1] = fovOffset.position;

            // Triangles
            int[] l_triangles = new int[visionConeResolution * 6];
            for (int i = 0, vi = 0; i < visionConeResolution * 3; i += 6, vi++)
            {
                l_triangles[i] = vi;
                l_triangles[i + 1] = (vi + 1) % (visionConeResolution + 1);
                l_triangles[i + 2] = vi + visionConeResolution + 1;

                l_triangles[i + 3] = vi + visionConeResolution + 1;
                l_triangles[i + 4] = (vi + 1) % (visionConeResolution + 1);
                l_triangles[i + 5] = (vi + 1) % (visionConeResolution + 1) + visionConeResolution + 1;
            }

            // Normals
            Vector3[] l_normals = new Vector3[l_vertices.Length];
            for (int i = 0; i < l_vertices.Length; i++)
            {
                l_normals[i] = Vector3.up;
            }

            m_visionConeMesh.Clear();
            m_visionConeMesh.vertices = l_vertices;
            m_visionConeMesh.triangles = l_triangles;
            m_visionConeMesh.normals = l_normals;
        }
        
        private void DrawVisionCone2D()
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
                Vector3 l_raycastDirection = (fovOffset.forward * l_cosine) + (fovOffset.right * l_sine);
                Vector3 l_vertForward = (Vector3.forward * l_cosine) + (Vector3.right * l_sine);
                if (Physics.Raycast(fovOffset.position, l_raycastDirection, out RaycastHit l_hit, m_data.ViewRange,
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
        

        #endregion
        
        

        

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_data.ViewRange);
            Gizmos.DrawLine(transform.position, transform.position + m_model.WantedDir*5);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward*5);
        }
#endif
    }
}