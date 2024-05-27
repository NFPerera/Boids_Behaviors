using _Main.Scripts.Boids;
using _Main.Scripts.SteeringData;
using UnityEngine;

namespace _Main.Scripts.Steering_Behaviours
{
    [CreateAssetMenu(fileName = "AlignmentState", menuName = "main/SteeringsBh/AlignmentState", order = 0)]
    public class AlignmentState : SteeringDataState
    {
        public override Vector3 GetDir(BoidsesModel p_model)
        {
            var l_averageAlignmentDirection = Vector3.zero;

            int l_count = 0;
            var l_allNeighbors = p_model.GetNeighbors();
            for (int i = 0; i < l_allNeighbors.Count; i++)
            {
                var l_neighTransform = l_allNeighbors[i].transform;
                if ((l_neighTransform != p_model.transform)
                    && Vector3.Distance(l_neighTransform.position, p_model.transform.position) < p_model.GetData().AlignmentRadius)
                {
                    l_averageAlignmentDirection += l_neighTransform.forward;
                    l_count++;
                }
            }

            if (l_count > 0)
            {
                l_averageAlignmentDirection /= l_count;
                l_averageAlignmentDirection.Normalize();
            }
            return l_averageAlignmentDirection * p_model.GetData().AlignmentWeight;
        }
    }
}