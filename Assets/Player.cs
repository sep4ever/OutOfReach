using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{
    [Header("Movement.")]
    Rigidbody playerRB;
    [SerializeField] Vector3 playerInput;
    [SerializeField] float playerSpeed = 10f;

    [Header("Camera.")]

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    public float yaw = 0.0f;
    public float pitch = 0.0f;

    [SerializeField] Transform cameraTransform;
    public bool mouseLock = true;
    [Header("Sound")]
    [SerializeField] AudioSource footstepAudioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        footstepAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void RotateCamera()
    {
        if (mouseLock)
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            pitch = Mathf.Clamp(pitch, -90f, 90f);

            cameraTransform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) mouseLock = !mouseLock;

        Cursor.lockState = mouseLock ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void Move()
    {
        //playerInput = Vector3.zero;
        playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        footstepAudioSource.volume = Mathf.Lerp(footstepAudioSource.volume, playerInput.normalized.magnitude, 5 * Time.deltaTime);

        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        // Движение в направлении взгляда: вперед/назад по camForward, влево/вправо по camRight
        Vector3 move = camRight * playerInput.x + camForward * playerInput.z;

        // Нормализуем, чтобы диагональ не дала больше скорости
        if (move.sqrMagnitude > 1f) move.Normalize();

        Vector3 velocity = move * playerSpeed * Time.fixedDeltaTime;

        playerRB.MovePosition(playerRB.position + velocity);
    }
}
