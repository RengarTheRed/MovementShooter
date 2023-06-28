using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    
    //Rotates playerbody
    public Transform playerBody;
    
    //Multiplier and variables to set
    public float mouseSensitivity = 400f;
    private float xRotate = 0f;
    
    //Start lock cursor to game
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    //Each frame check mouse input and applies rotation to body and move camera up/downs
    void Update()
    {
        var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        xRotate -= mouseY;
        xRotate = Mathf.Clamp(xRotate, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotate, 0f, 0f);
        playerBody.Rotate(Vector3.up, mouseX);
    }
}
