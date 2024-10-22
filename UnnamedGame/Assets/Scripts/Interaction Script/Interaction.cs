using UnityEngine;
using TMPro;
using DG.Tweening;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    [SerializeField] private RoundsManager roundsManager;
    [SerializeField] private float requiredDistance = 10f;
    [SerializeField] private Animator windowAnimator;
    TMP_Text interaction_text;
    [SerializeField] private Animator blackscreenFade;
    [SerializeField] public Animator DoorOpenAnimation;

    public GameObject interaction_Info_UI;
    public GameObject ClipBoardText;
    public GameObject WoodenPlanks;
    public GameObject knife;
    public GameObject knife_UI;
    public GameObject BoxWhereKnifeIsIn;
    public GameObject DoorKeys;
    public GameObject Bed;
    public GameObject FlashlightOnDesk;

    public Light BedroomLight;

    public bool allowsAnimation = false;
    public bool isClipBoardOpen = false;
    public bool isWindowCovered = false;
    public bool isKnifeGrabbed = false;
    public bool isKnifeHidden = false;
    public bool isDoorKeyGrabbed = false;
    public bool isDoorLocked = false;
    public bool isLightOn = true;
    public bool isFlashlightGrabbed = false;

    public bool isInAnimation = false;

    public bool doorOpen = false;

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

    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<TMP_Text>();
        ClipBoardText.SetActive(false);
        interaction_Info_UI.SetActive(false);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var selectionTransform = hit.transform;
            var playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

            if (Vector3.Distance(selectionTransform.position, playerPosition) < requiredDistance)
            {
                if (selectionTransform.GetComponent<InteractableObject>())
                {
                    BedInteraction();
                    KnifeInteraction();
                    interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                    if (!PauseSystem.Instance.IsPaused)
                        interaction_Info_UI.SetActive(true);

                    ToggleDoorInteraction();
                    ClipBoardInteraction();
                    DoorInteractionOpen();
                    DoorInteractionClose();
                    WindowInteraction();
                    IsLightOn();
                    IsLightOff();
                    GrabFlashlight();
                }
                else
                    interaction_Info_UI.SetActive(false);
            }
            else
                interaction_Info_UI.SetActive(false);
        }
    }

    public void GrabFlashlight()
    {
        if (interaction_text.text == "Grab Flashlight" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused)
        {
            RoundsManager.Instance.flashlight.SetActive(true);
            FlashlightOnDesk.SetActive(false);
            isFlashlightGrabbed = true;
        }
    }
    public void ToggleDoorInteraction()
    {
        if (interaction_text.text is "Open door" or "Close door" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused)
        {
            allowsAnimation = DoorOpenAnimation.GetCurrentAnimatorStateInfo(0).IsName("Wait");
            if (allowsAnimation)
            {
                doorOpen = !doorOpen;
                DoorOpenAnimation.SetTrigger(doorOpen ? "Open" : "Close");
            } 
        }
    }
    public bool IsLightOn()
    {
        if (interaction_text.text == "Light on" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused && RoundsManager.Instance.isLightSwitchAllowed)
        {
            interaction_text.text = "Light off";
            LightSwichter();
            isLightOn = false;
            return true;
        }
        else
            return false;
    }

    public bool IsLightOff()
    {
        if (interaction_text.text == "Light off" && Input.GetKeyDown(KeyCode.R) && !PauseSystem.Instance.IsPaused && RoundsManager.Instance.isLightSwitchAllowed)
        {
            interaction_text.text = "Light on";
            LightSwichter();
            isLightOn = true;
            return false;
        }
        else
            return true;
    }
    public bool DoorInteractionOpen()
    {
        if (interaction_text.text == "Door Key" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused)
        {
            isDoorKeyGrabbed = true;
        }
        if (interaction_text.text == "open" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused && isDoorKeyGrabbed)
        {
            interaction_text.text = "locked";
            isDoorLocked = true;
            return true;
        }
        else
            return false;
    }

    public bool DoorInteractionClose()
    {
        if (interaction_text.text == "Door Key" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused)
        {
            DoorKeys.SetActive(false);
            isDoorKeyGrabbed = true;
        }
        if (interaction_text.text == "locked" && Input.GetKeyDown(KeyCode.R) && !PauseSystem.Instance.IsPaused && isDoorKeyGrabbed)
        {
            interaction_text.text = "open";
            isDoorLocked = false;
            return false;
        }

        else
            return true;
    }

    private void ClipBoardInteraction()
    {
        if (interaction_text.text == "Clipboard" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused)
        {
            ClipBoardText.SetActive(true);
            isClipBoardOpen = true;
            TogglePause();
        }

        if (isClipBoardOpen && Input.GetKey(KeyCode.R) && !PauseSystem.Instance.IsPaused)
        {
            isClipBoardOpen = false;
            ClipBoardText.SetActive(false);
            Unpause();
            interaction_Info_UI.SetActive(false);
        }
    }

    private void WindowInteraction()
    {
        if (interaction_text.text == "Cover Window" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused)
        {
            windowAnimator.SetTrigger("Cover");
            isWindowCovered = true;
        }
    }

    private void KnifeInteraction()
    {
        if (!PauseSystem.Instance.IsPaused && RoundsManager.Instance.roundIndex == 0)
        {
            BoxWhereKnifeIsIn.GetComponent<InteractableObject>().interactable = false;
        }
        if (interaction_text.text == "Knife" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused)
        {
            knife_UI.SetActive(false);
            knife.SetActive(true);
            isKnifeGrabbed = true;
        }
        if (isKnifeGrabbed)
        {
            BoxWhereKnifeIsIn.GetComponent<InteractableObject>().interactable = true;
        }
        if (interaction_text.text == "Put in Knife" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused && isKnifeGrabbed)
        {
            knife_UI.SetActive(true);
            knife_UI.transform.position = new Vector3(6.13506889f, 3.24399996f, 3.15181732f);
            knife.SetActive(false);
            isKnifeGrabbed = false;
            isKnifeHidden = true;
            knife_UI.GetComponent<InteractableObject>().interactable = false;
            BoxWhereKnifeIsIn.GetComponent<InteractableObject>().interactable = false;
        }
    }

    public void BedInteraction()
    {
        if (interaction_text.text == "Go to Bed" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused && !isInAnimation && !RoundsManager.Instance.isInBed && !isLightOn && !doorOpen)
        {
            isInAnimation = true;
            Bed.GetComponent<BoxCollider>().enabled = false;
            foreach (Light l in RoundsManager.Instance.lights) // enable all lights
            {
                l.GetComponent<Light>().intensity = 0.3f;
            }
            roundsManager.StartNextRound();
        }
    }
    public void WindowWoodenPlanksRemover()
    {
        WoodenPlanks.GetComponent<InteractableObject>().interactable = false;
    }

    private void LightSwichter()
    {
        if (isLightOn)
            BedroomLight.GetComponent<Light>().intensity = 0.3f;
        else if (!isLightOn)
            BedroomLight.GetComponent<Light>().intensity = 1f;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
    }

    public void TogglePause()
    {
        Pause();
    }
}



