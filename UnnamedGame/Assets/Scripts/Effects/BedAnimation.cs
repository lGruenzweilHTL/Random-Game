using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class BedAnimation : MonoBehaviour
{
    public const float ANIMATION_TIME = 2f;
    private static Vector3 bedPosition = new(14.2820053f, 3.39200068f, -2.56600285f);
    private static Vector3 doorPosition = new(3.24000072f, 3.648f, 0.920000076f);

    private static Quaternion initialCamRotation = Quaternion.identity;
    private static Vector3 initialCamPosition = Vector3.zero;

    public static async Task AnimateToBed(GameObject visuals, Transform camera)
    {
        if (visuals != null) visuals.SetActive(false);
        PauseSystem.Instance.Crosshair.SetActive(false);
        initialCamRotation = camera.rotation;
        initialCamPosition = camera.position;

        camera.DOMove(bedPosition, ANIMATION_TIME);
        await camera.DOLookAt(doorPosition, ANIMATION_TIME).AsyncWaitForCompletion();
    }
    public static async Task AnimateOutOfBed(GameObject visuals, Transform camera)
    {
        camera.DOMove(initialCamPosition, ANIMATION_TIME);
        await camera.DORotateQuaternion(initialCamRotation, ANIMATION_TIME).AsyncWaitForCompletion();

        if (visuals != null) visuals.SetActive(true);
    }
}