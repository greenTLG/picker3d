using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckpointTargetCountSetter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI targetCountText;
    CheckpointController checkpoint;
    int targetCount;

    public void SetCheckpoint(CheckpointController checkpoint)
    {
        this.checkpoint = checkpoint;
        targetCount = checkpoint.GetTargetCount();
        UpdateTargetCountText();
    }

    void UpdateTargetCountText()
    {
        targetCountText.text = targetCount.ToString();
    }

    public void IncreaseTargetCount()
    {
        targetCount++;
        SetCheckpointTargetCount();
        UpdateTargetCountText();
    }
    public void DecreaseTargetCount()
    {
        if (targetCount == 0)
            return;
        targetCount--;
        SetCheckpointTargetCount();
        UpdateTargetCountText();
    }

    void SetCheckpointTargetCount()
    {
        if (checkpoint == null)
            return;
        checkpoint.SetTargetCount(targetCount);
    }


}
