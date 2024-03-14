using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Settings settingsData;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Toggle camShakeToggle;

    private void Start()
    {
        sensitivitySlider.value = settingsData.MouseSensitivity;
        camShakeToggle.isOn = settingsData.CameraShakeEnabled;
    }

    public void SetSensitivity(float sensitivity)
    {
        settingsData.MouseSensitivity = sensitivity;
    }
    public void SetPPActive(bool active)
    {
        settingsData.OnPostProcessingEnabledEvent(active);
    }
    public void SetCameraShakeActive(bool active)
    {
        settingsData.CameraShakeEnabled = active;
    }

    public void ToggleObject(GameObject obj) => obj.SetActive(!obj.activeSelf);
}