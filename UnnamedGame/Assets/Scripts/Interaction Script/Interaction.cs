using UnityEngine;
using TMPro;
using UnityEngine.ProBuilder.MeshOperations;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private float requiredDistance = 10f;
    public GameObject interaction_Info_UI;
    TMP_Text interaction_text;
    public GameObject ClipBoardText;
    public bool isClipBoardOpen = false;
    public bool IsPaused;

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
                    interaction_Info_UI.SetActive(true);
                    if (interaction_text.text == "Clipboard" && Input.GetKey(KeyCode.E))
                    {
                        ClipBoardText.SetActive(true);
                        isClipBoardOpen = true;
                        TogglePause();
                    }
                }
                else
                {
                    interaction_Info_UI.SetActive(false);
                }
            }
            if (isClipBoardOpen && Input.GetKey(KeyCode.R))
            {
                isClipBoardOpen = false;
                ClipBoardText.SetActive(false);
                if (IsPaused) Unpause();
                interaction_Info_UI.SetActive(false);
            }
        }
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;

        IsPaused = true;
    }
    public void Unpause()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;

        IsPaused = false;
    }
    public void TogglePause()
    {
     Pause();
    }

}


