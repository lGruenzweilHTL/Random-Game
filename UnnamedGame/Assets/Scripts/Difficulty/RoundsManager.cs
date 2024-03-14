using System.Collections.Generic;
using UnityEngine;

public class RoundsManager : MonoBehaviour
{
    [System.Serializable]
    public struct Item
    {
        public GameObject Prefab;
        public Vector3[] SpawnPoints;
    }
    [System.Serializable]
    public struct Round
    {
        public Item[] Items;
        public bool LightsOn;
    }

    public static RoundsManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
    }

    [SerializeField] private Round[] rounds;
    [SerializeField] private Light[] lights;
    [SerializeField] private GameObject flashlight;
    private int roundIndex = 0;

    private List<GameObject> spawned = new();

    public void StartNextRound()
    {
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

        foreach (Item item in currentRound.Items) // spawn items for this round
        {
            int randomIndex = Random.Range(0, item.SpawnPoints.Length);

            spawned.Add(Instantiate(item.Prefab, item.SpawnPoints[randomIndex], Quaternion.identity));
        }

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
            flashlight.SetActive(false); // enable flashlight
        }
    }

    private void TriggerWin()
    {
        Debug.Log("YOU WIN");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}