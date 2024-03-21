using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemConfirmation : MonoBehaviour
{
    public static ItemConfirmation Instance;
    public GameObject Robber;
    public Animator RobberAnimation;
    [SerializeField] private Animator blackscreen;
    private bool won = false;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject playerVisuals => GameObject.FindGameObjectWithTag("PlayerVisuals");
    private Queue<string> texts = new();

    public bool confirmationActive => dialogueText.text != "";

    public void Init(bool knifeActive)
    {
        texts = new();
        texts.Enqueue("Did you lock the door?");
        texts.Enqueue("Did you cover the window?");
        if (knifeActive) texts.Enqueue("Did you hide the knife?");
        if (RoundsManager.Instance.roundIndex >= RoundsManager.Instance.rounds.Length)
        {
            texts.Enqueue("Congratulations... You won all rounds!");
            won = true;
        }
        StartNextDialogue();
    }

    public void StartNextDialogue() => Dialogue();
    private async void Dialogue()
    {
        dialogueText.text = "";

        foreach (var button in buttons) button.SetActive(false);

        if (texts.Count == 0 && !won)
        {
            if (LosingSystem.Instance.DidUserLockEverything())
            {
                CloseWinningScene();
            }
            else
            {
                LosingSystem.Instance.LosingScreen();
                CloseLosingScene();
            }
            return;
        }
        else if (won)
        {
            //Blackscreen fading in
            //starting main menu
        }

        string text = texts.Dequeue();

        foreach (char c in text)
        {
            dialogueText.text += c;
            await Task.Delay(1);
        }

        foreach (var button in buttons) button.SetActive(true);
    }
    private async void CloseWinningScene()
    {
        Cursor.lockState = CursorLockMode.Locked;

        await BedAnimation.AnimateOutOfBed(playerVisuals, Camera.main.transform);
        RoundsManager.Instance.isInBed = false;
        FirstPersonMovement.Instance.isAllowed = true;
        SelectionManager.Instance.isInAnimation = false;
        PauseSystem.Instance.Crosshair.SetActive(true);
        SelectionManager.Instance.Bed.GetComponent<BoxCollider>().enabled = true;
        SelectionManager.Instance.isDoorKeyGrabbed = false;
        SelectionManager.Instance.isDoorLocked = false;
        SelectionManager.Instance.isWindowCovered = false;
        SelectionManager.Instance.isKnifeHidden = false;
        SelectionManager.Instance.isLightOn = true;

        foreach (Light l in RoundsManager.Instance.lights)
        {
            l.GetComponent<Light>().intensity = 1f;
        }
    }
    private async void CloseLosingScene()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 0.2f;
        ItemConfirmation.Instance.Robber.SetActive(true);
        RobberAnimation.SetBool("start", true);

        await Task.Delay(3500);
        blackscreen.SetTrigger("StartFade");

        await Task.Delay(2000);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}