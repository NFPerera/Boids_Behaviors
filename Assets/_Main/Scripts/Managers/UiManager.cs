using System.Collections.Generic;
using _Main.Scripts.Enum;
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


        public void AddBoidsStat(BoidsStatsIds p_statsIds, float p_f)
        {
            
        }
        public void SetBoidsStat(BoidsStatsIds p_statsIds, float p_f)
        {
            BoidsManager.Singleton.SetBoidsStats(p_statsIds,p_f);
            RefreshStatUi(p_statsIds);
        }

        private void RefreshStatUi(BoidsStatsIds p_statsIds)
        {
            var l_data = BoidsManager.Singleton.GetBoidsData();
            switch (p_statsIds)
            {
                case BoidsStatsIds.MovementSpeed:
                    speedTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.MovementSpeed].ToString("F");
                    break;
                case BoidsStatsIds.ViewRange:
                    viewRangeTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.ViewRange].ToString("F");
                    break;
                case BoidsStatsIds.ViewAngle:
                    viewAngleTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.ViewAngle].ToString("F");
                    break;
                case BoidsStatsIds.ObsAvoidanceWeight:
                    obsAvoidTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.ObsAvoidanceWeight].ToString("F");
                    break;
                case BoidsStatsIds.CohesionWeight:
                    cohesionTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.CohesionWeight].ToString("F");
                    break;
                case BoidsStatsIds.AlignmentRadius:
                    alignmentRadiusTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.AlignmentRadius].ToString("F");
                    break;
                case BoidsStatsIds.AlignmentWeight:
                    alignmentTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.AlignmentWeight].ToString("F");
                    break;
                case BoidsStatsIds.CohesionRadius:
                    cohesionRadiusTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.CohesionRadius].ToString("F");
                    break;
            }
            
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