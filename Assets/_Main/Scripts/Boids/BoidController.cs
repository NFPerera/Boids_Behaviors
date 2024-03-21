using System;
using _Main.Scripts.Managers;
using _Main.Scripts.Steering_Behaviours;
using _Main.Scripts.SteeringData;
using UnityEngine;

namespace _Main.Scripts.Boids
{
    public class BoidController : MonoBehaviour
    {
        private BoidModel p_model;

        private SteeringDataState m_currSteeringBehaviour;

        private void Start()
        {
            p_model = GetComponent<BoidModel>();
            m_currSteeringBehaviour = BoidsManager.Singleton.GetSteeringDataStateById(SteeringsId.Line);
        }

        private void Update()
        {
            var dir = m_currSteeringBehaviour.GetDir(p_model);
            p_model.Move((gameObject.transform.forward + dir), p_model.GetData().MovementSpeed);
        }

        public void SetSteeringBh(SteeringDataState p_steeringDataState) =>
            m_currSteeringBehaviour = p_steeringDataState;

    }
}