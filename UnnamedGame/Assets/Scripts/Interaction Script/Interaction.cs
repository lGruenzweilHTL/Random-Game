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
    [SerializeField] private Animator DoorOpenAnimation;

    public GameObject interaction_Info_UI;
    public GameObject ClipBoardText;
    public GameObject WoodenPlanks;
    public GameObject knife;
    public GameObject knife_UI;
    public GameObject BoxWhereKnifeIsIn;
    public GameObject DoorKeys;
    public GameObject Bed;

    public Light BedroomLight;

    public bool isClipBoardOpen = false;
    public bool isWindowCovered = false;
    public bool KnifeGrabbed = false;
    public bool knifeisHidden = false;
    public bool GoToBed = false;
    public bool isDoorKeyGrabbed = false;
    public bool isLightOn = true;

    public bool isInAnimation = false;

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
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
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
                    if (!PauseSystem.Instance.IsPaused) interaction_Info_UI.SetActive(true);

                    AnimateDoor(true);
                    ClipBoardInteraction();
                    DoorInteractionOpen();
                    DoorInteractionClose();
                    WindowInteraction();
                    IsLightOn();
                    IsLightOff();
                }
                else interaction_Info_UI.SetActive(false);
            }
            else interaction_Info_UI.SetActive(false);
        }
    }

    public void AnimateDoor(bool open)
    {
        if (interaction_text.text == "" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused)
        {
            DoorOpenAnimation.SetBool("Opened", open);
            DoorOpenAnimation.SetTrigger("Start");
        }
    }
    public bool IsLightOn()
    {
        if (interaction_text.text == "Light on" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused)
        {
            interaction_text.text = "Light off";
            LightSwichter();
            isLightOn = false;
            return true;
        }
        else return false;
    }

    public bool IsLightOff()
    {
        if (interaction_text.text == "Light off" && Input.GetKeyDown(KeyCode.R) && !PauseSystem.Instance.IsPaused)
        {
            interaction_text.text = "Light on";
            LightSwichter();
            isLightOn = true;
            return false;
        }
        else return true;
    }
    public bool DoorInteractionOpen()
    {
        if(interaction_text.text == "Door Key" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused)
        {
            isDoorKeyGrabbed = true;
        }
        if (interaction_text.text == "open" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused && isDoorKeyGrabbed)
        {
            interaction_text.text = "locked";
            return true;
        }
        else return false;
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
            return false;
        }

        else return true;
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
        if(interaction_text.text == "Cover Window" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused)
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
            KnifeGrabbed = true;
        }
        if (KnifeGrabbed)
        {
            BoxWhereKnifeIsIn.GetComponent<InteractableObject>().interactable = true;
        }
        if (interaction_text.text == "Put in Knife" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused && KnifeGrabbed)
        {
            knife_UI.SetActive(true);
            knife_UI.transform.position = new Vector3(6.13506889f, 3.24399996f, 3.15181732f);
            knife.SetActive(false);
            KnifeGrabbed = false;
            knifeisHidden = true;
            knife_UI.GetComponent<InteractableObject>().interactable = false;
            BoxWhereKnifeIsIn.GetComponent<InteractableObject>().interactable = false;
        }
    }

    public void BedInteraction()
    {
        if (interaction_text.text == "Go to Bed" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused && !isInAnimation && !RoundsManager.Instance.isInBed && !isLightOn)
        {
            isInAnimation = true;
            roundsManager.StartNextRound();
            Bed.GetComponent<InteractableObject>().interactable = false;
        }
    }
    public void WindowWoodenPlanksRemover()
    {
        WoodenPlanks.GetComponent<InteractableObject>().interactable = false;
    }

    private void LightSwichter()
    {
        if (isLightOn) BedroomLight.GetComponent<Light>().intensity = 0.3f;
        else if (!isLightOn) BedroomLight.GetComponent<Light>().intensity = 1f;
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



