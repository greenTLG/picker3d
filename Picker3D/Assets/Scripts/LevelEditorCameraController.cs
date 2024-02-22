using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorCameraController : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float speedMultiplier = 2;
    [SerializeField] float rotSpeed = 1;
    private void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float upDown = Input.GetAxis("UpDown");
        bool sprint = Input.GetKey(KeyCode.LeftShift);
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, transform.eulerAngles + (Vector3.right * -mouseY + Vector3.up * mouseX), rotSpeed * Time.deltaTime);
        }
        else
            Cursor.lockState = CursorLockMode.None;

        transform.position = Vector3.Lerp(transform.position, transform.position + (transform.right * horizontal + transform.forward * vertical + transform.up * upDown), speed * (sprint ? speedMultiplier : 1) * Time.deltaTime);


    }
}
