using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class PostProcessingSystem : MonoBehaviour
{
    [SerializeField] private Settings settings;
    private void Update()
    {
        GetComponent<PostProcessVolume>().enabled = settings.PostProcessingEnabled;
    }
}