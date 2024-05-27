using _Main.Scripts.SteeringData;
using UnityEngine;

namespace _Main.Scripts.Boids
{
    public interface IBoids
    {
        void Initialize(Vector3 p_spawnPoint, Vector3 p_initDir);
    }
}