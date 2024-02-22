using DG.Tweening;
using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DispencerController : MonoBehaviour
{
    [SerializeField] string playerObjTag;
    [SerializeField] SplineFollower splineFollower;
    [SerializeField] Collider triggerToCheckPlayer;
    [SerializeField] int spawnCount = 10;
    [SerializeField] float flightY = 5;
    List<PlayerObjectForDispencer> playerObjects = new List<PlayerObjectForDispencer>();
    [SerializeField] Transform parentForWaitingPlayerObjs;
    [SerializeField] Transform visualTransform;
    bool spawn = false;
    [SerializeField] Transform propellersParent;

    private void Start()
    {
        if (ObjectPooler.Instance != null)
        {
            parentForWaitingPlayerObjs.gameObject.SetActive(false);
            float currentPercent = 0;
            float percentPerPlayerObj = 1f / (spawnCount - 1);
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnPos = splineFollower.spline.EvaluatePosition(currentPercent);
                spawnPos.y = flightY;
                GameObject temp = ObjectPooler.Instance.SpawnFromPool(playerObjTag, spawnPos, Quaternion.identity);
                temp.transform.parent = parentForWaitingPlayerObjs;
                playerObjects.Add(new PlayerObjectForDispencer(temp, currentPercent));
                currentPercent = Mathf.Min(1, currentPercent + percentPerPlayerObj);
            }
        }
    }

    private void OnDestroy()
    {
        playerObjects.ForEach(x => x.Disable());

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerToCheckPlayer.enabled = false;
            visualTransform.DOLocalMoveY(flightY, 0.25f).OnComplete(StartSpawning);
            propellersParent.DOLocalRotate(Vector3.up * 360, 300, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1).SetSpeedBased();
        }
    }

    void StartSpawning()
    {
        spawn = true;
        splineFollower.follow = true;
    }

    private void FixedUpdate()
    {
        if (spawn)
        {
            playerObjects.FindAll(x => x.CheckSpawn((float)splineFollower.GetPercent())).ForEach(x => x.Spawn());
        }
    }

    public void WhenSpawnerReachedToEnd()
    {
        visualTransform.DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });


    }
}

class PlayerObjectForDispencer
{
    public GameObject playerObjGO;
    public float percent;
    public bool spawned = false;

    public PlayerObjectForDispencer(GameObject playerObjGO, float percent)
    {
        this.playerObjGO = playerObjGO;
        this.percent = percent;
    }

    public void Disable()
    {
        if (!spawned)
            playerObjGO.transform.parent = null;
        playerObjGO.SetActive(false);
    }

    public bool CheckSpawn(float targetPercent)
    {
        if (spawned)
            return false;

        return percent <= targetPercent;
    }

    public void Spawn()
    {
        playerObjGO.transform.parent = null;
        spawned = true;
    }
}
