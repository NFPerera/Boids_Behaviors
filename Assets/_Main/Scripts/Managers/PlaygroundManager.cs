using _Main.Scripts.Boids;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.Managers
{
    public class PlaygroundManager : MonoBehaviour,IPlaygroundManager
    {
        [SerializeField] private LayerMask boidMask;
        [SerializeField] private GameObject wallsBox;
        [SerializeField] private GameObject teleportBox;
        private Camera m_camera;
        private BoidsModel m_previousSelectedModel;
        private void Start()
        {
            m_camera = Camera.main;
            
            GameManager.Singleton.SetCurrentPlaygroundManager(this);
            SubscribeInputs();
            teleportBox.SetActive(false);
        }

        private void SubscribeInputs()
        {
            var l_manager = MyInputManager.Instance;
            
            l_manager.SubscribeInputOnPerformed(MyGame.LEFT_CLICK_ID, OnLeftClickPerformed);
        }

        private void OnLeftClickPerformed(InputAction.CallbackContext p_context)
        {

            var l_ray = m_camera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(l_ray, out RaycastHit l_hit, 300f, boidMask))
            {
                if (m_previousSelectedModel != null)
                {
                    m_previousSelectedModel.GetUnselected();
                    m_previousSelectedModel = null;
                }
                return;
            }

            if (!l_hit.transform.TryGetComponent(out BoidsModel l_boidModel)) 
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
        
        public void SetWallsInteraction(bool p_b)
        {
            Debug.Log(p_b);
            wallsBox.SetActive(!p_b);
            teleportBox.SetActive(p_b);
        }
    }
}