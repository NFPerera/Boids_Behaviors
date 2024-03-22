using UnityEngine;
using UnityEngine.Serialization;

namespace _Main.Scripts.SteeringData
{
    [CreateAssetMenu(fileName = "BoidManagerData", menuName = "main/BoidManagerData", order = 0)]
    public class BoidManagerData : ScriptableObject
    {
        [field: SerializeField] public SteeringDataState lineState;
        [field: SerializeField] public SteeringDataState obstacleAvoidanceState;
        [field: SerializeField] public SteeringDataState alignmentState;
        [field: SerializeField] public SteeringDataState cohesionState;
    }
}