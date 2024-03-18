using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ItemConfirmation : MonoBehaviour
{
    public static ItemConfirmation Instance;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject playerVisuals => GameObject.FindGameObjectWithTag("PlayerVisuals");
    private Queue<string> texts = new();

    public void Init(bool knifeActive)
    {
        texts = new();
        texts.Enqueue("Did you lock the door?");
        texts.Enqueue("Did you cover the window?");
        if (knifeActive) texts.Enqueue("Did you hide the knife?");
        StartNextDialogue();
    }

    public void StartNextDialogue() => Dialogue();
    private async void Dialogue()
    {
        if (texts.Count == 0)
        {
            CloseScene();
            return;
        }

        string text = texts.Dequeue();
        this.text.text = "";

        foreach (var button in buttons) button.SetActive(false);

        foreach (char c in text)
        {
            this.text.text += c;
            await Task.Delay(1);
        }

        foreach (var button in buttons) button.SetActive(true);
    }
    private async void CloseScene()
    {
        SelectionManager.Instance.isInAnimation = false;
        FirstPersonMovement.Instance.isAllowed = true;
        Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(2);

        await BedAnimation.AnimateOutOfBed(playerVisuals, Camera.main.transform);
    }
}