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
            m_data = m_model.GetData();
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