using System;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Boids;
using _Main.Scripts.DevelopmentUtilities.Extensions;
using _Main.Scripts.Enum;
using _Main.Scripts.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Main.Scripts.Managers
{
    public class BoidsManager : MonoBehaviour
    {
        [SerializeField] private BoidsModel boidsPrefab;

        [SerializeField,HideInInspector] private bool is2d;
        [SerializeField,HideInInspector] private Vector3 spawnCenter3d;
        [SerializeField,HideInInspector] private Vector3 spawnArea3dHalfExtent;
        [SerializeField,HideInInspector] private Vector2 spawnCenter2d;
        [SerializeField,HideInInspector] private Vector2 spawnArea2dHalfExtent;
        [SerializeField] private int boidsToSpawn;
        [SerializeField] private List<FlockData> allFlocksData;
        
        private PoolGeneric<BoidsModel> m_boidsPool;
        private List<FlockData> m_activeFlocksData=new List<FlockData>();
        private Vector3 m_currSpawnCenter;
        private Vector3 m_currSpawnAreaHalfExtent;
        private TimerExtensions m_myTimer;

        
        private readonly List<BoidsModel> m_allBoids = new List<BoidsModel>();
        private void Awake()
        {
            
            if (is2d)
            {
                m_currSpawnCenter = spawnCenter2d+(Vector2)transform.position;
                m_currSpawnAreaHalfExtent = spawnArea2dHalfExtent;
            }
            else
            {
                m_currSpawnCenter = spawnCenter3d+transform.position;
                m_currSpawnAreaHalfExtent = spawnArea3dHalfExtent;
            }
            
            
            m_myTimer = new TimerExtensions(5f, OnTimerComplete);
            m_myTimer.Start();
            
            m_boidsPool = new PoolGeneric<BoidsModel>(boidsPrefab);
            GameManager.Singleton.SetCurrentBoidsManager(this);
            m_activeFlocksData.Add(allFlocksData[0]);
        }

        private void Start()
        {
            SpawnBoids(boidsToSpawn);
        }

        private void OnDestroy()
        {
            if (m_myTimer != null)
            {
                m_myTimer.Stop();
                m_myTimer = null;
            }
        }

        public List<FlockData> GetAllFlockData() => allFlocksData;

        private void OnTimerComplete()
        {
            foreach (var l_model in m_allBoids)
            {
                if(is2d)
                {
                    l_model.transform.position = l_model.transform.position.Xyo();
                }
                
                if(CheckIfIsInBounds(l_model.transform.position))
                    continue;
                
                l_model.transform.position=Vector3.zero;
            }
            m_myTimer.Reset();
            m_myTimer.Start();
        }

        private void Update()
        {
            if(m_myTimer==null)
                return;
            
            
            if (m_myTimer.IsRunning())
            {
                m_myTimer.Update(Time.deltaTime);
            }
        }


        private void SpawnBoids(int p_boidsToSpawn)
        {
            if(p_boidsToSpawn<0)
                return;
            
            for (int l_i = 0; l_i < p_boidsToSpawn; l_i++)
            {
                var l_boid = m_boidsPool.GetOrCreate();
                
                Vector3 l_rndSpawnPoint;
                Vector3 l_rndDir;
                
                l_boid.gameObject.SetActive(true);
                
                if (is2d)
                {
                    l_rndSpawnPoint = VectorExtentions.GetRandomRangeVector2(-m_currSpawnAreaHalfExtent, m_currSpawnAreaHalfExtent);
                    l_rndDir = Random.insideUnitCircle;
                    l_boid.ConstrainTo2D();
                    
                }
                else
                {
                    l_rndSpawnPoint = VectorExtentions.GetRandomRangeVector3(-m_currSpawnAreaHalfExtent, m_currSpawnAreaHalfExtent);
                    l_rndDir = Random.onUnitSphere;
                    l_boid.ConstrainTo3D();
                }
                
                m_allBoids.Add(l_boid);
                
                l_boid.Initialize(m_currSpawnCenter + l_rndSpawnPoint, l_rndDir, GetNextFlockData(),is2d);
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
        
        

        private bool CheckIfIsInBounds(Vector3 p_pos)
        {
            return (p_pos.x >= m_currSpawnCenter.x - m_currSpawnAreaHalfExtent.x && p_pos.x <= m_currSpawnCenter.x + m_currSpawnAreaHalfExtent.x) &&
                   (p_pos.y >= m_currSpawnCenter.y - m_currSpawnAreaHalfExtent.y && p_pos.y <= m_currSpawnCenter.y + m_currSpawnAreaHalfExtent.y) &&
                   (p_pos.z >= m_currSpawnCenter.z - m_currSpawnAreaHalfExtent.z && p_pos.z <= m_currSpawnCenter.z + m_currSpawnAreaHalfExtent.z);
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




        public void AddNewFlock()
        {
            if (m_activeFlocksData.Count > allFlocksData.Count)
            {
                Debug.LogError("Error while adding new flock to current simulation, index out of data range");
                return;
            }
            m_activeFlocksData.Add(allFlocksData[m_activeFlocksData.Count]);
        }

        public void RemoveFlock()
        {
            if (m_activeFlocksData.Count < 1)
            {
                Debug.LogError("Error while removing flock to current simulation, index bellow 0");
                return;
            }
            m_activeFlocksData.GetLastElement().BoidsData.ResetCurrBoidsStats();
            m_activeFlocksData.RemoveLast();
        }
        public void UpdateBoidsFlock(int p_flockAmount)
        {
            //for, i %% p_flockAmount = id de tu nueva flock
            //  flockData, mesh, default mat, selected mat, statsdata
            if (p_flockAmount > m_activeFlocksData.Count)
            {
                Debug.LogError("Flock amount request exceeded current flock capacity");
                return;
            }
            
            for (int i = 0; i < m_allBoids.Count; i++)
            {
                var l_flockData = m_activeFlocksData[i % p_flockAmount];
                var l_boid = m_allBoids[i];
                l_boid.ChangeFlock(l_flockData);
            }
        }

        private FlockData GetNextFlockData() => allFlocksData[m_allBoids.Count % m_activeFlocksData.Count];
        public void SetBoidsStats(int p_flockId,BoidsStatsIds p_statsIds, float p_f)
        {
            m_activeFlocksData[p_flockId].BoidsData.SetBoidsStat(p_statsIds, p_f);
        }
        public BoidsData GetBoidsDataByFlockId(int p_flockId) => m_activeFlocksData[p_flockId].BoidsData;
        
        
        
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if (is2d)
            {
                Gizmos.DrawWireCube(spawnCenter2d+(Vector2)transform.position, spawnArea2dHalfExtent*2);
            }
            else
            {
                Gizmos.DrawWireCube(spawnCenter3d+transform.position, spawnArea3dHalfExtent*2);
                
            }
        }
#endif
        
    }
}