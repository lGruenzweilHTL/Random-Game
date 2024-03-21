using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundsManager : MonoBehaviour
{
    [System.Serializable]
    public struct Round
    {
        public GameObject clipboard;
        public GameObject knife;
        public bool LightsOn;
    }

    public bool isInBed = false;
    private void Start()
    {
        SpawnItems(rounds[0]);
    }

    //Instance
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

    public static RoundsManager Instance { get; private set; }
    [SerializeField] public Round[] rounds;
    [SerializeField] public Light[] lights;
    [SerializeField] public GameObject flashlight;
    [SerializeField] private Animator blackscreenFade;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerVisuals;

    public bool isLightSwitchAllowed = true;

    public int roundIndex = 0;

    private List<GameObject> spawned = new();

    public async void StartNextRound()
    {
        FirstPersonMovement.Instance.isAllowed = false;

        await BedAnimation.AnimateToBed(playerVisuals, Camera.main.transform);
        isInBed = true;


        if (++roundIndex >= rounds.Length)
        {
            TriggerWin();
            return;
        }

        foreach (GameObject obj in spawned) // Destroy all objects from previous round
        {
            Destroy(obj);
        }

        Round currentRound = rounds[roundIndex];

        SpawnItems(currentRound);

        if (currentRound.LightsOn) // if the lights should be on
        {
            foreach (Light l in lights) // enable all lights
            {
                l.GetComponent<Light>().intensity = 0.3f;
            }
            flashlight.SetActive(false); // disable flashlight
        }
        else // if the lights should be off
        {
            foreach (Light l in lights) // disable all lights
            {
                l.GetComponent<Light>().intensity = 0.3f;
            }
            SelectionManager.Instance.FlashlightOnDesk.SetActive(true);
            isLightSwitchAllowed = false;
        }

        TriggerConfirmation();
    }
    private void SpawnItems(Round currentRound)
    {
        if (currentRound.knife != null)
        {
            GameObject knife = Instantiate(currentRound.knife);
            spawned.Add(knife);
            SelectionManager.Instance.knife_UI = knife;
        }
        if (currentRound.clipboard != null) spawned.Add(Instantiate(currentRound.clipboard));
    }
    private void TriggerConfirmation()
    {
        Cursor.lockState = CursorLockMode.None;

        ItemConfirmation.Instance.Init(rounds[roundIndex - 1].knife != null);
    }

    private void TriggerWin()
    {
        lights = new Light[0];

        TriggerConfirmation();

        Debug.Log("YOU WIN");
        SceneManager.LoadScene(0);
    }
}
