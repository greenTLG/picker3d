using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    enum States
    {
        Waiting,
        Collecting,
        Resulting
    }

    [SerializeField] Transform successRoad;
    [SerializeField] float roadTargetLocalY = -0.5f;
    [SerializeField] float roadRaisingTime = 0.5f;
    [SerializeField] Ease roadRaisingEase = Ease.OutBack;
    [Space]
    [SerializeField] Transform[] blockers = new Transform[0];
    [SerializeField] float blockerTargetAngle = -90f;
    [SerializeField] float blockerOpeningTime = 0.5f;
    [SerializeField] Ease blockerOpeningEase = Ease.OutBounce;
    [Space]
    [SerializeField] TextMeshPro poolText;
    [SerializeField] string poolTextStr = "{value} / {target}";
    [Space]
    [SerializeField] float decisionDelay = 1;

    int collectedCount = 0;
    int targetCount = 10;
    bool canPass = false;
    States currentState = States.Waiting;
    public void WhenPlayerObjectCollected()
    {
        collectedCount++;
        UpdatePoolText();
        CheckTargetCount();
    }

    void UpdatePoolText()
    {
        if (poolText == null)
            return;

        poolText.text = poolTextStr.Replace("{value}", collectedCount.ToString()).Replace("{target}", targetCount.ToString());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentState == States.Waiting)
        {
            if (other.CompareTag("Player"))
            {
                PlayerMovementController.Instance.StopMovement_Forward();
                currentState = States.Collecting;
                CheckTargetCount();
            }
        }
    }

    public void SetTargetCount(int count)
    {
        targetCount = count;
        UpdatePoolText();
    }

    void CheckTargetCount()
    {
        if (currentState != States.Collecting)
            return;

        canPass = collectedCount >= targetCount;
        CancelInvoke("IsPlayerFailed");
        Invoke("IsPlayerFailed", decisionDelay);
    }

    void IsPlayerFailed()
    {
        if (currentState != States.Collecting)
            return;
        currentState = States.Resulting;
        if (canPass)
        {
            OpenBlockers();
            RaiseRoad();
            PlayerMovementController.Instance.Invoke("StartToMove_Forward", Mathf.Max(blockerOpeningTime, roadRaisingTime));
        }
        else
        {
            GameManager.PlayerFailed();
        }

    }

    void OpenBlockers()
    {
        for (int i = 0; i < blockers.Length; i++)
        {
            blockers[i].DOLocalRotate(new Vector3(blockers[i].localEulerAngles.x, blockers[i].localEulerAngles.y, blockerTargetAngle), blockerOpeningTime).SetEase(blockerOpeningEase);
        }
    }

    void RaiseRoad()
    {
        successRoad.gameObject.SetActive(true);
        successRoad.DOLocalMoveY(roadTargetLocalY, roadRaisingTime).SetEase(roadRaisingEase);
    }






}
