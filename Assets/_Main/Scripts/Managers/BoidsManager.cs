using System;
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
        [SerializeField] private BoidsData boidsData3D;
        [SerializeField] private BoidsData boidsData2D;
        [SerializeField] private BoidsModel boidsPrefab;

        [SerializeField,HideInInspector] private bool is2d;
        [SerializeField,HideInInspector] private Vector3 spawnCenter3d;
        [SerializeField,HideInInspector] private Vector3 spawnArea3dHalfExtent;
        [SerializeField,HideInInspector] private Vector2 spawnCenter2d;
        [SerializeField,HideInInspector] private Vector2 spawnArea2dHalfExtent;
        
        
        private PoolGeneric<BoidsModel> m_boidsPool;
        
        private Bounds m_arenaBounds;
        private Vector3 m_currSpawnCenter;
        private Vector3 m_currSpawnAreaHalfExtent;
        private BoidsData m_currBoidsData;

        private readonly List<BoidsModel> m_allBoids = new List<BoidsModel>();
        private void Awake()
        {
            
            if (is2d)
            {
                m_currBoidsData = boidsData2D;
                m_currSpawnCenter = spawnCenter2d;
                m_currSpawnAreaHalfExtent = spawnArea2dHalfExtent;
            }
            else
            {
                m_currBoidsData = boidsData3D;
                m_currSpawnCenter = spawnCenter3d;
                m_currSpawnAreaHalfExtent = spawnArea3dHalfExtent;
            }
            
            m_arenaBounds.center = m_currSpawnCenter;
            m_arenaBounds.extents = m_currSpawnAreaHalfExtent;
            
            m_boidsPool = new PoolGeneric<BoidsModel>(boidsPrefab);
            GameManager.Singleton.SetCurrentBoidsManager(this);
            
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
            if(p_boidsToSpawn<0)
                return;
            
            for (int l_i = 0; l_i < p_boidsToSpawn; l_i++)
            {
                var l_boid = m_boidsPool.GetOrCreate();
                var l_rndSpawnPoint = VectorExtentions.GetRandomRangeVector3(-m_currSpawnAreaHalfExtent, m_currSpawnAreaHalfExtent);
                var l_rndDir = Random.onUnitSphere;
                
                l_boid.gameObject.SetActive(true);
                
                l_boid.Initialize(m_currSpawnCenter + l_rndSpawnPoint, l_rndDir, m_currBoidsData);
                
                if (is2d)
                {
                    l_boid.ConstrainTo2D();
                }
                else
                {
                    l_boid.ConstrainTo3D();
                }
                
                m_allBoids.Add(l_boid);
            }   
        }

        public void SetBoidsPopulation(int p_newBoidsPopulation)
        {
            var l_boidsDiff = p_newBoidsPopulation-m_boidsPool.GetCurrentActiveCount();
            if (l_boidsDiff>0)
            {
                SpawnBoids(l_boidsDiff);
                return;
            }
            
            DespawnBoids(-l_boidsDiff);
            
        }


        public void DespawnBoids(int p_boidsToDespawn)
        {
            if(p_boidsToDespawn<0)
                return;
            
            for (int l_i = 0; l_i < p_boidsToDespawn; l_i++)
            {
                var l_lastBoidInList = m_allBoids[^1];
                l_lastBoidInList.gameObject.SetActive(false);
                m_boidsPool.AddToAvailablePool(l_lastBoidInList);
                m_allBoids.Remove(l_lastBoidInList);
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
            m_currBoidsData.SetBoidsStat(p_statsIds, p_f);
        }

        public BoidsData GetBoidsData() => m_currBoidsData;
        
        
        
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if (is2d)
            {
                Gizmos.DrawWireCube(spawnCenter2d, spawnArea2dHalfExtent*2);
            }
            else
            {
                Gizmos.DrawWireCube(spawnCenter3d, spawnArea3dHalfExtent*2);
                
            }
        }
#endif
        
    }
}