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
        [SerializeField] private bool is2D;
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
                }
                else
                {
                    l_rndSpawnPoint = VectorExtentions.GetRandomRangeVector3(-spawnAreaHalfExtent, spawnAreaHalfExtent);
                    l_rndDir = Random.onUnitSphere;
                }
                
                
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