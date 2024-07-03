using System;
using _Main.Scripts.Enum;
using _Main.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Main.Scripts.Boids
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class BoidsView : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [Header("FOV Settings")]
        [SerializeField] private Transform fovOffset;
        [SerializeField] private Material visionConeMaterial;
        [SerializeField] private int visionConeResolution = 120;
        [SerializeField] private float visionConeHeight = 3f;
        
        private Mesh m_visionConeMesh;
        private MeshFilter m_meshFilter;
        private BoidsModel m_model;
        private bool m_isBoidSelected;

        private event Action ShowFOV;
        private void Awake()
        {
            m_model = GetComponent<BoidsModel>();
        }

        
        void Start()
        {
            transform.GetComponent<MeshRenderer>().material = visionConeMaterial;
            m_meshFilter = GetComponent<MeshFilter>();
            m_visionConeMesh = new Mesh();
        }

        public void Initialize(bool p_is2d)
        {
            if (p_is2d)
                ShowFOV += Generate2dFovView;
            else
                ShowFOV += Generate3dFovView;
        }

        private void OnDestroy()
        {
            if (m_model.Is2D)
                ShowFOV -= Generate2dFovView;
            else
                ShowFOV -= Generate3dFovView;
        }

        public void RefreshFlockView(FlockData p_data)
        {
            m_isBoidSelected = false;
            meshFilter.mesh = p_data.BoidsMesh;
            meshRenderer.material = p_data.DefaultMaterial;
        }
        
        public void ChangeToSelectedMode(FlockData p_data)
        {
            m_isBoidSelected = true;
            meshRenderer.material = p_data.SelectedMaterial;
        }
        public void ChangeToUnselectedMode(FlockData p_data)
        {
            m_isBoidSelected = false;
            meshRenderer.material = p_data.DefaultMaterial;
            m_visionConeMesh.Clear();
        }

        private void OnGUI()
        {
            if(!m_isBoidSelected)
                return;
            
            ShowFOV?.Invoke();
        }

        #region Fov_View
        
        private void Generate3dFovView()
        {
            int l_totalVertices = (visionConeResolution + 1) * (visionConeResolution + 1);
            Vector3[] l_vertices = new Vector3[l_totalVertices];
            int[] l_triangles = new int[visionConeResolution * visionConeResolution * 6];

            l_vertices[0] = Vector3.zero;
            float l_viewAngle = m_model.GetData().GetStatById(BoidsStatsIds.ViewAngle);
            float l_viewRange = m_model.GetData().GetStatById(BoidsStatsIds.ViewRange);
            float l_angleStep = l_viewAngle / visionConeResolution;
            float l_currentAngleY = -l_viewAngle / 2f;

            int l_vertIndex = 1;
            int l_triIndex = 0;

            for (int l_y = 0; l_y <= visionConeResolution; l_y++)
            {
                float l_currentAngleX = -l_viewAngle / 2f;

                for (int l_x = 0; l_x <= visionConeResolution; l_x++)
                {
                    float l_radianY = l_currentAngleY * Mathf.Deg2Rad;
                    float l_radianX = l_currentAngleX * Mathf.Deg2Rad;

                    Vector3 l_direction = new Vector3(Mathf.Sin(l_radianY) * Mathf.Cos(l_radianX), Mathf.Sin(l_radianX), Mathf.Cos(l_radianY) * Mathf.Cos(l_radianX));
                    Vector3 l_vertex = l_direction * l_viewRange;
                    
                    if (l_vertIndex < l_totalVertices)
                    {
                        l_vertices[l_vertIndex] = l_vertex;
                    }

                    if (l_x < visionConeResolution && l_y < visionConeResolution && l_vertIndex + visionConeResolution + 2 < l_totalVertices)
                    {
                        l_triangles[l_triIndex] = l_vertIndex;
                        l_triangles[l_triIndex + 1] = l_vertIndex + visionConeResolution + 1;
                        l_triangles[l_triIndex + 2] = l_vertIndex + 1;

                        l_triangles[l_triIndex + 3] = l_vertIndex + 1;
                        l_triangles[l_triIndex + 4] = l_vertIndex + visionConeResolution + 1;
                        l_triangles[l_triIndex + 5] = l_vertIndex + visionConeResolution + 2;

                        l_triIndex += 6;
                    }

                    l_vertIndex++;
                    l_currentAngleX += l_angleStep;
                }
                l_currentAngleY += l_angleStep;
            }

            m_visionConeMesh.Clear();
            m_visionConeMesh.vertices = l_vertices;
            m_visionConeMesh.triangles = l_triangles;
            m_visionConeMesh.RecalculateNormals();
            m_meshFilter.mesh = m_visionConeMesh;
        }
        

        private void Generate2dFovView()
        {
            int[] l_triangles = new int[(visionConeResolution - 1) * 3];
            Vector3[] l_vertices = new Vector3[visionConeResolution + 1];
            l_vertices[0] = Vector3.zero;
            float l_currentAngle = - (m_model.GetData().GetStatById(BoidsStatsIds.ViewAngle) * Mathf.Deg2Rad)  / 2;
            float l_angleIcrement = (m_model.GetData().GetStatById(BoidsStatsIds.ViewAngle) * Mathf.Deg2Rad) / (visionConeResolution - 1);

            for (int l_i = 0; l_i < visionConeResolution; l_i++)
            {
                var l_sine = Mathf.Sin(l_currentAngle);
                var l_cosine = Mathf.Cos(l_currentAngle);
                Vector3 l_raycastDirection = (fovOffset.forward * l_cosine) + (fovOffset.up * l_sine);
                Vector3 l_vertForward = (Vector3.forward * l_cosine) + (Vector3.up * l_sine);
                if (Physics.Raycast(fovOffset.position, l_raycastDirection, out RaycastHit l_hit, m_model.GetData().GetStatById(BoidsStatsIds.ViewRange),
                        m_model.GetData().ObstacleMask))
                {
                    l_vertices[l_i + 1] = l_vertForward * l_hit.distance;
                }
                else
                {
                    l_vertices[l_i + 1] = l_vertForward * m_model.GetData().GetStatById(BoidsStatsIds.ViewRange);
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
            Gizmos.DrawWireSphere(transform.position, m_model.GetData().GetStatById(BoidsStatsIds.ViewRange));
            Gizmos.DrawLine(transform.position, transform.position + m_model.WantedDir*5);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward*5);
        }
#endif
    }
}