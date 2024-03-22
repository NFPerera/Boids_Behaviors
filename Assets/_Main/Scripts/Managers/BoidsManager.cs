using System;
using System.Collections.Generic;
using _Main.Scripts.Boids;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.Steering_Behaviours;
using _Main.Scripts.SteeringData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Main.Scripts.Managers
{
    public class BoidsManager : MonoBehaviour
    {
        [SerializeField] private BoidManagerData data;
        [SerializeField] private BoidData boidData;
        [SerializeField] private BoidModel boidPrefab;
        [SerializeField] private bool is2D;
        [SerializeField] private Vector3 spawnCenter;
        [SerializeField] private Vector3 spawnAreaHalfExtent;
        [SerializeField] private int boidsToSpawn;
        public static BoidsManager Singleton;
        
        
        private Bounds m_arenaBounds;

        private Dictionary<SteeringsId, SteeringDataState> m_allSteeringDataStates = new Dictionary<SteeringsId, SteeringDataState>();
        private List<BoidModel> m_allBoids = new List<BoidModel>();
        private void Awake()
        {
            if (Singleton != default)
            {
                Destroy(this);
                return;
            }

            Singleton = this;
            DontDestroyOnLoad(this);

            m_arenaBounds.center = spawnCenter;
            m_arenaBounds.extents = spawnAreaHalfExtent;
            
            m_allSteeringDataStates.Add(SteeringsId.Line, data.lineState);
            m_allSteeringDataStates.Add(SteeringsId.ObstacleAvoidance, data.obstacleAvoidanceState);
            m_allSteeringDataStates.Add(SteeringsId.Cohesion, data.cohesionState);
            m_allSteeringDataStates.Add(SteeringsId.Alignment, data.alignmentState);
        }

        private void Start()
        {
            SpawnBoids(boidsToSpawn);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeAllBoidsSteeringBh(SteeringsId.Line);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeAllBoidsSteeringBh(SteeringsId.ObstacleAvoidance);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChangeAllBoidsSteeringBh(SteeringsId.Alignment);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ChangeAllBoidsSteeringBh(SteeringsId.Cohesion);
            }
        }

        public void CheckForBounds(BoidModel p_model)
        {
            var boidPos = p_model.gameObject.transform.position;
            if(m_arenaBounds.Contains(boidPos))
                return;

            p_model.gameObject.transform.position = -m_arenaBounds.ClosestPoint(boidPos);
        }

        private void SpawnBoids(int p_boidsToSpawn)
        {
            for (int i = 0; i < p_boidsToSpawn; i++)
            {
                var l_boid = Instantiate(boidPrefab);
                Vector3 l_rndSpawnPoint;
                Vector3 l_rndDir;
                if (is2D)
                {
                    l_rndSpawnPoint = VectorExtentions.GetRandomRangeVector2(-spawnAreaHalfExtent, spawnAreaHalfExtent);
                    l_rndDir = VectorExtentions.GetRandomDirVector2();
                    l_boid.ConstrainTo2D();
                }
                else
                {
                    l_rndSpawnPoint = VectorExtentions.GetRandomRangeVector3(-spawnAreaHalfExtent, spawnAreaHalfExtent);
                    l_rndDir = Random.onUnitSphere;
                    l_boid.ConstrainTo3D();
                }
                
                
                l_boid.Initialize(spawnCenter + l_rndSpawnPoint, l_rndDir, SteeringsId.Line);
                
                m_allBoids.Add(l_boid);
            }   
        }

        public void ChangeAllBoidsSteeringBh(SteeringsId p_id)
        {
            for (int i = 0; i < m_allBoids.Count; i++)
            {
                //m_allBoids[i].ChangeSteeringBehaviour(p_id);
            }
        }
        public SteeringDataState GetSteeringDataStateById(SteeringsId p_id) => m_allSteeringDataStates[p_id];

        public void SetBoidsSpeed(float p_f) => boidData.SetMovementSpeed(p_f);
        public void SetBoidViewRange(float p_f) => boidData.SetViewRange(p_f);
        public void SetBoidViewAngle(float p_f) => boidData.SetViewAngle(p_f);
        
        public void SetBoidObsAvoid(float p_f) => boidData.SetObsAvoidanceWeight(p_f);
        public void SetBoidCohesion(float p_f) => boidData.SetCohesionWeight(p_f);
        public void SetBoidAlignment(float p_f) => boidData.SetAlignmentWeight(p_f);
        public void SetBoidAlignmentRadius(float p_f) => boidData.SetAlignmentRadius(p_f);
        public void SetBoidCohesionRadius(float p_f) => boidData.SetCohesionRadius(p_f);
        
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(spawnCenter, spawnAreaHalfExtent*2);
        }
#endif
        
    }
}