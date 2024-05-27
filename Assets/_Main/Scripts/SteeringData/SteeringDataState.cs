using _Main.Scripts.Boids;
using UnityEngine;

namespace _Main.Scripts.SteeringData
{
    public abstract class SteeringDataState : ScriptableObject
    {
        public abstract Vector3 GetDir(BoidsesModel p_model);
    }
}