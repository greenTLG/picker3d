using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPoolController : MonoBehaviour
{
    [SerializeField] CheckpointController ownCheckpoint;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerObject"))
        {
            ownCheckpoint.WhenPlayerObjectCollected();
            collision.gameObject.GetComponent<PlayerObjectController>().WhenCollected();
        }
    }
}
