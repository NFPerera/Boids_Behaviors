using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Main.Scripts.Controllers
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuObj;
        [SerializeField] private GameObject optionsMenuObj;
        [SerializeField] private GameObject selectSimMenuObj;



        public void SelectMainMenu() => ChangeCurrentMenuSelection(MenuSection.MainMenu);
        public void SelectOptionsMenu() => ChangeCurrentMenuSelection(MenuSection.OptionsMenu);
        public void SelectSimMenu() => ChangeCurrentMenuSelection(MenuSection.SelectSimMenu);
        public void ChangeScene(string p_sceneToLoad) => SceneManager.LoadScene(p_sceneToLoad);
        public void ExitApp() => Application.Quit();
        private void ChangeCurrentMenuSelection(MenuSection p_currSelection)
        {
            switch (p_currSelection)
            {
                case MenuSection.MainMenu:
                    mainMenuObj.SetActive(true);
                    optionsMenuObj.SetActive(false);
                    selectSimMenuObj.SetActive(false);
                    break;
                
                case MenuSection.OptionsMenu:
                    mainMenuObj.SetActive(false);
                    optionsMenuObj.SetActive(true);
                    selectSimMenuObj.SetActive(false);
                    break; 
                
                case MenuSection.SelectSimMenu:
                    mainMenuObj.SetActive(false);
                    optionsMenuObj.SetActive(false);
                    selectSimMenuObj.SetActive(true);
                    break; 
            }
        }
    }
    public enum MenuSection
    {
        MainMenu,
        OptionsMenu,
        SelectSimMenu
    }
}