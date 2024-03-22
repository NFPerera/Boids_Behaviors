using System;
using System.Collections.Generic;
using _Main.Scripts.Managers;
using _Main.Scripts.Steering_Behaviours;
using _Main.Scripts.SteeringData;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Main.Scripts.Boids
{
    public class BoidController : MonoBehaviour
    {
        private BoidModel p_model;

        
        private void Start()
        {
            p_model = GetComponent<BoidModel>();
        }

        private void Update()
        {
            var l_wantedDir = Vector3.zero;
            for (int i = 0; i < p_model.GetData().SteeringBehaviours.Count; i++)
            {
                l_wantedDir += p_model.GetData().SteeringBehaviours[i].GetDir(p_model);
            }
            
            p_model.Move((gameObject.transform.forward + l_wantedDir), p_model.GetData().MovementSpeed);
        }

        //public void SetSteeringBh(SteeringDataState p_steeringDataState) =>
        //    steeringBehaviours = p_steeringDataState;

    }
}