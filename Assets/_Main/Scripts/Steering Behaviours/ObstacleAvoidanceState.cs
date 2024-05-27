using _Main.Scripts.Boids;
using _Main.Scripts.Enum;
using _Main.Scripts.SteeringData;
using UnityEngine;

namespace _Main.Scripts.Steering_Behaviours
{
    [CreateAssetMenu(fileName = "ObstacleAvoidanceState", menuName = "main/SteeringsBh/ObstacleAvoidanceState", order = 0)]
    public class ObstacleAvoidanceState: SteeringDataState
    {

        public override Vector3 GetDir(BoidsModel p_model)
        {
            var l_data = p_model.GetData();
            var l_allObs = Physics.OverlapSphere(p_model.transform.position, l_data.GetStatById(BoidsStatsIds.ViewRange), l_data.ObstacleMask);
            
            Vector3 l_dirToAvoid = Vector3.zero;
            int l_trueObs = 0;
            for (int l_i = 0; l_i < l_allObs.Length; l_i++)
            {
                var l_currObs = l_allObs[l_i];
                var l_closestPoint = l_currObs.ClosestPointOnBounds(p_model.transform.position);
                var l_diffToPoint = l_closestPoint - p_model.transform.position;
                
                var l_angleToPoint = Vector3.Angle(p_model.transform.forward, l_diffToPoint.normalized);
                
                if(l_angleToPoint > l_data.GetStatById(BoidsStatsIds.ViewAngle)/2) continue;
                float l_dist = l_diffToPoint.magnitude;
                
                l_trueObs++;
                l_dirToAvoid += -(l_diffToPoint).normalized * (l_data.GetStatById(BoidsStatsIds.ViewRange) - l_dist);

            }
            
            if(l_trueObs != 0)
                l_dirToAvoid /= l_trueObs;

            return l_dirToAvoid * l_data.GetStatById(BoidsStatsIds.ObsAvoidanceWeight);
        }
        
    }
}