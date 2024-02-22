using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmPowerUp : MonoBehaviour
{
    [SerializeField] string armTag = "Arm";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCollectionController.Instance.SetArms(armTag);
            gameObject.SetActive(false);
        }
    }
}
