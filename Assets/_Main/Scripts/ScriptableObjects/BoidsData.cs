using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities.DictionaryUtilities;
using _Main.Scripts.Enum;
using _Main.Scripts.SteeringData;
using UnityEngine;

namespace _Main.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BoidsData", menuName = "main/BoidsData", order = 0)]
    public class BoidsData : ScriptableObject
    {
        [field : SerializeField] public List<SteeringDataState> SteeringBehaviours3D { get; private set; } 
        [field : SerializeField] public List<SteeringDataState> SteeringBehaviours2D { get; private set; } 
        
        [field: SerializeField] public SerializableDictionary<BoidsStatsIds, float> MinBoidsStats { get; private set; } 
        [field: SerializeField] public SerializableDictionary<BoidsStatsIds, float> DefaultBoidsStats { get; private set; } 
        [field: SerializeField] public SerializableDictionary<BoidsStatsIds, float> CurrBoidsStats { get; private set; }
        [field: SerializeField] public SerializableDictionary<BoidsStatsIds, float> MaxBoidsStats { get; private set; }
        [field: SerializeField] public LayerMask ObstacleMask { get; private set; }

        public void ResetCurrBoidsStats() => CurrBoidsStats = DefaultBoidsStats;
        public void SetBoidsStat(BoidsStatsIds p_statsIds, float p_f)
        {
            CurrBoidsStats[p_statsIds] = p_f;
            CheckUpgradeIsInBounds(p_statsIds);
        }

        public float GetStatById(BoidsStatsIds p_statsIds) => CurrBoidsStats[p_statsIds];
        
        
        private void CheckUpgradeIsInBounds(BoidsStatsIds p_statsId)
        {
            if (CurrBoidsStats[p_statsId] < MinBoidsStats[p_statsId])
            {
                CurrBoidsStats[p_statsId] = MinBoidsStats[p_statsId];
            }
            
            if (CurrBoidsStats[p_statsId] > MaxBoidsStats[p_statsId])
                CurrBoidsStats[p_statsId] = MaxBoidsStats[p_statsId];
        }

    }
}