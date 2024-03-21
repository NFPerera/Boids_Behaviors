using UnityEngine;

namespace _Main.Scripts.Steering_Behaviours
{
    public class EvadeSb : ISteeringBehaviour
    {
        private Transform m_origin;
        private Transform m_target;
        private float m_targetSpeed;
        private float m_time;
        
        public EvadeSb(Transform p_origin, Transform p_target,float p_targetSpeed, float p_time)
        {
            m_origin = p_origin;
            m_target = p_target;
            m_targetSpeed = p_targetSpeed;
            m_time = p_time;
        }
        public Vector3 GetDir()
        {
            Vector3 l_point = m_target.transform.position + (m_target.transform.forward * Mathf.Clamp(m_targetSpeed * m_time, 0, 100));
            return -(l_point - m_origin.position).normalized;
        }
    }
}
