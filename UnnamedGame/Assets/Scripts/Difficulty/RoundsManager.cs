using System.Collections.Generic;
using System.Threading.Tasks;
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

    private void Start()
    {
        SpawnItems(rounds[0]);
    }

    [SerializeField] private Round[] rounds;
    [SerializeField] private Light[] lights;
    [SerializeField] private GameObject flashlight;
    [SerializeField] private Animator blackscreenFade;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerVisuals;

    private int roundIndex = 0;

    private List<GameObject> spawned = new();

    public async void StartNextRound()
    {
        FirstPersonMovement.Instance.isAllowed = false;

        await BedAnimation.AnimateToBed(playerVisuals, Camera.main.transform);

        /*
        blackscreenFade.SetTrigger("StartFade");
        await Task.Delay(2000); // wait for blackscreen finished
        */

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
                l.enabled = true;
            }
            flashlight.SetActive(false); // disable flashlight
        }
        else // if the lights should be off
        {
            foreach (Light l in lights) // disable all lights
            {
                l.enabled = false;
            }
            flashlight.SetActive(true); // enable flashlight
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
    private async void TriggerConfirmation()
    {
        var process = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);

        while (!process.isDone) await Task.Yield(); // Wait for load

        Cursor.lockState = CursorLockMode.None;
        ItemConfirmation.Instance.Init(rounds[roundIndex - 1].knife != null);
    }

    private void TriggerWin()
    {
        lights = new Light[0]; // prevents cross-scene error

        TriggerConfirmation();

        Debug.Log("YOU WIN");
        SceneManager.LoadScene(0);
    }
}