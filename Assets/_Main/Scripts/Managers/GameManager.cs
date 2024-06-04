using _Main.Scripts.Interfaces;
using UnityEngine;

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
            DontDestroyOnLoad(this);
        }


        public IPlaygroundManager PlaygroundManager => m_currentPlaygroundManager;
        private IPlaygroundManager m_currentPlaygroundManager;
        public BoidsManager BoidsManager => m_boidsManager;
        private BoidsManager m_boidsManager;

        public void SetCurrentPlaygroundManager(IPlaygroundManager p_manager) => m_currentPlaygroundManager = p_manager;
        public void SetCurrentBoidsManager(BoidsManager p_manager) => m_boidsManager = p_manager;
    }
}