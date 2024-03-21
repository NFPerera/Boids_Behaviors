using System;
using System.Collections.Generic;
using _Main.Scripts.Boids;
using _Main.Scripts.DevelopmentUtilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Main.Scripts
{
    public class BoidsManager : MonoBehaviour
    {
        [SerializeField] private BoidModel boidPrefab;
        [SerializeField] private Vector3 spawnCenter;
        [SerializeField] private Vector3 spawnAreaHalfExtent;
        [SerializeField] private int boidsToSpawn;
        public static BoidsManager Singleton;
        
        
        private Bounds m_arenaBounds;
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
        }

        private void Start()
        {
            SpawnBoids(boidsToSpawn);
        }

        private void Update()
        {
            

            for (int i = 0; i < m_allBoids.Count; i++)
            {
                var l_boid = m_allBoids[i];
                var boidPos = l_boid.gameObject.transform.position;
                Debug.Log($"Chequeando {m_arenaBounds.Contains(boidPos)}");
                if(m_arenaBounds.Contains(boidPos))
                    return;

                Debug.Log($"SE PASO EN {m_arenaBounds.ClosestPoint(boidPos)}");
                l_boid.gameObject.transform.position = -m_arenaBounds.ClosestPoint(boidPos);
                Debug.Log($"Lo tepeo a {l_boid.gameObject.transform.position}");
            }
        }

        private void SpawnBoids(int p_boidsToSpawn)
        {
            for (int i = 0; i < p_boidsToSpawn; i++)
            {
                var l_boid = Instantiate(boidPrefab);

                var l_rndSpawnPoint = VectorExtentions.GetRandomRangeVector3(-spawnAreaHalfExtent, spawnAreaHalfExtent);
                var l_rndDir = Random.onUnitSphere;
                
                l_boid.Initialize(spawnCenter + l_rndSpawnPoint, l_rndDir);
                
                m_allBoids.Add(l_boid);
            }   
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(spawnCenter, spawnAreaHalfExtent*2);
        }
    }
}