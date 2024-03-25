using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundsManager : MonoBehaviour
{
    [System.Serializable]
    public struct Round
    {
        public Vector3[] clipboardSpawns;
        public Vector3[] knifeSpawns;
        public Vector3[] doorKeySpawns;
        public bool LightsOn;
    }

    public bool isInBed = false;
    private void Start()
    {
        SpawnItems(rounds[roundIndex]);
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

    [Header("Spawnables")]
    [SerializeField] private GameObject clipboard;
    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject doorKey;

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
        if (currentRound.knifeSpawns.Length > 0)
        {
            GameObject knife = Instantiate(this.knife, ChooseRandomPoint(currentRound.knifeSpawns), this.knife.transform.rotation);
            spawned.Add(knife);
            SelectionManager.Instance.knife_UI = knife;
        }
        if (currentRound.clipboardSpawns.Length > 0) 
            spawned.Add(Instantiate(clipboard, ChooseRandomPoint(currentRound.clipboardSpawns), clipboard.transform.rotation));

        if (currentRound.doorKeySpawns.Length > 0)
        {
            GameObject key = Instantiate(doorKey, ChooseRandomPoint(currentRound.doorKeySpawns), doorKey.transform.rotation);
            spawned.Add(key);
            SelectionManager.Instance.DoorKeys = key;
        }
    }
    private void TriggerConfirmation()
    {
        Cursor.lockState = CursorLockMode.None;

        ItemConfirmation.Instance.Init(rounds[roundIndex - 1].knifeSpawns.Length > 0);
    }

    private void TriggerWin()
    {
        lights = new Light[0];

        TriggerConfirmation();

        Debug.Log("YOU WIN");
        SceneManager.LoadScene(0);
    }

    private Vector3 ChooseRandomPoint(Vector3[] lookup)
    {
        var random = Random.Range(0, lookup.Length);
        return lookup[random];
    }
}
