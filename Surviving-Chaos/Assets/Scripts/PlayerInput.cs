using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Vertical;
    public float Horizontal;
    public float mouseX;
    public float mouseY;
    public bool Fire1;
    public bool Aim;
    public bool Reload;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;       // lock the cursor to the center of the screen
        Cursor.visible = false;
    }

    void Update()
    {
        Vertical = Input.GetAxis("Vertical");
        Horizontal = Input.GetAxis("Horizontal");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        Fire1 = Input.GetButton("Fire1");
        Aim = Input.GetButton("Fire2");
        Reload = Input.GetKeyDown("r");
    }
}
