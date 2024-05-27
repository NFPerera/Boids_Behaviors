using UnityEngine;
using UnityEngine.Serialization;

namespace _Main.Scripts.SteeringData
{
    [CreateAssetMenu(fileName = "BoidsManagerData", menuName = "main/BoidsManagerData", order = 0)]
    public class BoidsManagerData : ScriptableObject
    {
        [field: SerializeField] public SteeringDataState ObstacleAvoidanceState{ get; private set;}
        [field: SerializeField] public SteeringDataState AlignmentState{ get; private set;}
        [field: SerializeField] public SteeringDataState CohesionState{ get; private set;}
    }
}