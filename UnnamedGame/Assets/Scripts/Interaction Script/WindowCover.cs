using UnityEngine;

public class WindowCover : MonoBehaviour
{
    public void Cover()
    {
        SelectionManager.Instance.WindowWoodenPlanksRemover();
    }
}
