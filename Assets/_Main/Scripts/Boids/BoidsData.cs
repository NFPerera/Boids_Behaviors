using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities.DictionaryUtilities;
using _Main.Scripts.Enum;
using _Main.Scripts.SteeringData;
using UnityEngine;

namespace _Main.Scripts.Boids
{
    [CreateAssetMenu(fileName = "BoidsData", menuName = "main/BoidsData", order = 0)]
    public class BoidsData : ScriptableObject
    {
        [field : SerializeField] public List<SteeringDataState> SteeringBehaviours { get; private set; } 
        
        [field: SerializeField] public SerializableDictionary<BoidsStatsIds, float> BaseBoidsStats { get; private set; } 
        [field: SerializeField] public SerializableDictionary<BoidsStatsIds, float> CurrBoidsStats { get; private set; }
        [field: SerializeField] public LayerMask ObstacleMask { get; private set; }

        public void ResetCurrBoidsStats() => CurrBoidsStats = BaseBoidsStats;
        public void SetBoidsStat(BoidsStatsIds p_statsIds, float p_f) => CurrBoidsStats[p_statsIds] = p_f;

        public float GetStatById(BoidsStatsIds p_statsIds) => CurrBoidsStats[p_statsIds];

    }
}