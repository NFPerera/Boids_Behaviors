using _Main.Scripts.Boids;
using _Main.Scripts.Enum;
using _Main.Scripts.SteeringData;
using UnityEngine;

namespace _Main.Scripts.Steering_Behaviours._3d
{
    [CreateAssetMenu(fileName = "CohesionState3D", menuName = "main/SteeringsBh/3D/CohesionState3D", order = 0)]
    public class CohesionState3D : SteeringDataState
    {
        public override Vector3 GetDir(BoidsModel p_model)
        {
            var l_targetPoint = Vector3.zero;
            var l_allNeighbors = p_model.GetNeighbors();
            int l_count = 0;

            var l_data = p_model.GetData();
            
            for (int l_i = 0; l_i < l_allNeighbors.Count; l_i++)
            {
                var l_neighTransform = l_allNeighbors[l_i].transform;
                
                if (l_neighTransform != p_model.transform 
                    && Vector3.Distance(l_neighTransform.position, p_model.transform.position) < l_data.GetStatById(BoidsStatsIds.CohesionRadius))
                {
                    l_targetPoint += l_neighTransform.position;
                    l_count++;
                } 
            }

            if (l_count > 0)
            {
                l_targetPoint /= l_count;
                return (l_targetPoint - p_model.transform.position).normalized * l_data.GetStatById(BoidsStatsIds.CohesionWeight);
            }
            return Vector3.zero;
        }
    }
}