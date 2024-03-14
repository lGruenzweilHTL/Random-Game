using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings", menuName = "Menus/Settings")]
public class Settings : ScriptableObject
{
    public float MouseSensitivity = 300f;
    public Action<bool> OnPostProcessingEnabledEvent;
    public bool CameraShakeEnabled = true;
}