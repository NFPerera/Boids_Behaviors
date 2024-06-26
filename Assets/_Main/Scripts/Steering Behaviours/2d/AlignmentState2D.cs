﻿using _Main.Scripts.Boids;
using _Main.Scripts.DevelopmentUtilities.Extensions;
using _Main.Scripts.Enum;
using _Main.Scripts.SteeringData;
using UnityEngine;

namespace _Main.Scripts.Steering_Behaviours._2d
{
    [CreateAssetMenu(fileName = "AlignmentState2D", menuName = "main/SteeringsBh/2D/AlignmentState2D", order = 0)]
    public class AlignmentState2D : SteeringDataState
    {
        public override Vector3 GetDir(BoidsModel p_model)
        {
            var l_averageAlignmentDirection = Vector2.zero;

            int l_count = 0;
            var l_allNeighbors = p_model.GetNeighbors();
            var l_data = p_model.GetData();
            
            for (int i = 0; i < l_allNeighbors.Count; i++)
            {
                var l_neighTransform = l_allNeighbors[i].transform;
                if ((l_neighTransform != p_model.transform)
                    && Vector2.Distance(l_neighTransform.position, p_model.transform.position) < l_data.GetStatById(BoidsStatsIds.AlignmentRadius))
                {
                    l_averageAlignmentDirection += (Vector2)l_neighTransform.forward;
                    l_count++;
                }
            }

            if (l_count > 0)
            {
                l_averageAlignmentDirection /= l_count;
                l_averageAlignmentDirection.Normalize();
            }
            return (l_averageAlignmentDirection * l_data.GetStatById(BoidsStatsIds.AlignmentWeight)).XY0();
        }
    }
}