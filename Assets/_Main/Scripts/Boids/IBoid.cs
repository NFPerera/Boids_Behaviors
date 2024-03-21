using _Main.Scripts.SteeringData;
using UnityEngine;

namespace _Main.Scripts.Boids
{
    public interface IBoid
    {
        void Initialize(Vector3 p_spawnPoint, Vector3 p_initDir, SteeringsId p_initSteeringBh);
    }
}