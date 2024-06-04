using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.Controllers
{
    public class Camera3dMovementController : MonoBehaviour
    {
        [SerializeField] private float normalMovementSpeed=10;
        [SerializeField] private float fastMovementSpeed=30;
        [SerializeField] private float mouseSensX=5;
        [SerializeField] private float mouseSensY=5;

        private bool m_controlsAvailable;
        private Vector3 m_currDir;
        private Vector3 m_mouseDelta;

        private float m_currRotationX;
        private float m_currRotationY;
        private float m_currSpeed;
        private void Start()
        {
            var  l_instance=MyInputManager.Instance;
            l_instance.SubscribeInputOnPerformed(MyGame.RIGHT_CLICK_ID, OnRightClickOnPerformed);
            l_instance.SubscribeInputOnCanceled(MyGame.RIGHT_CLICK_ID, OnRightClickOnCanceled);
            l_instance.SubscribeInputOnPerformed(MyGame.LEFT_SHIFT_ID, OnLeftShiftPerformed);
            l_instance.SubscribeInputOnCanceled(MyGame.LEFT_SHIFT_ID, OnLeftShiftCanceled);

            var l_transformRotation = transform.rotation;
            m_currRotationX = l_transformRotation.x;
            m_currRotationY = l_transformRotation.y;
            m_currSpeed = normalMovementSpeed;
        }

        

        private void OnDestroy()
        {
            var  l_instance=MyInputManager.Instance;
            l_instance.UnsubscribeInputOnPerformed(MyGame.RIGHT_CLICK_ID, OnRightClickOnPerformed);
            l_instance.UnsubscribeInputOnCanceled(MyGame.RIGHT_CLICK_ID, OnRightClickOnCanceled);
            l_instance.UnsubscribeInputOnCanceled(MyGame.LEFT_SHIFT_ID, OnLeftShiftPerformed);
            l_instance.UnsubscribeInputOnCanceled(MyGame.LEFT_SHIFT_ID, OnLeftShiftCanceled);
            
            
            l_instance.UnsubscribeInputOnPerformed(MyGame.CAMERA_MOVEMENT_ID, OnCameraMovementPerformed);
            l_instance.UnsubscribeInputOnPerformed(MyGame.CAMERA_ROTATION_ID, OnCameraRotationPerformed);
        }

        private void Update()
        {
            if(!m_controlsAvailable)
                return;
            
            
            if (m_currDir.magnitude > 0.1)
            {
                transform.position += m_currDir * (m_currSpeed * Time.deltaTime);
            }

            if (m_mouseDelta.magnitude > 0.1)
            {
                float l_mouseX = m_mouseDelta.x * mouseSensX * Time.deltaTime;
                float l_mouseY = m_mouseDelta.y * mouseSensY * Time.deltaTime;

                m_currRotationX -= l_mouseY;
                m_currRotationY += l_mouseX;

                m_currRotationX = Mathf.Clamp(m_currRotationX, -90f, 90f);

                transform.localEulerAngles = new Vector3(m_currRotationX, m_currRotationY, 0);
                m_mouseDelta=Vector3.zero;
            }
        }


        private void OnRightClickOnPerformed(InputAction.CallbackContext p_obj)
        {
            var  l_instance=MyInputManager.Instance;
            l_instance.SubscribeInputOnPerformed(MyGame.CAMERA_MOVEMENT_ID, OnCameraMovementPerformed);
            l_instance.SubscribeInputOnPerformed(MyGame.CAMERA_ROTATION_ID, OnCameraRotationPerformed);
            l_instance.SubscribeInputOnCanceled(MyGame.CAMERA_ROTATION_ID, OnCameraRotationCanceled);

            Cursor.lockState = CursorLockMode.Locked;
            m_controlsAvailable = true;
        }

        

        private void OnRightClickOnCanceled(InputAction.CallbackContext p_obj)
        {
            var  l_instance=MyInputManager.Instance;
            Debug.Log(MyGame.CAMERA_MOVEMENT_ID);
            l_instance.UnsubscribeInputOnPerformed(MyGame.CAMERA_MOVEMENT_ID, OnCameraMovementPerformed);
            l_instance.UnsubscribeInputOnPerformed(MyGame.CAMERA_ROTATION_ID, OnCameraRotationPerformed);
            l_instance.UnsubscribeInputOnCanceled(MyGame.CAMERA_ROTATION_ID, OnCameraRotationCanceled);
            Cursor.lockState = CursorLockMode.None;
            m_controlsAvailable = false;
        }

        private void OnCameraMovementPerformed(InputAction.CallbackContext p_obj)
        {
            var l_inputVal = p_obj.ReadValue<Vector3>();
            
            
            //m_currDir = l_inputVal.RotateVectorToMatchForward(transform.forward);
            var l_transform1 = transform;
            m_currDir = l_transform1.forward*l_inputVal.z+l_transform1.right*l_inputVal.x+l_transform1.up*l_inputVal.y;
        }
        private void OnCameraRotationPerformed(InputAction.CallbackContext p_obj)
        {
            m_mouseDelta = p_obj.ReadValue<Vector2>();
        }
        
        private void OnCameraRotationCanceled(InputAction.CallbackContext p_obj)
        {
            m_mouseDelta=Vector3.zero;
        }
        
        private void OnLeftShiftCanceled(InputAction.CallbackContext p_obj)
        {
            m_currSpeed = normalMovementSpeed;
        }

        private void OnLeftShiftPerformed(InputAction.CallbackContext p_obj)
        {
            m_currSpeed = fastMovementSpeed;
        }
    }
}