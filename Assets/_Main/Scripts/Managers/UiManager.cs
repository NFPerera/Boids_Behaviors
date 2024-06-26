using System;
using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities;
using _Main.Scripts.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Main.Scripts.Managers
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> allUiElements = new List<GameObject>();
        [SerializeField] private InputField inputField;
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
        [Header("Stats Obj")] 
        [SerializeField] private GameObject behavioursObj;
        [SerializeField] private GameObject dataObj;
        [SerializeField] private Toggle toggle;
        
        [Header("Flock Management")]
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private int maxDropdownOptions;
        [SerializeField] private List<TMP_Dropdown.OptionData> dropdownOptionDatas;
        [SerializeField] private Button removeFlockButton;

        [Header("Menus")]
        [SerializeField] private GameObject optionsObj;
        
        private int m_currFlockCount;
        private int m_currFlockId;
        private bool m_isUiHidden=false;
        private bool m_canCreateFlock=true;
        private void Start()
        {
            m_currFlockCount = 1;
            m_currFlockId = dropdown.value;
            RefreshAllUi();
            
            MyInputManager.Instance.SubscribeInputOnPerformed(MyGame.ESCAPE_ID, ToggleOptionsMenu);
        }

        private void OnDestroy()
        {
            MyInputManager.Instance.UnsubscribeInputOnPerformed(MyGame.ESCAPE_ID, ToggleOptionsMenu);
        }

        

        public void AddBoidsStat(BoidsStatsModificationsData p_data)
        {
            var l_manager = GameManager.Singleton.BoidsManager;
            var l_previousValue = l_manager.GetBoidsDataByFlockId(m_currFlockId);
            var l_newValue = l_previousValue.CurrBoidsStats[p_data.boidsStatsIds] + p_data.modification;
            
            l_manager.SetFlockStats(m_currFlockId,p_data.boidsStatsIds,l_newValue);
            RefreshStatUi(p_data.boidsStatsIds);
        }
        public void SetBoidsStat(BoidsStatsModificationsData p_data)
        {
            GameManager.Singleton.BoidsManager.SetFlockStats(m_currFlockId,p_data.boidsStatsIds,p_data.modification);
            RefreshStatUi(p_data.boidsStatsIds);
        }

        private void RefreshStatUi(BoidsStatsIds p_statsIds)
        {
            var l_data = GameManager.Singleton.BoidsManager.GetBoidsDataByFlockId(m_currFlockId);
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
            var l_data = GameManager.Singleton.BoidsManager.GetBoidsDataByFlockId(m_currFlockId);
            speedTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.MovementSpeed].ToString();
            viewRangeTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.ViewRange].ToString();
            viewAngleTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.ViewAngle].ToString();
            obsAvoidTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.ObsAvoidanceWeight].ToString();
            cohesionTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.CohesionWeight].ToString();
            alignmentRadiusTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.AlignmentRadius].ToString();
            cohesionRadiusTxt.text = l_data.CurrBoidsStats[BoidsStatsIds.CohesionRadius].ToString();


            removeFlockButton.interactable = m_currFlockCount > 1;

        }

        
        public void ToggleAllUi()
        {
            if (!m_isUiHidden)
            {
                foreach (var l_obj in allUiElements)
                {
                    l_obj.SetActive(false);
                }

                m_isUiHidden = true;
            }
            else
            {
                foreach (var l_obj in allUiElements)
                {
                    l_obj.SetActive(true);
                }
                
                dataObj.SetActive(false);
                m_isUiHidden = false;
            }
        }

        public void ToggleStatsDataUi()
        {
            behavioursObj.SetActive(!behavioursObj.activeSelf);
            dataObj.SetActive(!dataObj.activeSelf);
        }
        
        public void SetBoidsPopulation()
        {
            var l_bool = int.TryParse(inputField.text, out int l_result);

            if (l_bool)
            {
                GameManager.Singleton.BoidsManager.SetBoidsPopulation(l_result);
            }
        }

        public void SetWallsInteraction()
        {
            GameManager.Singleton.PlaygroundManager.SetWallsInteraction(toggle.isOn);
        }

        
        public void OnDropDownSelection()
        {
            var value = dropdown.value;
            
            //Selecciono el mismo que ya estaba seleccionado
            if(value == m_currFlockId)
                return;

            if (value == dropdown.options.Count -1 && m_canCreateFlock)
            {
                AddFlock();
            }

            ChangeSelectedFlockDataUI();
        }
        
        private void ChangeSelectedFlockDataUI()
        {
            m_currFlockId = dropdown.value;
            RefreshAllUi();
        }
        private void AddFlock()
        {
            m_currFlockCount++;
            var l_manager = GameManager.Singleton.BoidsManager;
            l_manager.AddNewFlock();
            l_manager.UpdateBoidsFlock(m_currFlockCount);

            m_canCreateFlock = m_currFlockCount < maxDropdownOptions;
            
            UpdateFlockDropdownUi();
        }

        public void RemoveFlock()
        {
            m_currFlockCount--;
            var l_manager = GameManager.Singleton.BoidsManager;
            l_manager.RemoveFlock();
            l_manager.UpdateBoidsFlock(m_currFlockCount);
            m_canCreateFlock = true;
            UpdateFlockDropdownUi();
        }

        private void UpdateFlockDropdownUi()
        {
            if (m_currFlockCount >= maxDropdownOptions)
            {
                dropdown.options.Clear();
                for (int i = 0; i < m_currFlockCount; i++)
                {
                    dropdown.options.Add(dropdownOptionDatas[i]);
                    //dropdown.options[i] = dropdownOptionDatas[i];
                }
                dropdown.RefreshShownValue();
                return;
            }
            dropdown.options.Clear();
            for (int i = 0; i < m_currFlockCount; i++)
            {
                dropdown.options.Add(dropdownOptionDatas[i]);
                //dropdown.options[i] = dropdownOptionDatas[i];
            }
            
            
            
            dropdown.options.Add(new TMP_Dropdown.OptionData("Create New Flock"));
            dropdown.value = m_currFlockCount-1;
            dropdown.RefreshShownValue();
        }


        private void ToggleOptionsMenu(InputAction.CallbackContext p_obj)
        {
            
            optionsObj.SetActive(!optionsObj.activeSelf);
            var l_timeScale = optionsObj.activeSelf ? 0 : 1;

            Time.timeScale = l_timeScale;
        }
        public void LoadScene(string p_s)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(p_s);
        }
    }
}