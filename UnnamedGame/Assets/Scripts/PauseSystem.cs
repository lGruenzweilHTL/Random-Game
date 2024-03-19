using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    public static PauseSystem Instance;
    public GameObject Crosshair;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus) Pause();
    }

    [SerializeField] private GameObject PauseObject;
    [SerializeField] private GameObject SettingsPanel;
    public bool IsPaused { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !SelectionManager.Instance.isClipBoardOpen)
        {
            TogglePause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        PauseObject.SetActive(true);
        Crosshair.SetActive(false);
        Cursor.lockState = CursorLockMode.None;

        IsPaused = true;
    }
    public void Unpause()
    {
        Time.timeScale = 1f;
        PauseObject.SetActive(false);
        SettingsPanel.SetActive(false);
        Crosshair.SetActive(true);
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
