using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectionController : MonoBehaviour
{
    public static PlayerCollectionController Instance { get; private set; }

    List<GameObject> collectedPlayerObjects = new List<GameObject>();
    [SerializeField] float pushPower = 10;
    void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObject") && !collectedPlayerObjects.Contains(other.gameObject))
        {
            collectedPlayerObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerObject"))
        {
            collectedPlayerObjects.Remove(other.gameObject);
        }
    }

    public void PushCollectedPlayerObjects()
    {
        collectedPlayerObjects.ForEach(x => x.GetComponent<Rigidbody>().AddForce(Vector3.forward * pushPower));
    }
}
