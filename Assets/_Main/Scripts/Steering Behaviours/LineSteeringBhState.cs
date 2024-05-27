using _Main.Scripts.Boids;
using _Main.Scripts.SteeringData;
using UnityEngine;

namespace _Main.Scripts.Steering_Behaviours
{
    [CreateAssetMenu(fileName = "LineSteeringBhState", menuName = "main/SteeringsBh/LineSteeringBhState", order = 0)]
    public class LineSteeringBhState : SteeringDataState
    {
        public override Vector3 GetDir(BoidsesModel p_model)
        {
            return p_model.transform.forward.normalized;
        }
    }
}