using System;
using _Main.Scripts.Boids;
using UnityEngine;

namespace _Main.Scripts
{
    public class WallTriggerTeleporter : MonoBehaviour
    {
        

        private void OnCollisionEnter(Collision p_other)
        {
            if (!p_other.gameObject.TryGetComponent(out BoidsModel l_boid))
                return;

            var l_prevPos = l_boid.transform.position;
            l_boid.transform.position = -l_prevPos;
        }
    }
}