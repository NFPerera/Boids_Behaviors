using UnityEngine;

namespace _Main.Scripts.Boids
{
    public class BoidsController : MonoBehaviour
    {
        private BoidsModel m_model;
        
        
        private void Start()
        {
            m_model = GetComponent<BoidsModel>();
            
        }

        private void Update()
        {
            var l_wantedDir = Vector3.zero;
            for (int l_i = 0; l_i < m_model.GetData().SteeringBehaviours.Count; l_i++)
            {
                l_wantedDir += m_model.GetData().SteeringBehaviours[l_i].GetDir(m_model);
            }
            
            m_model.Move((gameObject.transform.forward + l_wantedDir), m_model.GetCurrSpeedBasedOnDistance(0.2f));
            //m_model.MoveWithAcceleration((transform.forward+l_wantedDir), m_model.GetCurrSpeedBasedOnDistance(0.5f)*m_model.GetData().AccelerationRate);
        }


        

    }
}