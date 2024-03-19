using UnityEngine;

public class InteractableObject : MonoBehaviour
{ 
    public string ItemName;
    public bool interactable = true;
    public string GetItemName()
    {
        if (!interactable) return "";

        bool isDoorOpen = SelectionManager.Instance.DoorInteractionOpen();
        bool isDoorClose = SelectionManager.Instance.DoorInteractionClose();

        bool isLightOn = SelectionManager.Instance.IsLightOn();
        bool isLightOff = SelectionManager.Instance.IsLightOff();

        bool doorOpen = SelectionManager.Instance.doorOpen;

        if (ItemName == "open" && isDoorOpen)
        {
            ItemName = "locked";
        }
        else if(ItemName == "locked" && !isDoorClose)
        {
            ItemName = "open";
        }
        if (ItemName == "Light on" && isLightOn)
        {
            ItemName = "Light off";
        }
        else if (ItemName == "Light off" && !isLightOff)
        {
            ItemName = "Light on";
        }
        if (ItemName == "Open door" && doorOpen)
        {
            ItemName = "Close door";
        }
        else if (ItemName == "Close door" && !doorOpen)
        {
            ItemName = "Open door";
        }
        return ItemName;
    }
}
