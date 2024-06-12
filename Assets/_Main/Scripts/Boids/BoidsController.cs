using System;
using UnityEngine;

namespace _Main.Scripts.Boids
{
    public class BoidsController : MonoBehaviour
    {
        private BoidsModel m_model;
        
        public void Initialize(BoidsModel p_model)
        {
            m_model = p_model;
        }
        private void Update()
        {
            var l_wantedDir = Vector3.zero;
            for (int l_i = 0; l_i < m_model.GetData().SteeringBehaviours3D.Count; l_i++)
            {
                if (m_model.Is2D)
                    l_wantedDir += m_model.GetData().SteeringBehaviours2D[l_i].GetDir(m_model);
                else
                    l_wantedDir += m_model.GetData().SteeringBehaviours3D[l_i].GetDir(m_model);
            }
            m_model.Move3d((gameObject.transform.forward + l_wantedDir), m_model.GetCurrSpeedBasedOnDistance(0.2f));
            
            
        }
    }
}