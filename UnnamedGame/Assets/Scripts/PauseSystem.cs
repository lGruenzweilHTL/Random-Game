using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    private void OnApplicationFocus(bool focus)
    {
        if (!focus) Pause();
    }

    [SerializeField] private GameObject PauseObject;
    [SerializeField, Tooltip("The object to disable when paused")] private GameObject HUD;
    [SerializeField] private GameObject SettingsPanel;
    public bool IsPaused { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        PauseObject.SetActive(true);
        if (HUD != null) HUD.SetActive(false);
        Cursor.lockState = CursorLockMode.None;

        IsPaused = true;
    }
    public void Unpause()
    {
        Time.timeScale = 1f;
        PauseObject.SetActive(false);
        SettingsPanel.SetActive(false);
        if (HUD != null) HUD.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;

        IsPaused = false;
    }
    public void TogglePause()
    {
        if (IsPaused) Unpause();
        else Pause();
    }


    public void MainMenu()
    {
        Unpause();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void QuitGame() => Application.Quit();
}
