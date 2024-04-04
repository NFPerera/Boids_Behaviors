using System;
using UnityEngine;

namespace _Main.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        private Camera m_camera;


        private void Start()
        {
            m_camera = Camera.main;
        }
    }
}