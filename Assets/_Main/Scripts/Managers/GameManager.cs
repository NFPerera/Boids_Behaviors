using System;
using _Main.Scripts.Boids;
using _Main.Scripts.DevelopmentUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LayerMask boidMask;
        private Camera m_camera;


        private BoidModel m_previousSelectedModel;
        private void Start()
        {
            m_camera = Camera.main;
            SubscribeInputs();
        }

        private void SubscribeInputs()
        {
            var manager = MyInputManager.Instance;
            
            manager.SubscribeInput("LeftClick", OnLeftClickPerformed);
        }

        private void OnLeftClickPerformed(InputAction.CallbackContext p_context)
        {
            Debug.Log("Click");

            var ray = m_camera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit l_hit, 300f, boidMask))
                return;
            
            if(!l_hit.transform.TryGetComponent(out BoidModel l_boidModel))
                return;
            
            if(l_boidModel == m_previousSelectedModel)
                return;
            
            l_boidModel.GetSelected();

            if (m_previousSelectedModel != default)
            {
                m_previousSelectedModel.GetUnselected();
            }
            m_previousSelectedModel = l_boidModel;
        }
    }
}