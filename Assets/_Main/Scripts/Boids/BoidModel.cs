using System;
using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.Managers;
using _Main.Scripts.SteeringData;
using UnityEngine;

namespace _Main.Scripts.Boids
{
    public class BoidModel : MonoBehaviour, IBoid
    {
        [SerializeField] private BoidData data;
        
        
        private BoidController m_controller;
        private Rigidbody m_rigidbody;
        private bool m_2dMovement;


        private void Awake()
        {
            m_controller = GetComponent<BoidController>();
            m_rigidbody = GetComponent<Rigidbody>();
        }

        public void Initialize(Vector3 p_spawnPoint, Vector3 p_initDir, SteeringsId p_initSteeringBh)
        {
            transform.position = p_spawnPoint;
            
            Quaternion targetRotation = Quaternion.LookRotation(p_initDir);
            transform.rotation = targetRotation;
            ChangeSteeringBehaviour(p_initSteeringBh);
        }

        private void Update()
        {
            BoidsManager.Singleton.CheckForBounds(this);
        }

        public BoidData GetData() => data;
        public void Move(Vector3 p_dir, float p_speed)
        {
            if(m_2dMovement)
            {
                p_dir = p_dir.Xyo();
            }
            transform.position += p_dir.normalized * (p_speed * Time.deltaTime);
            transform.LookAt(transform.position + p_dir);
        }

        public void ChangeSteeringBehaviour(SteeringsId p_id) =>
            m_controller.SetSteeringBh(BoidsManager.Singleton.GetSteeringDataStateById(p_id));

        public void ConstrainTo2D()
        {
            m_2dMovement = true;
            transform.position = (Vector2)transform.position;
            transform.Rotate(Vector3.forward, 0f);
            
            m_rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
            m_rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
        }
        
        public void ConstrainTo3D()
        {
            m_2dMovement = false;
            m_rigidbody.constraints = RigidbodyConstraints.None;
        }

        public Collider[] GetNeighbors()
        {
            return Physics.OverlapSphere(transform.position, data.ViewRange, gameObject.layer);
            
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, data.ViewRange);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward*5);
        }
#endif
        
    }
}