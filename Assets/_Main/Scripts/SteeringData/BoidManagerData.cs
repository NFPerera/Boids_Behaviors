using UnityEngine;
using UnityEngine.Serialization;

namespace _Main.Scripts.SteeringData
{
    [CreateAssetMenu(fileName = "BoidManagerData", menuName = "main/BoidManagerData", order = 0)]
    public class BoidManagerData : ScriptableObject
    {
        [field: SerializeField] public SteeringDataState ObstacleAvoidanceState{ get; private set;}
        [field: SerializeField] public SteeringDataState AlignmentState{ get; private set;}
        [field: SerializeField] public SteeringDataState CohesionState{ get; private set;}
    }
}