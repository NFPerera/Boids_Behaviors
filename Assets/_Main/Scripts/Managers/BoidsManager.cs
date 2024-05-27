using System.Collections.Generic;
using _Main.Scripts.Boids;
using _Main.Scripts.DevelopmentUtilities.Extensions;
using _Main.Scripts.Enum;
using _Main.Scripts.SteeringData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Main.Scripts.Managers
{
    public class BoidsManager : MonoBehaviour
    {
        [SerializeField] private BoidsManagerData data;
        [SerializeField] private BoidsData boidsData;
        [SerializeField] private BoidsModel boidsPrefab;
        [SerializeField] private bool is2D;
        [SerializeField] private Vector3 spawnCenter;
        [SerializeField] private Vector3 spawnAreaHalfExtent;
        [SerializeField] private int boidsToSpawn;
        public static BoidsManager Singleton;
        
        
        private Bounds m_arenaBounds;

        private readonly Dictionary<SteeringsId, SteeringDataState> m_allSteeringDataStates = new Dictionary<SteeringsId, SteeringDataState>();
        private readonly List<BoidsModel> m_allBoids = new List<BoidsModel>();
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
            
            m_allSteeringDataStates.Add(SteeringsId.ObstacleAvoidance, data.ObstacleAvoidanceState);
            m_allSteeringDataStates.Add(SteeringsId.Cohesion, data.CohesionState);
            m_allSteeringDataStates.Add(SteeringsId.Alignment, data.AlignmentState);
        }

        private void Start()
        {
            SpawnBoids(boidsToSpawn);
        }

        public void CheckForBounds(BoidsModel p_model)
        {
            var l_boidPos = p_model.gameObject.transform.position;
            if(m_arenaBounds.Contains(l_boidPos))
                return;

            p_model.gameObject.transform.position = -m_arenaBounds.ClosestPoint(l_boidPos);
        }

        private void SpawnBoids(int p_boidsToSpawn)
        {
            for (int l_i = 0; l_i < p_boidsToSpawn; l_i++)
            {
                var l_boid = Instantiate(boidsPrefab);
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
                
                
                l_boid.Initialize(spawnCenter + l_rndSpawnPoint, l_rndDir);
                
                m_allBoids.Add(l_boid);
            }   
        }

        public void ConstrainBoidsTo2D()
        {
            foreach (var l_boid in m_allBoids)
            {
                l_boid.ConstrainTo2D();
            }
        }

        public void ConstrainBoidsTo3D()
        {
            foreach (var l_boid in m_allBoids)
            {
                l_boid.ConstrainTo3D();
            }
        }

        public void SetBoidsStats(BoidsStatsIds p_statsIds, float p_f)
        {
            boidsData.SetBoidsStat(p_statsIds, p_f);
        }

        public BoidsData GetBoidsData() => boidsData;
        
        
        
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(spawnCenter, spawnAreaHalfExtent*2);
        }
#endif
        
    }
}