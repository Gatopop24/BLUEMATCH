using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject menuPause;
    private bool isPaused = false;

    private void Start()
    {
        Time.timeScale = 1;
        menuPause.SetActive(false);
        isPaused = false;
    }

    private void Update()
    {
        if(InputController.Instance.GetButtonDown(InputController.InputAction.Pause))
        {
            if(isPaused)
            {
                Resume();
            }
            else
            {

                Pause();
            }
        }
    }

    public void Resume()
    {
        menuPause.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    private void Pause()
    {
        menuPause.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}
