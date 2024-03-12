using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
public class FirstPersonMovement : MonoBehaviour
{
    #region Fields

    [SerializeField] private Transform cameraTransform;

    [Space, SerializeField] private float speed = 5f;
    [SerializeField] private float mouseSensitivity = 300f;

    [Space, SerializeField] private float gravity = -20f;

    [Space, SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayers;

    [Space, SerializeField] private AudioClip footstepSound;
    [SerializeField] private float footstepDelay = 0.3f;
    [SerializeField] private float cameraWiggleTime = 0.1f;
    [SerializeField] private float cameraWiggleOffset = 0.1f;

    #endregion

    #region Variables

    private CharacterController controller;
    private AudioSource footstepAudioSource;

    private Vector2 input;

    private float yMovement;

    private float nextFootstep = 0;

    private bool isGrounded;

    private float xRotation = 0f;

    #endregion

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        controller = GetComponent<CharacterController>();
        footstepAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleMoving();
        HandleLooking();
    }

    void HandleMoving()
    {
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckDistance, groundLayers);

        if (isGrounded && yMovement < 0) // reset gravity when on the ground
        {
            yMovement = -2f;
        }

        // Get Input (works with controller)
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");


        Vector3 move = transform.right * input.x + transform.forward * input.y;

        //adding gravity
        yMovement += gravity * Time.deltaTime;

        controller.Move(speed * Time.deltaTime * move);
        controller.Move(Time.deltaTime * yMovement * transform.up);

        if (isGrounded && input.magnitude > 0 && footstepSound != null) // play footsteps
        {
            nextFootstep -= Time.deltaTime;
            if (nextFootstep <= 0)
            {
                footstepAudioSource.PlayOneShot(footstepSound, 0.7f);
                nextFootstep += footstepDelay;
            }
        }
    }
    void HandleLooking()
    {
        float MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Vector3 newXRot = cameraTransform.localRotation.eulerAngles;
        newXRot.x = xRotation;
        cameraTransform.localRotation = Quaternion.Euler(newXRot);
        transform.Rotate(Vector3.up * MouseX);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckDistance);
    }
}