﻿using System;
using System.Collections.Generic;
using _Main.Scripts.Enum;
using TMPro;
using UnityEngine;

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


        private void Start()
        {
            RefreshAllUi();
        }

        public void AddBoidsStat(BoidsStatsModificationsData p_data)
        {
            var l_manager = BoidsManager.Singleton;
            var l_newValue = l_manager.GetBoidsData().CurrBoidsStats[p_data.boidsStatsIds] +
                           p_data.modification;
            l_manager.SetBoidsStats(p_data.boidsStatsIds,l_newValue);
            RefreshStatUi(p_data.boidsStatsIds);
        }
        public void SetBoidsStat(BoidsStatsModificationsData p_data)
        {
            BoidsManager.Singleton.SetBoidsStats(p_data.boidsStatsIds,p_data.modification);
            RefreshStatUi(p_data.boidsStatsIds);
        }

        private void RefreshStatUi(BoidsStatsIds p_statsIds)
        {
            var l_data = BoidsManager.Singleton.GetBoidsData();
            switch (p_statsIds)
            {
                case BoidsStatsIds.MovementSpeed:
                    speedTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.MovementSpeed].ToString();
                    break;
                case BoidsStatsIds.ViewRange:
                    viewRangeTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.ViewRange].ToString();
                    break;
                case BoidsStatsIds.ViewAngle:
                    viewAngleTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.ViewAngle].ToString();
                    break;
                case BoidsStatsIds.ObsAvoidanceWeight:
                    obsAvoidTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.ObsAvoidanceWeight].ToString();
                    break;
                case BoidsStatsIds.CohesionWeight:
                    cohesionTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.CohesionWeight].ToString();
                    break;
                case BoidsStatsIds.AlignmentRadius:
                    alignmentRadiusTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.AlignmentRadius].ToString();
                    break;
                case BoidsStatsIds.AlignmentWeight:
                    alignmentTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.AlignmentWeight].ToString();
                    break;
                case BoidsStatsIds.CohesionRadius:
                    cohesionRadiusTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.CohesionRadius].ToString();
                    break;
            }
        }

        private void RefreshAllUi()
        {
            var l_data = BoidsManager.Singleton.GetBoidsData();
            speedTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.MovementSpeed].ToString();
            viewRangeTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.ViewRange].ToString();
            viewAngleTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.ViewAngle].ToString();
            obsAvoidTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.ObsAvoidanceWeight].ToString();
            cohesionTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.CohesionWeight].ToString();
            alignmentRadiusTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.AlignmentRadius].ToString();
            cohesionRadiusTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.CohesionRadius].ToString();
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