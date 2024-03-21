using _Main.Scripts.Boids;
using UnityEngine;

namespace _Main.Scripts.Steering_Behaviours
{
    public class ChaseSbState: SteeringDataState
    {
        

        public override Vector3 GetDir(BoidModel p_model)
        {
            return (m_target.position - m_origin.position).normalized;
        }
    }
}