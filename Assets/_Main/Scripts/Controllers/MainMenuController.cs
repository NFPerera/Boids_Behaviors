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
        [SerializeField] private GameObject select3dArenaMenuObj;
        [SerializeField] private GameObject select2dArenaMenuObj;



        public void SelectMainMenu() => ChangeCurrentMenuSelection(MenuSection.MainMenu);
        public void SelectOptionsMenu() => ChangeCurrentMenuSelection(MenuSection.OptionsMenu);
        public void SelectSimMenu() => ChangeCurrentMenuSelection(MenuSection.SelectSimMenu);
        public void Select3dArenaMenu() => ChangeCurrentMenuSelection(MenuSection.Select3dArena);
        public void Select2dArenaMenu() => ChangeCurrentMenuSelection(MenuSection.Select2dArena);
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
                    select3dArenaMenuObj.SetActive(false);
                    select2dArenaMenuObj.SetActive(false);
                    break;
                
                case MenuSection.OptionsMenu:
                    mainMenuObj.SetActive(false);
                    optionsMenuObj.SetActive(true);
                    selectSimMenuObj.SetActive(false);
                    select3dArenaMenuObj.SetActive(false);
                    select2dArenaMenuObj.SetActive(false);
                    break; 
                
                case MenuSection.SelectSimMenu:
                    mainMenuObj.SetActive(false);
                    optionsMenuObj.SetActive(false);
                    selectSimMenuObj.SetActive(true);
                    select3dArenaMenuObj.SetActive(false);
                    select2dArenaMenuObj.SetActive(false);
                    break; 
                
                case MenuSection.Select3dArena:
                    mainMenuObj.SetActive(false);
                    optionsMenuObj.SetActive(false);
                    selectSimMenuObj.SetActive(false);
                    select3dArenaMenuObj.SetActive(true);
                    select2dArenaMenuObj.SetActive(false);
                    break;
                
                case MenuSection.Select2dArena:
                    mainMenuObj.SetActive(false);
                    optionsMenuObj.SetActive(false);
                    selectSimMenuObj.SetActive(false);
                    select3dArenaMenuObj.SetActive(false);
                    select2dArenaMenuObj.SetActive(true);
                    break;
            }
        }
    }
    public enum MenuSection
    {
        MainMenu,
        OptionsMenu,
        SelectSimMenu,
        Select3dArena,
        Select2dArena
    }
}