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


        private BoidsModel m_previousSelectedModel;
        private void Start()
        {
            m_camera = Camera.main;
            SubscribeInputs();
        }

        private void SubscribeInputs()
        {
            var l_manager = MyInputManager.Instance;
            
            l_manager.SubscribeInput("LeftClick", OnLeftClickPerformed);
        }

        private void OnLeftClickPerformed(InputAction.CallbackContext p_context)
        {

            var l_ray = m_camera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(l_ray, out RaycastHit l_hit, 300f, boidMask))
                return;
            
            if(!l_hit.transform.TryGetComponent(out BoidsModel l_boidModel))
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