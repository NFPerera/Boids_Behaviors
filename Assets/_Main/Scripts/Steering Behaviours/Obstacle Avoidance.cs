using UnityEngine;

namespace _Main.Scripts.Steering_Behaviours
{
    public class ObstacleAvoidanceSb : ISteeringBehaviour
    {

        private Transform m_origin;
        private float m_radius;
        private float m_viewAngle;
        private LayerMask m_mask;
        private Collider[] m_allObs;

        public ObstacleAvoidanceSb(Transform p_origin, float p_radius, int p_maxObs, float p_viewAngle, LayerMask p_mask)
        {
            m_origin = p_origin;
            m_radius = p_radius;
            m_mask = p_mask;
            m_viewAngle = p_viewAngle;
            m_allObs = new Collider[p_maxObs];
        }

        public Vector3 GetDir()
        {
            var l_countObs = Physics.OverlapSphereNonAlloc(m_origin.position, m_radius, m_allObs, m_mask);
            Vector3 l_dirToAvoid = Vector3.zero;
            int l_trueObs = 0;
            for (int l_i = 0; l_i < l_countObs; l_i++)
            {
                var l_currObs = m_allObs[l_i];
                var l_closestPoint = l_currObs.ClosestPointOnBounds(m_origin.transform.position);
                var l_diffToPoint = l_closestPoint - m_origin.position;
                
                var l_angleToPoint = Vector3.Angle(m_origin.forward, l_diffToPoint.normalized);
                
                if(l_angleToPoint > m_viewAngle/2) continue;
                float l_dist = l_diffToPoint.magnitude;
                
                l_trueObs++;
                l_dirToAvoid += -(l_diffToPoint).normalized * (m_radius - l_dist);

            }
            
            if(l_trueObs != 0)
                l_dirToAvoid /= l_trueObs;

            return l_dirToAvoid;
        }
        
        
    }
}
