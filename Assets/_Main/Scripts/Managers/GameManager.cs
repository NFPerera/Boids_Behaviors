using System;
using _Main.Scripts.Boids;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Singleton;
        private void Awake()
        {
            if (Singleton != null)
            {
                Destroy(this);
                return;
            }

            Singleton = this;
        }


        public IPlaygroundManager PlaygroundManager => m_currentPlaygroundManager;
        private IPlaygroundManager m_currentPlaygroundManager;
        

        public void SetCurrentPlaygroundManager(IPlaygroundManager p_manager) => m_currentPlaygroundManager = p_manager;
    }
}