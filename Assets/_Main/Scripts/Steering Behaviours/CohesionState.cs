using _Main.Scripts.Boids;
using _Main.Scripts.SteeringData;
using UnityEngine;

namespace _Main.Scripts.Steering_Behaviours
{
    [CreateAssetMenu(fileName = "CohesionState", menuName = "main/SteeringsBh/CohesionState", order = 0)]
    public class CohesionState : SteeringDataState
    {
        public override Vector3 GetDir(BoidModel p_model)
        {
            var l_targetPoint = Vector3.zero;

            int count = 0;
            
            var l_allNeighbors = p_model.GetNeighbors();
            for (int i = 0; i < l_allNeighbors.Count; i++)
            {
                var l_neighTransform = l_allNeighbors[i].transform;
                if (l_neighTransform != p_model.transform 
                    && Vector3.Distance(l_neighTransform.position, p_model.transform.position) < p_model.GetData().CohesionRadius)
                {
                    l_targetPoint += l_neighTransform.position;
                    count++;
                } 
            }

            if (count > 0)
            {
                l_targetPoint /= count;
                return (l_targetPoint - p_model.transform.position).normalized * p_model.GetData().CohesionWeight;
            }
            return Vector3.zero;
        }
    }
}