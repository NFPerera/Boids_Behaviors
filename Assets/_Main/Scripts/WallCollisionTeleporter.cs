using System;
using _Main.Scripts.Boids;
using UnityEngine;

namespace _Main.Scripts
{
    public class WallCollisionTeleporter : MonoBehaviour
    {
        private void OnCollisionEnter(Collision p_other)
        {
            if (!p_other.gameObject.TryGetComponent(out BoidsModel l_boid))
                return;

            Vector3 l_contactPoint = p_other.GetContact(0).point;

            //Esto esta mal, pero creo que funcione
            var l_newPos = -l_contactPoint - (-l_contactPoint).normalized;
            // Teleport the entity to the new position
            l_boid.transform.position = l_newPos;
            
            
        }
    }
}