using System;
using UnityEngine;

namespace _Main.Scripts.Boids
{
    public class BoidController : MonoBehaviour
    {
        
        [SerializeField] private float movementSpeed;
        private BoidModel p_model;


        private void Awake()
        {
            p_model = GetComponent<BoidModel>();
        }

        private void Update()
        {
            p_model.Move(p_model.p_dir,movementSpeed);
        }
    }
}