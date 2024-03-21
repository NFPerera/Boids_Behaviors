using UnityEngine;

namespace _Main.Scripts.Boids
{
    public class BoidModel : MonoBehaviour, IBoid
    {

        private BoidController m_controller;
        public Vector3 p_dir;
        
        public void Initialize(Vector3 p_spawnPoint, Vector3 p_initDir)
        {
            transform.position = p_spawnPoint;
            p_dir = p_initDir;
            transform.Rotate(Quaternion.Euler(p_initDir).eulerAngles);
        }

        public void Move(Vector3 p_dir, float p_speed)
        {
            transform.position += p_dir.normalized * (p_speed * Time.deltaTime);
        }
    }
}