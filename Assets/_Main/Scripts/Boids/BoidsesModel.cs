﻿using System;
using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities.Extensions;
using _Main.Scripts.Managers;
using UnityEngine;

namespace _Main.Scripts.Boids
{
    [RequireComponent(typeof(BoidsController))]
    [RequireComponent(typeof(BoidsView))]
    public class BoidsesModel : MonoBehaviour, IBoids
    {
        [SerializeField] private BoidsData data;
        
        
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

        public void Initialize(Vector3 p_spawnPoint, Vector3 p_initDir)
        {
            transform.position = p_spawnPoint;
            
            var l_targetRotation = Quaternion.LookRotation(p_initDir);
            transform.rotation = l_targetRotation;
            m_raycastHits = new RaycastHit[5];
        }

        private void Update()
        {
            BoidsManager.Singleton.CheckForBounds(this);
        }

        public BoidsData GetData() => data;
        public void Move(Vector3 p_dir, float p_speed)
        {
            WantedDir = p_dir;
            if(m_2dMovement)
            {
                WantedDir = WantedDir.Xyo();
            }

            var l_lerpDir = Vector3.Lerp(transform.forward, WantedDir, data.TurningSpeed * Time.deltaTime);
            
            transform.position += l_lerpDir.normalized * (p_speed * Time.deltaTime);
            transform.LookAt(transform.position + l_lerpDir);
        }
        
        public void MoveWithAcceleration(Vector2 p_dir, float p_accMult)
        {
            WantedDir = p_dir.normalized;

            var l_accelerationVector = WantedDir * (data.AccelerationRate * p_accMult);

            var l_velocity = m_rigidbody.velocity;
            l_velocity += l_accelerationVector * Time.deltaTime;

            
            var l_lerpDir = Vector3.Lerp(transform.forward, WantedDir, data.TurningSpeed * Time.deltaTime);
            
            
            m_rigidbody.velocity = Vector2.ClampMagnitude(l_velocity, data.TerminalVelocity);
            transform.LookAt(transform.position + l_lerpDir);
        }

        public float GetCurrSpeedBasedOnDistance(float p_decreasePace)
        {
            var l_size = Physics.RaycastNonAlloc(transform.position, transform.forward, m_raycastHits, data.ViewRange, data.ObstacleMask);
            
            if (l_size < 1)
                return data.MovementSpeed;

            var l_closestDistanceToObs = data.ViewRange;

            for (int l_i = 0; l_i < l_size; l_i++)
            {
                if (m_raycastHits[l_i].distance < l_closestDistanceToObs)
                {
                    l_closestDistanceToObs = m_raycastHits[l_i].distance;
                }
            }
            
            //return (data.TerminalVelocity)/(p_decreasePace+l_closestDistanceToObs);
            return data.MovementSpeed / (1 + (float)Math.Pow(p_decreasePace * l_closestDistanceToObs, 2));
        }
        
        public void ConstrainTo2D()
        {
            m_2dMovement = true;
            var l_transform = transform;
            l_transform.position = (Vector2)l_transform.position;
            transform.Rotate(Vector3.forward, 0f);
            
            m_rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
            m_rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
        }
        
        public void ConstrainTo3D()
        {
            m_2dMovement = false;
            m_rigidbody.constraints = RigidbodyConstraints.None;
        }

        public List<BoidsesModel> GetNeighbors() => m_allNeighbors;

        private readonly List<BoidsesModel> m_allNeighbors = new List<BoidsesModel>();

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
            if(p_other.TryGetComponent(out BoidsesModel l_model))
                m_allNeighbors.Add(l_model);
        }

        private void OnTriggerExit(Collider p_other)
        {
            if (p_other.TryGetComponent(out BoidsesModel l_model))
            {
                m_allNeighbors.Remove(l_model);
            }
        }



        
    }
}