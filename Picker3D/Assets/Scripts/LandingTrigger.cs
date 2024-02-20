using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LandingTrigger : MonoBehaviour
{
    [SerializeField] Collider ownCollider;
    [SerializeField] TextMeshPro valueText;
    [SerializeField] FinishController ownFinishController;
    [SerializeField] int value;

    private void Start()
    {
        ownFinishController.AddLandingTrigger(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ownCollider.enabled = false;
            ownFinishController.WhenPlayerEnteredLandingTrigger(this);
        }
    }

    public int GetValue()
    {
        return value;
    }

    public void PlayerEarnedTheValue()
    {
        MoneyManager.Instance.EarnMoney(value, transform.position,false,UIManager.MoneySendingTypes.Parallel);
    }

    public void Shrink()
    {
        transform.DOScale(0, 0.5f);
    }

    private void OnValidate()
    {
        if (valueText != null)
            valueText.text = value.ToString();
    }
}
