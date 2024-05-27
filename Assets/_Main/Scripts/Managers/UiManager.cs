using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.Managers
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> allUiElements = new List<GameObject>();

        [Header("Steerings Stats")]
        [SerializeField] private TMP_Text obsAvoidTxt;
        [SerializeField] private TMP_Text cohesionTxt;
        [SerializeField] private TMP_Text alignmentTxt;
        [SerializeField] private TMP_Text alignmentRadiusTxt;
        [SerializeField] private TMP_Text cohesionRadiusTxt;
        
        [Header("Boids Stats")]
        [SerializeField] private TMP_Text speedTxt;
        [SerializeField] private TMP_Text viewRangeTxt;
        [SerializeField] private TMP_Text viewAngleTxt;

        public void SetBoidSpeed(float p_f)
        {
            BoidsManager.Singleton.SetBoidsSpeed(p_f);
            speedTxt.text = p_f.ToString();
        }
        
        public void SetBoidViewRange(float p_f)
        {
            BoidsManager.Singleton.SetBoidsViewRange(p_f);
            viewRangeTxt.text = p_f.ToString();
        }
        
        public void SetBoidViewAngle(float p_f)
        {
            BoidsManager.Singleton.SetBoidsViewAngle(p_f);
            viewAngleTxt.text = p_f.ToString();
        }
        
        public void SetBoidObsAvoid(float p_f)
        {
            BoidsManager.Singleton.SetBoidsObsAvoid(p_f);
            obsAvoidTxt.text = p_f.ToString();
        }
        public void SetBoidCohesion(float p_f)
        {
            BoidsManager.Singleton.SetBoidsCohesion(p_f);
            cohesionTxt.text = p_f.ToString();
        }
        public void SetBoidAlignment(float p_f)
        {
            BoidsManager.Singleton.SetBoidsAlignment(p_f);
            alignmentTxt.text = p_f.ToString();
        }
        public void SetBoidAlignmentRadius(float p_f)
        {
            BoidsManager.Singleton.SetBoidsAlignmentRadius(p_f);
            alignmentRadiusTxt.text = p_f.ToString();
        }
        public void SetBoidCohesionRadius(float p_f)
        {
            BoidsManager.Singleton.SetBoidsCohesionRadius(p_f);
            cohesionRadiusTxt.text = p_f.ToString();
        }
        
        
        public void ToggleUi()
        {
            foreach (var obj in allUiElements)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }
        
        
    }
}