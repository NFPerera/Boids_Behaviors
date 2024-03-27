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
        private BoidModel m_model;
        private bool m_isBoidSelected;
        private BoidData m_data;

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
        
        
        
#region FOV

        [SerializeField] private Material visionConeMaterial;
        [SerializeField] private int visionConeResolution = 30; //the vision cone will be made up of triangles, the higher this value is the pretier the vision cone will be

        private Mesh m_visionConeMesh;

        private MeshFilter m_meshFilter;
        
        void DrawVisionCone2D() //this method creates the vision cone mesh
        {
            var l_visionAngle = m_data.ViewAngle * Mathf.Deg2Rad;
            int[] l_triangles = new int[(visionConeResolution - 1) * 3];
            Vector3[] l_vertices = new Vector3[visionConeResolution + 1];
            l_vertices[0] = Vector3.zero;
            float l_currentAngle = - l_visionAngle  / 2;
            float l_angleIcrement = l_visionAngle / (visionConeResolution - 1);
            

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

            for (int l_i = 0, l_j = 0; 
                 l_i < l_triangles.Length; 
                 l_i += 3, l_j++)
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
        
        void DrawVisionCone3D()
        {
            

        }
        
        
     
#endregion
        
        
    void Update()
        {
            //DrawVisionCone();//calling the vision cone function every frame just so the cone is updated every frame
        }

        private void OnGUI()
        {
            //DrawVisionCone2D();
            //DrawVisionCone3D();
        }

        

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