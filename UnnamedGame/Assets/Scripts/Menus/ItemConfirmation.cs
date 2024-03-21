using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static RoundsManager;

public class ItemConfirmation : MonoBehaviour
{
    public static ItemConfirmation Instance;
    public GameObject Robber;
    public Animator RobberAnimation;
    private bool won = false;
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
        if (++RoundsManager.Instance.roundIndex >= RoundsManager.Instance.rounds.Length) texts.Enqueue("Congratulations... You won all rounds!"); won = true;
        StartNextDialogue();
    }

    public void StartNextDialogue() => Dialogue();
    private async void Dialogue()
    {
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
        else
        {
            //Blackscreen fading in
            //starting main menu
        }

        string text = texts.Dequeue();          //--> Quene Empty?
        this.text.text = "";

        foreach (var button in buttons) button.SetActive(false);

        foreach (char c in text)
        {
            this.text.text += c;
            await Task.Delay(1);
        }

        foreach (var button in buttons) button.SetActive(true);
    }
    private async void CloseWinningScene()
    {
        Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(2);

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

        foreach (Light l in RoundsManager.Instance.lights)
        {
            l.GetComponent<Light>().intensity = 1f;
        }
    }
    private async void CloseLosingScene()
    {
        Cursor.lockState = CursorLockMode.Locked;
        await Task.Delay(1);
        Time.timeScale = 0.1f;
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(2);
        RobberAnimation.SetBool("start", true);
        //während der Räuber rennt soll ein Blackscreen auftauchen
        //nachdem der Animator ausgeführt wurde soll man im Main Screen landen also ganz am Anfang
    }
}