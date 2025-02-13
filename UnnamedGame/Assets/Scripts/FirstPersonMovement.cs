using FirstGearGames.SmoothCameraShaker;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
public class FirstPersonMovement : MonoBehaviour
{
    #region Fields

    [SerializeField] private Settings settingsData;
    [SerializeField] private Transform cameraTransform;

    [Space, SerializeField] private float speed = 5f;
    [SerializeField] private float accelerationMultiplier;
    [SerializeField] private AnimationCurve accelerationCurve;

    [Space, SerializeField] private float gravity = -20f;

    [Space, SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayers;

    [Space, SerializeField] private AudioClip footstepSound;
    [SerializeField] private float footstepDelay = 0.3f;

    [Space, SerializeField] private ShakeData cameraShakeData;

    #endregion
    public bool isAllowed = true;
    public static FirstPersonMovement Instance { get; private set; }
    #region Variables

    private CharacterController controller;
    private AudioSource footstepAudioSource;
    private Vector2 input;
    private float yMovement;
    private float currentAcceleration = 0f;
    private float nextFootstep = 0;
    private bool isGrounded;
    private float xRotation = 0f;
    private bool cameraShakeActive = false;

    #endregion

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        controller = GetComponent<CharacterController>();
        footstepAudioSource = GetComponent<AudioSource>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (isAllowed)
        {
            HandleMoving();
            HandleLooking();
        }
    }

    void HandleMoving()
    {
        Vector3 groundExtents = new Vector3(transform.localScale.x, groundCheckDistance, transform.localScale.z) / 2;
        isGrounded = Physics.CheckBox(groundCheckTransform.position, groundExtents);

        if (isGrounded && yMovement < 0) // reset gravity when on the ground
        {
            yMovement = -2f;
        }

        // Get Input (works with controller)
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        if (input.magnitude > 0) currentAcceleration += Time.deltaTime * accelerationMultiplier;
        else currentAcceleration = 0;
        currentAcceleration = Mathf.Clamp01(currentAcceleration);

        input.Normalize();
        input *= accelerationCurve.Evaluate(currentAcceleration);


        Vector3 move = transform.right * input.x + transform.forward * input.y;

        //adding gravity
        yMovement += gravity * Time.deltaTime;

        controller.Move(speed * Time.deltaTime * move);
        controller.Move(Time.deltaTime * yMovement * transform.up);

        if (isGrounded && input.magnitude > 0 && footstepSound != null)
        {
            // play footsteps
            nextFootstep -= Time.deltaTime;
            if (nextFootstep <= 0)
            {
                footstepAudioSource.PlayOneShot(footstepSound, 0.7f);
                nextFootstep += footstepDelay;
                if (settingsData.CameraShakeEnabled) CameraShakerHandler.Shake(cameraShakeData);
            }
        }
    }
    void HandleLooking()
    {
        if (cameraShakeActive) return;

        float MouseX = Input.GetAxis("Mouse X") * settingsData.MouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * settingsData.MouseSensitivity * Time.deltaTime;

        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * MouseX);
    }
}