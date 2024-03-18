using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings", menuName = "Menus/Settings")]
public class Settings : ScriptableObject
{
    public float MouseSensitivity = 300f;
    public bool PostProcessingEnabled = true;
    public bool CameraShakeEnabled = true;
}