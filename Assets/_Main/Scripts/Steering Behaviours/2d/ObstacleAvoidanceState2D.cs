using _Main.Scripts.Boids;
using _Main.Scripts.Enum;
using _Main.Scripts.SteeringData;
using UnityEngine;

namespace _Main.Scripts.Steering_Behaviours._2d
{
    [CreateAssetMenu(fileName = "ObstacleAvoidanceState2D", menuName = "main/SteeringsBh/2D/ObstacleAvoidanceState2D", order = 0)]
    public class ObstacleAvoidanceState2D: SteeringDataState
    {
        public override Vector3 GetDir(BoidsModel p_model)
        {
            var l_data = p_model.GetData();
            var l_modelTransform = p_model.transform;
            var l_modelPos = l_modelTransform.position;
            
            var l_allObs = Physics2D.OverlapCircleAll(l_modelPos, l_data.GetStatById(BoidsStatsIds.ViewRange), l_data.ObstacleMask);
            var l_dirToAvoid = Vector2.zero;
            int l_trueObs = 0;
            for (int l_i = 0; l_i < l_allObs.Length; l_i++)
            {
                
                var l_currObs = l_allObs[l_i];
                var l_closestPoint = l_currObs.ClosestPoint(l_modelPos);
                var l_diffToPoint = l_closestPoint - (Vector2)l_modelPos;
                
                var l_angleToPoint = Vector3.Angle(l_modelTransform.forward, l_diffToPoint.normalized);
                
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