using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LosingSystem : MonoBehaviour
{
    public static LosingSystem Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool DidUserLockEverything()
    {
        if (RoundsManager.Instance.roundIndex == 0)
        {
            if (SelectionManager.Instance.isWindowCovered && SelectionManager.Instance.isDoorLocked)
            {
                return true;
            }
            else
                return false;
        }
        else if (RoundsManager.Instance.roundIndex >= 1)
        {
            if (SelectionManager.Instance.isWindowCovered && SelectionManager.Instance.isKnifeHidden && SelectionManager.Instance.isDoorLocked)
            {
                return true;
            }
            else return false;
        }

        return false;

    }

    public void LosingScreen()
    {
        SelectionManager.Instance.DoorOpenAnimation.SetTrigger("Open");
    }
}
