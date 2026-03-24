using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement.")]
    public bool canMove = false;
    [SerializeField] Rigidbody playerRB;
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

    [Header("QuestSystem")]
    [SerializeField] QuestHandling questHandler;
    [SerializeField] public Quest quest;
    public HashSet<InteractionType> interactions = new HashSet<InteractionType>();

    [Header("Starting Dialogue")]
    [SerializeField] DialogueBox dialogueBox;
    [SerializeField] bool firstStart = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        footstepAudioSource = GetComponent<AudioSource>();
        layerMask = LayerMask.GetMask("Interactable");
        if (GameManager.Instance != null) GameManager.Instance.LoadGame();
    }

    private void Awake()
    {
        dialogueBox.SetMessages(new System.Collections.Generic.List<string> { "Снова.", "Снова я проснулся.", "Каждый день я надеюсь, что я не проснусь.", "Хотя... Весь этот кошмар что со мной происходит...", "Моя жизнь кажется просто плохим сном.", "После того как началась война..." });
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!canMove && firstStart)
        {
            canMove = dialogueBox.finishedDialogue;
            firstStart = false;
        }*/
        RotateCamera();
    }

    private void FixedUpdate()
    {
        if (canMove && !dialogueBox.isActive) Move();
        if (!canMove) footstepAudioSource.volume = Mathf.Lerp(footstepAudioSource.volume, 0, 5 * Time.deltaTime);
        Interact();
    }

    [SerializeField] LayerMask layerMask;
    [SerializeField] TMP_Text interactionText;
    [SerializeField] float interactionRange;
    bool interacted = false;
    public bool hasAxe = false;
    void Interact()
    {
        RaycastHit hit;

        bool isHit = Physics.Raycast(
            transform.position,
            cameraTransform.forward,
            out hit,
            interactionRange,
            layerMask
        );

        if (!isHit)
        {
            interactionText.gameObject.SetActive(false);
            interacted = false;
            interactions.Clear();
            return;
        }

        var interactable = hit.transform.GetComponentInParent<Interactable>();

        if (interactable != null)
        {
            interactionText.gameObject.SetActive(!interacted && interactable.canInteract);

            if (Input.GetKeyDown(KeyCode.E))
            {
                interacted = true;
                interactable.Interact();

                if (!interactions.Contains(interactable.id)) interactions.Add(interactable.id);
                if (!hasAxe && questHandler.questId == 9) hasAxe = hit.transform.GetComponent<Axe>() != null;
                /*interactedWithWindow = hit.transform.GetComponent<Window>() != null;
                interactedWithWorkingStation = hit.transform.GetComponent<WorkingStation>() != null;
                interactedWithBed = hit.transform.GetComponent<Bed>() != null;
                interactedWithShotgun = hit.transform.GetComponent<Shotgun>() != null;*/
            }
        }
        else
        {
            interactionText.gameObject.SetActive(false);
        }
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
