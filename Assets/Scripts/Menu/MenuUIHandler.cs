using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField]private GameObject panelMenu;
    [SerializeField]private GameObject panelSkins;
    
    private void Start()
    {
        ShowMenu();
    }

    public void StartNew()
    {
        //GameMainManager.Instance.LoadData();
        SceneManager.LoadScene(1);
    }
    
    public void Exit()
    {
        
#if UNITY_EDITOR
        //Don't forget since this use editor code, need to add "using UnityEditor" at the top and wrap it between #if
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void ShowSkinsMenu()
    {
        panelMenu.SetActive(false);
        panelSkins.SetActive(true);
    }

    public void ShowMenu()
    {
        panelSkins.SetActive(false);
        panelMenu.SetActive(true);
    }

}
