using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerObjectContainerController : MonoBehaviour
{
    [SerializeField] Collider playerTrigger;
    [SerializeField] Collider spawnCollider;
    [SerializeField] string[] playerObjTags = new string[0];
    [SerializeField] int playerObjCount = 7;
    [SerializeField] Vector3 localMovementTarget;
    [SerializeField] float movementTime = 1;
    [SerializeField] float movementDelay = 0;
    [SerializeField] Ease movementEase = Ease.Linear;
    List<GameObject> spawnedPlayerObjects = new List<GameObject>();
    enum States
    {
        Waiting,
        Moving,
        Exploded
    }

    States currentState = States.Waiting;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentState == States.Waiting)
            {
                StartToMove();
            }
            else if (currentState == States.Moving)
            {
                Explode();
            }
        }
    }

    void Explode()
    {
        if (currentState == States.Exploded)
            return;
        currentState = States.Exploded;
        for (int i = 0; i < playerObjCount; i++)
        {
            spawnedPlayerObjects.Add(ObjectPooler.Instance.SpawnFromPool(playerObjTags[Random.Range(0, playerObjTags.Length)], spawnCollider.bounds.RandomPointInBounds(), Random.rotation));
        }
        gameObject.SetActive(false);
        Taptic.Medium();
    }

    void StartToMove()
    {
        currentState = States.Moving;
        playerTrigger.enabled = false;
        transform.DOMove(transform.position + localMovementTarget, movementTime).SetDelay(movementDelay).SetEase(movementEase).OnComplete(Explode);
    }

    private void OnDestroy()
    {
        spawnedPlayerObjects.FindAll(x => x != null && x.activeSelf).ForEach(x => x.SetActive(false));
    }

}
