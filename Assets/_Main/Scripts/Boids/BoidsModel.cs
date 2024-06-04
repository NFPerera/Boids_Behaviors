using System;
using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities.Extensions;
using _Main.Scripts.Enum;
using _Main.Scripts.Managers;
using UnityEngine;

namespace _Main.Scripts.Boids
{
    [RequireComponent(typeof(BoidsController))]
    [RequireComponent(typeof(BoidsView))]
    public class BoidsModel : MonoBehaviour, IBoids
    {
        private BoidsData m_data;
        
        
        private BoidsController m_controller;
        private BoidsView m_view;
        private Rigidbody m_rigidbody;
        private bool m_2dMovement;
        private RaycastHit[] m_raycastHits;
        public Vector3 WantedDir { get; private set; }

        private void Awake()
        {
            m_controller = GetComponent<BoidsController>();
            m_view = GetComponent<BoidsView>();
            m_rigidbody = GetComponent<Rigidbody>();
        }

        public void Initialize(Vector3 p_spawnPoint, Vector3 p_initDir, BoidsData p_data)
        {
            transform.position = p_spawnPoint;
            m_data = p_data;
            var l_targetRotation = Quaternion.LookRotation(p_initDir);
            transform.rotation = l_targetRotation;
            m_raycastHits = new RaycastHit[5];
        }

        private void Update()
        {
            //GameManager.Singleton.BoidsManager.CheckForBounds(this);
        }

        public BoidsData GetData() => m_data;
        public void Move(Vector3 p_dir, float p_speed)
        {
            WantedDir = p_dir;
            if(m_2dMovement)
            {
                WantedDir = WantedDir.Xyo();
            }

            var l_lerpDir = Vector3.Lerp(transform.forward, WantedDir, m_data.GetStatById(BoidsStatsIds.TurningSpeed) * Time.deltaTime);
            
            transform.position += l_lerpDir.normalized * (p_speed * Time.deltaTime);
            transform.LookAt(transform.position + l_lerpDir);
        }
        
        public void MoveWithAcceleration(Vector2 p_dir, float p_accMult)
        {
            WantedDir = p_dir.normalized;

            var l_accelerationVector = WantedDir * (m_data.GetStatById(BoidsStatsIds.AccelerationRate) * p_accMult);

            var l_velocity = m_rigidbody.velocity;
            l_velocity += l_accelerationVector * Time.deltaTime;

            
            var l_lerpDir = Vector3.Lerp(transform.forward, WantedDir, m_data.GetStatById(BoidsStatsIds.TurningSpeed) * Time.deltaTime);
            
            
            m_rigidbody.velocity = Vector2.ClampMagnitude(l_velocity, m_data.GetStatById(BoidsStatsIds.TerminalSpeed));
            transform.LookAt(transform.position + l_lerpDir);
        }

        public float GetCurrSpeedBasedOnDistance(float p_decreasePace)
        {
            var l_size = Physics.RaycastNonAlloc(transform.position, transform.forward, m_raycastHits, 
                m_data.GetStatById(BoidsStatsIds.ViewRange), m_data.ObstacleMask);
            
            if (l_size < 1)
                return m_data.GetStatById(BoidsStatsIds.MovementSpeed);

            var l_closestDistanceToObs = m_data.GetStatById(BoidsStatsIds.ViewRange);

            for (int l_i = 0; l_i < l_size; l_i++)
            {
                if (m_raycastHits[l_i].distance < l_closestDistanceToObs)
                {
                    l_closestDistanceToObs = m_raycastHits[l_i].distance;
                }
            }
            
            //return (data.TerminalVelocity)/(p_decreasePace+l_closestDistanceToObs);
            return m_data.GetStatById(BoidsStatsIds.MovementSpeed) / (1 + (float)Math.Pow(p_decreasePace * l_closestDistanceToObs, 2));
        }
        
        public void ConstrainTo2D()
        {
            m_2dMovement = true;
            var l_transform = transform;
            l_transform.position = l_transform.position.Xyo();
            var l_newRot =transform.rotation.eulerAngles.Xyo();
            transform.rotation  = Quaternion.Euler(l_newRot);

            m_rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
            
        }
        
        public void ConstrainTo3D()
        {
            m_2dMovement = false;
            m_rigidbody.constraints = RigidbodyConstraints.None;
        }

        public List<BoidsModel> GetNeighbors() => m_allNeighbors;

        private readonly List<BoidsModel> m_allNeighbors = new List<BoidsModel>();

        public void GetSelected()
        {
            m_view.ChangeToSelectedMode();
        }

        public void GetUnselected()
        {
            m_view.ChangeToUnselectedMode();
        }
        private void OnTriggerEnter(Collider p_other)
        {
            if(p_other.TryGetComponent(out BoidsModel l_model))
                m_allNeighbors.Add(l_model);
        }

        private void OnTriggerExit(Collider p_other)
        {
            if (p_other.TryGetComponent(out BoidsModel l_model))
            {
                m_allNeighbors.Remove(l_model);
            }
        }



        
    }
}