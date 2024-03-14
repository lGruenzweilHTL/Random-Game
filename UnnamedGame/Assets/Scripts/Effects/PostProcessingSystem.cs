using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class PostProcessingSystem : MonoBehaviour
{
    [SerializeField] private Settings settings;
    private void Start()
    {
        settings.OnPostProcessingEnabled += (active) => GetComponent<PostProcessVolume>().enabled = active;
    }
}