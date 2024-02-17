using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public static PlayerMovementController Instance { get; private set; }
    [SerializeField] Rigidbody ownRb;
    [SerializeField] Transform innerTransform;
    [SerializeField] Vector2 limits_horizontal = new Vector2(-5, 5);
    [SerializeField] float speed_horizontal = 1;
    [SerializeField] float speed_forward = 1;
    bool canMove_forward = false;
    bool canMove_horizontal = true;
    Vector2 movementChange = Vector2.zero;
    void Awake()
    {
        Instance = this;
    }

    public void SetMovementChange(Vector2 movementChange)
    {
        this.movementChange = movementChange;
    }

    public void StartToMove_Forward()
    {
        canMove_forward = true;
    }

    public void StartToMove_Horizontal()
    {
        canMove_horizontal = true;
    }

    public void StopMovement_Forward()
    {
        canMove_forward = false;
    }

    public void StopMovement_Horizontal()
    {
        canMove_horizontal = false;
    }

    private void Update()
    {
        ownRb.velocity = canMove_forward ? Vector3.forward * speed_forward : Vector3.zero;

        if (canMove_horizontal)
        {
            float lerpT = Mathf.InverseLerp(limits_horizontal.x, limits_horizontal.y, innerTransform.localPosition.x);
            lerpT = Mathf.Clamp01(lerpT + movementChange.x * speed_horizontal * Time.deltaTime);
            innerTransform.localPosition = new Vector3(Mathf.Lerp(limits_horizontal.x, limits_horizontal.y, lerpT), 0, 0);
        }
    }


}
