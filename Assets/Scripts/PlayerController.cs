using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float crouchSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float turnSpeed;
    [SerializeField] GameObject flashlightObj;
    [SerializeField] float flashOnFogDensity;
    [SerializeField] float flashOffFogDensity;
    
    bool canJump = false;
    public bool onGround = false;
    bool flashOnOff = false;
    public bool isMoving = false;
    bool isCrouching = false;
    public bool isRunning = false;
    
    Vector2 movement;
    Rigidbody rb;
    Vector2 lookInput;
    public Animator anim;
    public CapsuleCollider playerCollider;
    public Transform playerCamera;

    float VelocityX;
    float VelocityZ;
    float CrouchValue;
    float pitch;
    float moveSpeed;

    void Awake()
    {
        moveSpeed = walkSpeed;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        flashOnOff = flashlightObj.activeSelf;
    }

    void Update()
    {
        float speedFactor = isRunning ? 1 : 0.5f;

        VelocityX = Mathf.Lerp(VelocityX, movement.x * speedFactor, Time.deltaTime * 10);
        VelocityZ = Mathf.Lerp(VelocityZ, movement.y * speedFactor, Time.deltaTime * 10);

        anim.SetFloat("VelocityX", VelocityX);
        anim.SetFloat("VelocityZ", VelocityZ);
        
        float targetCrouch = isCrouching ? 1f : 0f;
        CrouchValue = Mathf.Lerp(CrouchValue, targetCrouch, Time.deltaTime * 10f);
        anim.SetFloat("CrouchValue", CrouchValue);
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
        HandleTurn();
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed && isCrouching == false)
        {
            moveSpeed = runSpeed;
            isRunning = true;
        }
        else if (context.canceled)
        {
            moveSpeed = walkSpeed;
            isRunning = false;
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = !isCrouching;
            anim.SetBool("isCrouching", isCrouching);
            if (isCrouching)
            {
                moveSpeed = crouchSpeed;
                playerCamera.transform.localPosition = new Vector3(0, -0.81f, 0);
                playerCollider.height = 1.18f;
                playerCollider.center = new Vector3(0, 0.57f, 0);
            }
            else
            {
                moveSpeed = walkSpeed;
                playerCamera.transform.localPosition = new Vector3(0, 0, 0);
                playerCollider.height = 1.88f;
                playerCollider.center = new Vector3(0, 0.94f, 0);
            }
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && onGround)
        {
            canJump = true;
        }
    }

    public void Camera(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void Flashlight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            flashOnOff = !flashOnOff;
            flashlightObj.SetActive(flashOnOff);

            if (flashOnOff)
            {
                RenderSettings.fogDensity = flashOnFogDensity;
            }
            else
            {
                RenderSettings.fogDensity = flashOffFogDensity;
            }
        }
    }

    void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(movement.x, 0, movement.y);

        Vector3 worldMove = transform.TransformDirection(moveDirection);
        rb.linearVelocity = new Vector3(worldMove.x * moveSpeed, rb.linearVelocity.y, worldMove.z * moveSpeed);
        
        if (movement.magnitude > 0.1f || onGround == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    void HandleJump()
    {
        if (canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("jumpTrigger");
            isRunning = false;
            canJump = false;
            onGround = false;
        }
    }

    void HandleTurn()
    {
        float mouseX = lookInput.x * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, mouseX, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        pitch -= lookInput.y * turnSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }
}
