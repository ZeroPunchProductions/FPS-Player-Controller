using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FPSCharacterController : MonoBehaviour    
{    public float walkSpeed = 7.5f;
    public float runSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera mainCam;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    CharacterController chrController;
    public Vector3 moveDir = Vector3.zero;
    float rotateX = 0;
    [HideInInspector]
    public bool isMove = true;

    void Start()
    {
        chrController = GetComponent<CharacterController>();

        // make it so mouse is not visible
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    void Update()
    {
        // When grounded recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
      
        
        // Press left shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = isMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = isMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDir.y;
        moveDir = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && isMove && chrController.isGrounded)
        {
            moveDir.y = jumpSpeed;
        }
        else
        {
            moveDir.y = movementDirectionY;
        }

        // applies gravity to the player character
        if (!chrController.isGrounded)
        {
            moveDir.y -= gravity * Time.deltaTime;
        }

        // allows the player controller to move based on the move direction
        chrController.Move(moveDir * Time.deltaTime);

        // if you are grounded and can move calculate player rotation and camera rotation
        if (isMove)
        {
            rotateX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotateX = Mathf.Clamp(rotateX, -lookXLimit, lookXLimit);
            mainCam.transform.localRotation = Quaternion.Euler(rotateX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}

