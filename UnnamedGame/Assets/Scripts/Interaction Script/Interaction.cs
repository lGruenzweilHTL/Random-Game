using UnityEngine;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    [SerializeField] private RoundsManager roundsManager;
    [SerializeField] private float requiredDistance = 10f;
    [SerializeField] private Animator windowAnimator;
    TMP_Text interaction_text;
    [SerializeField] private Animator blackscreenFade;

    public GameObject interaction_Info_UI;
    public GameObject ClipBoardText;
    public GameObject WoodenPlanks;
    public GameObject knife;
    public GameObject knife_UI;
    public GameObject BoxWhereKnifeIsIn;

    public bool isClipBoardOpen = false;
    public bool IsPaused;
    public bool isWindowCovered = false;
    public bool KnifeGrabbed = false;
    public bool knifeisHidden = false;
    public bool GoToBed = false;

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
                    interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                    if(!PauseSystem.Instance.IsPaused) interaction_Info_UI.SetActive(true);

                    ClipBoardInteraction();
                    DoorInteractionOpen();
                    DoorInteractionClose();
                    WindowInteraction();
                    KnifeInteraction();
                    BedInteraction();
                }
                else interaction_Info_UI.SetActive(false);
            }
            else interaction_Info_UI.SetActive(false);
        }
    }

    public bool DoorInteractionOpen()
    {
        if (interaction_text.text == "open" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused)
        {
            interaction_text.text = "locked";
            return true;
        }
        else return false;
    }

    public bool DoorInteractionClose()
    {
        if (interaction_text.text == "locked" && Input.GetKeyDown(KeyCode.R) && !PauseSystem.Instance.IsPaused)
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
            if (IsPaused) Unpause();
            interaction_Info_UI.SetActive(false);
        }
    }

    private void WindowInteraction()
    {
        if(interaction_text.text == "Cover Window" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused)
        {
            windowAnimator.SetTrigger("Cover");
        }
    }

    private void KnifeInteraction()
    {
        if (interaction_text.text == "Knife" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused)
        {
            knife_UI.SetActive(false);
            knife.SetActive(true);
            KnifeGrabbed = true;
        }
        if(interaction_text.text == "Put in Knife" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused && KnifeGrabbed)
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
        if (interaction_text.text == "Go to Bed" && Input.GetKeyDown(KeyCode.E) && !PauseSystem.Instance.IsPaused && !isInAnimation)
        {
            isInAnimation = true;
            roundsManager.StartNextRound();
        }
    }
    public void WindowWoodenPlanksRemover()
    {
        WoodenPlanks.GetComponent<InteractableObject>().interactable = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void TogglePause()
    {
        Pause();
    }
}



