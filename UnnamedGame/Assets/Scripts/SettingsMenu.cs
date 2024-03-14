using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Settings settingsData;
    [SerializeField] private Slider sensitivitySlider;

    private void Start()
    {
        sensitivitySlider.value = settingsData.MouseSensitivity;
    }

    public void SetSensitivity(float sensitivity)
    {
        settingsData.MouseSensitivity = sensitivity;
    }
    public void SetPPActive(bool active)
    {
        settingsData.OnPostProcessingEnabled(active);
    }
    public void SetCameraShakeActive(bool active)
    {
        settingsData.CameraShakeEnabled = active;
    }

    public void ToggleObject(GameObject obj) => obj.SetActive(!obj.activeSelf);
}