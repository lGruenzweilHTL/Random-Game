using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightSwitchInteraction : MonoBehaviour
{
    private Light BedroomLight;

    private void LightSwichter()
    {
        if (SelectionManager.Instance.isLightOn) BedroomLight.enabled = true;
        else if (!SelectionManager.Instance.isLightOn) BedroomLight.enabled = false;
    }
}
