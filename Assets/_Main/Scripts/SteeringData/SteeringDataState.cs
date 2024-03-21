using _Main.Scripts.Boids;
using UnityEngine;

namespace _Main.Scripts.SteeringData
{
    public abstract class SteeringDataState : ScriptableObject
    {
        public abstract Vector3 GetDir(BoidModel p_model);
    }
}