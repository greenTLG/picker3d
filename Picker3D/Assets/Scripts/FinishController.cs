using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour
{
    enum States
    {
        Waiting,
        ClimbingTheRamp,
        Flying,
        SendingPlayerToNextLevel
    }
    [SerializeField] SplineProjector splineProjector;
    [SerializeField] Collider ownCollider;
    [SerializeField] float speed = 1;
    States currentState = States.Waiting;
    float currentRampPercent = 0;
    [SerializeField] float speedUpVal = 0.1f;
    [SerializeField] float throwingIncreaseVal = 0.05f;
    [SerializeField] float throwingDecreaseVal = 0.025f;
    float currentThrowingVal = 0;
    [SerializeField] AnimationCurve throwingValueCurve = AnimationCurve.Linear(0, 0, 1, 1);
    float throwingPercent = 0;
    float throwingAngle = 30;
    [SerializeField] Vector2 throwingRange;
    LandingTrigger currentHighestLandingTrigger;
    List<LandingTrigger> landingTriggers = new List<LandingTrigger>();
    private void OnTriggerEnter(Collider other)
    {
        if (currentState == States.Waiting)
        {
            if (other.CompareTag("Player"))
            {
                StartToClimbTheRamp();
            }
        }
    }

    void StartToClimbTheRamp()
    {
        currentRampPercent = 0;
        currentState = States.ClimbingTheRamp;
        UpdateSplineProjector();
        PlayerMovementController.Instance.StopMovement_Forward();
        PlayerMovementController.Instance.StopMovement_Horizontal();
        splineProjector.targetObject = PlayerMovementController.Instance.gameObject;
        ownCollider.enabled = false;
        PlayerInputHandler.Instance.SetFinishController(this);
    }

    private void Update()
    {
        if (currentState == States.ClimbingTheRamp)
        {
            currentRampPercent += speed * Time.deltaTime;

            UpdateSplineProjector();

            throwingPercent -= throwingDecreaseVal * Time.deltaTime;
            UpdateThrowingVal();
        }
    }

    public void SpeedUp()
    {
        if (currentState != States.ClimbingTheRamp)
            return;

        currentRampPercent += speedUpVal;

        throwingPercent += throwingIncreaseVal;
        UpdateThrowingVal();
    }

    void UpdateSplineProjector()
    {
        if (currentState != States.ClimbingTheRamp)
            return;


        splineProjector.SetPercent(currentRampPercent);

        if (Mathf.Abs(1 - currentRampPercent) < 0.05f)
        {
            StartFlying();
        }
    }

    void StartFlying()
    {
        PlayerInputHandler.Instance.SetFinishController(null);
        currentState = States.Flying;

        Vector3 playerPos = PlayerMovementController.Instance.transform.position;
        Vector3 targetPos = new Vector3(playerPos.x, 0, playerPos.z + Mathf.Lerp(throwingRange.x, throwingRange.y, currentThrowingVal));
        float distToTarget = Vector3.Distance(targetPos, new Vector3(playerPos.x, 0, playerPos.z));
        float gravity = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(throwingAngle * Mathf.Deg2Rad);
        float heightDiff = targetPos.y - playerPos.y;
        float vZ = Mathf.Sqrt(gravity * distToTarget * distToTarget / (2 * (heightDiff - distToTarget * tanAlpha)));
        float vY = tanAlpha * vZ;
        Vector3 throwingVelocity = PlayerMovementController.Instance.transform.TransformDirection(new Vector3(0, vY, vZ));
        PlayerCollectionController.Instance.CloseCollectionColliders();
        PlayerCollectionController.Instance.OpenLandingColliders();
        PlayerMovementController.Instance.Throw(throwingVelocity);

    }

    void UpdateThrowingVal()
    {
        throwingPercent = Mathf.Clamp01(throwingPercent);
        currentThrowingVal = throwingValueCurve.Evaluate(throwingPercent);
        //Update UI
    }

    public void WhenPlayerEnteredLandingTrigger(LandingTrigger landingTrigger)
    {
        if (currentHighestLandingTrigger == null || landingTrigger.GetValue() > currentHighestLandingTrigger.GetValue())
        {
            currentHighestLandingTrigger = landingTrigger;
            Invoke("LandingFinished", 1.5f);
        }
    }

    void LandingFinished()
    {
        currentHighestLandingTrigger.PlayerEarnedTheValue();
        landingTriggers.ForEach(x => x.Shrink());
        SendPlayerToNextLevel();
    }

    void SendPlayerToNextLevel()
    {
        currentState = States.SendingPlayerToNextLevel;
        PlayerMovementController.Instance.StopMovement_Forward();
        PlayerMovementController.Instance.StopMovement_Horizontal();
        PlayerMovementController.Instance.ResetRigidbody();
        PlayerCollectionController.Instance.CloseLandingColliders();
        PlayerCollectionController.Instance.OpenCollectionColliders();

    }

    public void AddLandingTrigger(LandingTrigger landingTrigger)
    {
        if (landingTrigger == null)
            return;
        landingTriggers.Add(landingTrigger);
    }








}
