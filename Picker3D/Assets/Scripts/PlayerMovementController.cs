using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public static PlayerMovementController Instance { get; private set; }
    [SerializeField] Rigidbody ownRb;
    [SerializeField] Vector2 limits_horizontal = new Vector2(-5, 5);
    [SerializeField] float speed_horizontal = 1;
    [SerializeField] float speed_forward = 1;
    bool canMove_forward = false;
    bool canMove_horizontal = true;
    Vector2 movementChange = Vector2.zero;
    bool isFlying = false;
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

    private void FixedUpdate()
    {
        Vector3 velocity = Vector3.zero;
        if (canMove_forward)
        {
            velocity.z = speed_forward;
        }

        if (canMove_horizontal)
        {
            if (CheckSideLimits())
                velocity.x = speed_horizontal * movementChange.x;
        }
        if (!isFlying)
            ownRb.velocity = velocity;
    }

    bool CheckSideLimits()
    {
        if (movementChange.x > 0)
        {
            if (transform.position.x + 0.15f >= limits_horizontal.y)
            {
                return false;
            }
        }
        else
        {
            if (transform.position.x - 0.15f <= limits_horizontal.x)
            {
                return false;
            }
        }

        return true;
    }

    public void Throw(Vector3 throwingVelocity)
    {
        isFlying = true;
        ownRb.constraints = RigidbodyConstraints.None;
        ownRb.velocity = throwingVelocity;
    }

    public void ResetRigidbody()
    {
        isFlying = false;
        ownRb.velocity = Vector3.zero;
        ownRb.angularVelocity = Vector3.zero;
        ownRb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    public void ResetPlayerMovement()
    {
        ResetRigidbody();
        StopMovement_Forward();
        StopMovement_Horizontal();
    }
}
