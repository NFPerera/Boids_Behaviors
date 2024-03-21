using UnityEngine;

namespace _Main.Scripts.Steering_Behaviours
{
    public class ZeroSteering : ISteeringBehaviour
    {
        public Vector3 GetDir() => Vector3.zero;
    }
}