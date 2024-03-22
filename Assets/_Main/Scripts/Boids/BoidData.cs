using UnityEngine;

namespace _Main.Scripts.Boids
{
    [CreateAssetMenu(fileName = "BoidData", menuName = "main/BoidData", order = 0)]
    public class BoidData : ScriptableObject
    {
        [field : SerializeField] public float MovementSpeed { get; private set; } 
        [field : SerializeField] public float ViewRange { get; private set; } 
        [field : SerializeField] public float AlignmentRadius { get; private set; } 
        [field : SerializeField] public float CohesionRadius { get; private set; } 
        [field : SerializeField] public float ViewAngle { get; private set; } 
        [field : SerializeField] public LayerMask ObstacleMask { get; private set; } 
        
        [field : Header("Steering Weight")]
        
        [field : SerializeField] public float ObsAvoidanceWeight { get; private set; } 
        [field : SerializeField] public float AlignmentWeight { get; private set; } 
        [field : SerializeField] public float CohesionWeight { get; private set; } 
    }
}