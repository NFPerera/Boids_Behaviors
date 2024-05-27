using System.Collections.Generic;
using _Main.Scripts.SteeringData;
using UnityEngine;

namespace _Main.Scripts.Boids
{
    [CreateAssetMenu(fileName = "BoidsData", menuName = "main/BoidsData", order = 0)]
    public class BoidsData : ScriptableObject
    {
        [field : SerializeField] public List<SteeringDataState> SteeringBehaviours { get; private set; } 
        [field : SerializeField] public float TurningSpeed { get; private set; } 
        [field : SerializeField] public float MovementSpeed { get; private set; } 
        [field : SerializeField] public float AccelerationRate { get; private set; } 
        [field : SerializeField] public float TerminalVelocity { get; private set; } 
        [field : SerializeField] public float ViewRange { get; private set; } 
        [field : SerializeField] public float ViewAngle { get; private set; } 
        [field : SerializeField] public float AlignmentRadius { get; private set; } 
        [field : SerializeField] public float CohesionRadius { get; private set; } 
        
        [field : SerializeField] public LayerMask ObstacleMask { get; private set; } 
        
        [field : Header("Steering Weight")]
        
        [field : SerializeField] public float ObsAvoidanceWeight { get; private set; } 
        [field : SerializeField] public float AlignmentWeight { get; private set; } 
        [field : SerializeField] public float CohesionWeight { get; private set; }


        public void SetMovementSpeed(float p_f) => MovementSpeed = p_f;
        public void SetViewRange(float p_f) => ViewRange = p_f;
        public void SetViewAngle(float p_f) => ViewAngle = p_f;
        public void SetAlignmentRadius(float p_f) => AlignmentRadius = p_f;
        public void SetCohesionRadius(float p_f) => CohesionRadius = p_f;
        
        public void SetObsAvoidanceWeight(float p_f) => ObsAvoidanceWeight = p_f;
        public void SetAlignmentWeight(float p_f) => AlignmentWeight = p_f;
        public void SetCohesionWeight(float p_f) => CohesionWeight = p_f;
        
    }
}