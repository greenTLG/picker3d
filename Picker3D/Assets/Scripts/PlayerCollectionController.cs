using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectionController : MonoBehaviour
{
    public static PlayerCollectionController Instance { get; private set; }

    List<GameObject> collectedPlayerObjects = new List<GameObject>();
    [SerializeField] float pushPower = 10;
    [SerializeField] GameObject collectionCollidersParent, landingCollidersParent;
    [SerializeField] Transform[] armPositons = new Transform[2];
    List<GameObject> armGOs = new List<GameObject>();
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

    public void OpenCollectionColliders()
    {
        collectionCollidersParent.SetActive(true);
    }

    public void CloseCollectionColliders()
    {
        collectionCollidersParent.SetActive(false);
    }

    public void OpenLandingColliders()
    {
        landingCollidersParent.SetActive(true);
    }

    public void CloseLandingColliders()
    {
        landingCollidersParent.SetActive(false);
    }

    public void ResetPlayerCollection()
    {
        collectedPlayerObjects.FindAll(x => x != null).ForEach(x => x.SetActive(false));
        collectedPlayerObjects.Clear();
        CloseLandingColliders();
        OpenCollectionColliders();
    }

    public void SetArms(string armTag)
    {
        for (int i = 0; i < armPositons.Length; i++)
        {
            GameObject tempArm = ObjectPooler.Instance.SpawnFromPool(armTag, armPositons[i].position, armPositons[i].rotation);
            tempArm.transform.parent = armPositons[i];
            armGOs.Add(tempArm);
        }
    }

    public void DeleteArms()
    {
        armGOs.ForEach(x => x.transform.parent = null);
        armGOs.ForEach(x => x.gameObject.SetActive(false));
        armGOs.Clear();
    }
}
