using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    #region Singleton Reference

    public PauseSystem Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    #endregion

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) Pause();
    }

    [SerializeField] private GameObject PauseObject;
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
        Cursor.lockState = CursorLockMode.None;

        IsPaused = true;
    }
    public void Unpause()
    {
        Time.timeScale = 1f;
        PauseObject.SetActive(false);
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
