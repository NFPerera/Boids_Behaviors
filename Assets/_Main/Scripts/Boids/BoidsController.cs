using System;
using _Main.Scripts.Enum;
using _Main.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Main.Scripts.Boids
{
    public class BoidsController : MonoBehaviour
    {
        private BoidsModel m_model;
        private BoidsData m_data;
        private RaycastHit[] m_obstacleRayHit;
        
        public void Initialize(BoidsModel p_model)
        {
            m_model = p_model;
            m_data = m_model.GetData();
            m_obstacleRayHit = new RaycastHit[5];
        }
        private void Update()
        {
            var l_wantedDir = Vector3.zero;
            for (int l_i = 0; l_i < m_data.SteeringBehaviours3D.Count; l_i++)
            {
                if (m_model.Is2D)
                    l_wantedDir += m_data.SteeringBehaviours2D[l_i].GetDir(m_model);
                else
                    l_wantedDir += m_data.SteeringBehaviours3D[l_i].GetDir(m_model);
            }
            m_model.Move((gameObject.transform.forward + l_wantedDir),
                m_data.GetStatById(BoidsStatsIds.MovementSpeed) * GetCurrSpeedBasedOnDistance(0.02f));
        }
        /*
        public float GetCurrSpeedBasedOnDistance(float p_decreasePace)
        {
            var l_size = Physics.RaycastNonAlloc(transform.position, transform.forward, m_obstacleRayHit, 
                m_data.GetData().GetStatById(BoidsStatsIds.ViewRange), m_data.GetData().ObstacleMask);
            
            if (l_size < 1)
                return m_data.GetData().GetStatById(BoidsStatsIds.MovementSpeed);

            var l_closestDistanceToObs = m_data.GetData().GetStatById(BoidsStatsIds.ViewRange);

            for (int l_i = 0; l_i < l_size; l_i++)
            {
                if (m_obstacleRayHit[l_i].distance < l_closestDistanceToObs)
                {
                    l_closestDistanceToObs = m_obstacleRayHit[l_i].distance;
                }
            }
            
            //return (data.TerminalVelocity)/(p_decreasePace+l_closestDistanceToObs);
            return m_data.GetData().GetStatById(BoidsStatsIds.MovementSpeed) / (1 + (float)Math.Pow(p_decreasePace * l_closestDistanceToObs, 2));
        }*/
        
        public float GetCurrSpeedBasedOnDistance(float p_decreasePace)
        {
            var l_size = Physics.RaycastNonAlloc(transform.position, transform.forward, m_obstacleRayHit, 
                m_data.GetStatById(BoidsStatsIds.ViewRange), m_data.ObstacleMask);
        
            if (l_size < 1)
                return 1.0f; // Return 1 when there are no obstacles

            var l_closestDistanceToObs = m_data.GetStatById(BoidsStatsIds.ViewRange);

            for (int l_i = 0; l_i < l_size; l_i++)
            {
                if (m_obstacleRayHit[l_i].distance < l_closestDistanceToObs)
                {
                    l_closestDistanceToObs = m_obstacleRayHit[l_i].distance;
                }
            }
        
            return 1 / (1 + Mathf.Pow(p_decreasePace * l_closestDistanceToObs, 2));
        }
    }
}